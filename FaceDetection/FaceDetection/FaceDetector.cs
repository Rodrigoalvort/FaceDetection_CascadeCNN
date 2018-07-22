using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Microsoft.MSR.CNTK.Extensibility.Managed;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Windows.Media;


namespace FaceDetection
{
   
    class FaceDetector
    {

        
        /// <summary>
        ///  Canvas  for show the defections for default 
        /// </summary>
        System.Windows.Controls.Canvas Cv1; 
        /// <summary>
        /// Dinctionary of arrays to be evaluate in the classfiers
        /// </summary>
        Dictionary<string,float[]> array; 
        /// <summary>
        /// edge of the Classifier's iput images   
        /// </summary>
        Dictionary<string, int> edge;  
 
        /// <summary>
        ///  Classifiers Face or no face
        /// </summary>
        Dictionary<string, NetEvaluation> Classifier;

        /// <summary>
        ///   this object contain the detections of the last image evaluated
        /// </summary>
       public Dictionary<string, BoundingBoxes> BoundingBox;

       public Dictionary<string, System.Windows.Media.Brush> bbColor; 
        /// <summary>
        ///   factor to define the  stride of the sliding window 
        /// </summary>
        float strideFactor;  
        
        /// <summary>
        /// Width of the input image
        /// </summary>
        int ImageWidth;    

        /// <summary>
        /// height of the input image
        /// </summary>
        int ImageHeight;  

        /// <summary>
        /// Upper Limit of the window 
        /// </summary>
        int Limit;        

        /// <summary>
        /// stride of the sliding window
        /// </summary>
        int stride;  

        /// <summary>
        /// edge of the sliding Window
        /// </summary>
        float scale;     

        /// <summary>
        /// initial scale of the sliding window
        /// </summary>
        int initialScale;

        float ScaleResizeRate;
 
        int colorIndex;    
        /// <summary>
        /// Color list
        /// </summary>
        List<Brush> colors;

        /// <summary>
        ///  Count how many windows was evaluated
        /// </summary>
        public int cnt;
        
        /// <summary>
        /// Face detector instance
        /// </summary>
        /// <param name="c"> Get  a default canvas to show result</param>
        /// <param name="Scale"> Initial scale</param>
        /// <param name="StrideFactor"> divisor of the scale to define the stride of the sliding window</param>


        public FaceDetector(System.Windows.Controls.Canvas c, int Scale,float StrideFactor, float scaleResizeRate)
        {
            ScaleResizeRate = scaleResizeRate;
            Classifier  =new Dictionary<String,NetEvaluation>();
            edge        =new Dictionary<String, int>();
            array       = new Dictionary<string, float[]>();
            bbColor  = new Dictionary<string, System.Windows.Media.Brush>();
            scale       = Scale;
            strideFactor=1/StrideFactor;
            
            Cv1 = c;
            BoundingBox = new Dictionary<string, BoundingBoxes>();// BoundingBoxes();
            initialScale = Scale;
            colorIndex = 0;
            colors = new List<System.Windows.Media.Brush>()
            {   
                Brushes.GreenYellow,
                Brushes.Orange,
                Brushes.BlueViolet,
                Brushes.Red,
                Brushes.LightBlue,
                Brushes.Yellow
            };
        }

        /// <summary>
        /// Add a classifier
        /// </summary>
        /// <param name="Key"> key of the classifier</param>
        /// <param name="Net"> classifier</param>
        /// <param name="Edge">Borde</param>
        public void AddClassifier(string Key, NetEvaluation Net, int Edge)
        {
            AddClassifier(Key, Net, Edge, colors[colorIndex]);    
        }
        public void AddClassifier(string Key, NetEvaluation Net, int Edge, System.Windows.Media.Brush BbColor)
        {
            Classifier.Add(Key, Net);
            edge.Add(Key, Edge);
            array.Add(Key, new float[Edge * Edge * 3]);
            BoundingBox.Add(Key, new BoundingBoxes());
            bbColor.Add(Key, BbColor);
        }
        /// <summary>
        /// Scan an imag with a single classifier 
        /// </summary>
        /// <param name="image"> image to scan</param>
        /// <param name="Treshold">theshold for the decision </param>
        /// <param name="key"> key of the classifier</param>
        public void ScanImageSingleClassifier(Image<Bgr, Byte> image, float  Treshold,string key)
        {
            cnt = 0;
            ImageHeight = image.Height;
            ImageWidth = image.Width;
            clearList();      
            if (image.Height>= image.Width)   Limit=image.Width;   else      Limit=image.Height;
            scale = initialScale;
            int idx0_0 = 0, idx1_0 = edge[key] * edge[key], idx2_0 = 2 * edge[key] * edge[key];
            int idx0, idx1, idx2;
            float  subsample;
            while (scale <= Limit)
            {
                subsample = ((float)scale + 1) / (float)edge[key];
                stride = (int)(scale * strideFactor);
                for (int x = 0; x <= image.Width - scale; x += stride)
                {
                    for (int y = 0; y <= image.Height - scale; y += stride)
                    {
                        cnt++;
                        idx0 = idx0_0;
                        idx1 = idx1_0;
                        idx2 = idx2_0;
                        int i_i, j_i;
                        for (float j = y; j < y + scale; j += subsample)
                        {

                            j_i = (int)j;
                            for (float i = x; i < x + scale; i += subsample)
                            {
                                i_i = (int)i;
                                array[key][idx0++] = (float)image[j_i, i_i].Red;
                                array[key][idx1++] = (float)image[j_i, i_i].Green;
                                array[key][idx2++] = (float)image[j_i, i_i].Blue;
                            }
                        }
                       DecisionFaceOrNoFaceSingleClasifier(key,Classifier[key].simulateWithSoftmax(array[key]), Treshold, x, y, scale);

                    }
                }
                scale =scale*ScaleResizeRate;
            }
        }

        /// <summary>
        /// Scan and image an not combine closest detections
        /// </summary>
        /// <param name="image"> image to scan</param>
        /// <param name="Treshold"> theshold face or no face  0:1</param>
        /// <param name="key"> clasifier key</param>
        public void ScanImageMultiClassifier(Image<Bgr, Byte> image, float Treshold, string key)
        {
            ImageHeight = image.Height;
            ImageWidth = image.Width;
            clearList();
            if (image.Height >= image.Width) Limit = image.Width; else Limit = image.Height;
            scale = initialScale;
            int idx0_0 = 0, idx1_0 = edge[key] * edge[key], idx2_0 = 2 * edge[key] * edge[key];
            int idx0, idx1, idx2;
            float subsample;
            while (scale <= Limit)
            {
                subsample = ((float)scale+1) / (float)edge[key];
                stride = (int)(scale * strideFactor);
                for (int x = 0; x <= image.Width - scale; x += stride)
                {
                    for (int y = 0; y <= image.Height - scale; y += stride)
                    {
                        cnt++;
                        idx0 = idx0_0;
                        idx1 = idx1_0;
                        idx2 = idx2_0;
                        int i_i, j_i;
                        for (float j = y; j < y + scale; j += subsample)
                        {

                            j_i = (int)j;
                            for (float i = x; i < x + scale; i += subsample)
                            {
                                i_i = (int)i;
                                array[key][idx0++] = (float)image[j_i, i_i].Red;
                                array[key][idx1++] = (float)image[j_i, i_i].Green;
                                array[key][idx2++] = (float)image[j_i, i_i].Blue;
                            }
                        }
                   DecisionFaceOrNoFaceMultiClasifier(key,Classifier[key].simulateWithSoftmax(array[key]), Treshold, x, y, scale);
                
                    }
                }
                scale = scale*ScaleResizeRate;
            }
        }

        public void ScanImageWithCascadeClassifier(Image<Bgr, Byte> image, Dictionary<string, float> classifierandFloat)
        {
            var keys = classifierandFloat.Keys.ToArray();
            var thds = classifierandFloat.Values.ToArray();

            if (classifierandFloat.Count > 1)
            {
                ScanImageMultiClassifier(image, classifierandFloat.Values.First(), classifierandFloat.Keys.First());


                for (int i = 1; i < keys.Length - 1; i++)
                {
                    foreach (bb b in BoundingBox[keys[i - 1]].list)
                    {
                        DecisionFaceOrNoFaceMultiClasifier(keys[i],
                                                           EvaluatesinglePosition(image, b.x, b.y, b.scale, keys[i]),
                                                           thds[i],
                                                           b.x,
                                                           b.y,
                                                           b.scale);
                    }

                }

                foreach (bb b in BoundingBox[keys[keys.Length - 2]].list)
                {
                    DecisionFaceOrNoFaceSingleClasifier(keys.Last(),
                                                         EvaluatesinglePosition(image, b.x, b.y, b.scale, keys.Last()),
                                                         thds.Last(),
                                                         b.x,
                                                         b.y,
                                                         b.scale);
                }
            }
            else
            {
                ScanImageSingleClassifier(image, classifierandFloat.Values.First(), classifierandFloat.Keys.First());

            }
            BoundingBox[keys.Last()].Check();

        }
        

        /// <summary>
        /// Evaluate single position of tje image
        /// </summary>
        /// <param name="image">image</param>
        /// <param name="x"> position in x</param>
        /// <param name="y"> position in y</param>
        /// <param name="Scale"> height and width of the window</param>
        /// <param name="key">classifier key</param>
        /// <returns></returns>
        public float [] EvaluatesinglePosition(Image<Bgr,Byte> image, float x, float y, float Scale, string key )
        {
        float  subsample = ((float)Scale+1) / (float)edge[key];


         int   idx0 = 0;
         int   idx1 = edge[key] * edge[key];
         int   idx2 = edge[key] * edge[key] * 2 ;
         int i_i, j_i;
            for (float j = y; j < y + Scale; j += subsample)
            {

                j_i = (int)j;
                for (float i = x; i < x + Scale; i += subsample)
                {
                    i_i = (int)i;
                    array[key][idx0++] = (float)image[j_i, i_i].Red;
                    array[key][idx1++] = (float)image[j_i, i_i].Green;
                    array[key][idx2++] = (float)image[j_i, i_i].Blue;
                }
            }
            return  Classifier[key].simulateWithSoftmax(array[key]);


        }



        /// <summary>
        /// Clear lists of detections
        /// </summary>
        public void clearList()
        {
            foreach (BoundingBoxes bbxs in BoundingBox.Values)
            {
                if (bbxs.list.Count > 0)
                {
                    foreach (bb B in bbxs.list) B.ChangeVisibility();

                    bbxs.list.Clear();

                }
            }

        }

        /// <summary>
        /// clear list of a clasifier
        /// </summary>
        /// <param name="Key">classifier key</param>
        public void clearList(string Key)
        {
                if (BoundingBox[Key] .list.Count > 0)
                {

                    foreach (bb B in BoundingBox[Key].list) B.ChangeVisibility();

                    BoundingBox[Key].list.Clear();

                }
            

        }

        /// <summary>
        /// Show the detection of a classifier
        /// </summary>
        /// <param name="Key">classifier key</param>
        public void ShowList(string Key)
        {
            ShowList(Key,Cv1);


        }

        /// <summary>
        /// Show classifier's detections in a defined canvas
        /// </summary>
        /// <param name="Key">classifier key</param>
        /// <param name="canvas"> Canvas to show result</param>
        public void ShowList(string Key,System.Windows.Controls.Canvas canvas)
        {
            if (BoundingBox[Key].getListLength() != 0)
            {
                BoundingBox[Key].Check();

                foreach (bb B in BoundingBox[Key].list) B.addToCanvas(canvas);


            }

        }

        public void ShowListWithoutCombine(string Key, System.Windows.Controls.Canvas canvas)
        {
            if (BoundingBox[Key].getListLength() != 0)
            {
                foreach (bb B in BoundingBox[Key].list) B.addToCanvas(canvas);
            }

        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="key"></param>
        /// <param name="a"></param>
        /// <param name="treshold"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="scale"></param>
        private   void DecisionFaceOrNoFaceSingleClasifier(string key,float[] a, float  treshold, float x,float y,float scale)
        {
            if (a[1] > treshold)
            {
              bb bb1 = new bb(x, y, scale, bbColor[key],a[1],1);
                BoundingBox[key].AddAndCombine(bb1);
            
            }


        }

        private void DecisionFaceOrNoFaceMultiClasifier(string key,float[] a, float treshold, float x, float y, float scale)
        {
            if (a[1] > treshold)
            {
                bb bb1 = new bb(x, y, scale, bbColor[key], a[1], 1);
                BoundingBox[key].Add(bb1);
            }


        }


        
        public  class BoundingBoxes
        {

            public  List<bb> list;

            private List<Brush> colors;  //lista de colores 
            

            public BoundingBoxes()
            {
                list = new List<bb>();
                colors = new List<System.Windows.Media.Brush>()
            {   
                Brushes.GreenYellow,
                Brushes.Orange,
                Brushes.BlueViolet,
                Brushes.Red,
                Brushes.LightBlue,
                Brushes.Yellow
            };
            }

            public void Check()
            {
                List<bb> CheckedList = new List<bb>();
                for (int i = 0; i < list.Count; i++)
                {
                    CheckedList = Combine(list[i], CheckedList);
                        
                }
                list = CheckedList;
            }

            public void Add(bb BoundingBox)
            {
                list.Add(BoundingBox);
            }

            public void AddAndCombine(bb Newbb)
            {
                bool Combined =false;
                for (int i = 0; i < list.Count; i++ )
                {
                    bb Oldbb = list[i];
                    if (Combined != true)
                    {
                        if ((Oldbb.Left <= Newbb.Left && Oldbb.Right > Newbb.Left) ||
                            (Newbb.Left <= Oldbb.Left && Oldbb.Left < Newbb.Right))
                        {
                            if ((Oldbb.Top <= Newbb.Top && Oldbb.Bottom > Newbb.Top) ||
                                (Newbb.Top <= Oldbb.Top && Oldbb.Top < Newbb.Bottom))
                            {
                                float maxleft = (float)selectmax(Oldbb.Left, Newbb.Left);
                                float minright = (float)selectmin(Oldbb.Right, Newbb.Right);
                                float maxtop = (float)selectmax(Oldbb.Top, Newbb.Top);
                                float minbottom = (float)selectmin(Oldbb.Bottom, Newbb.Bottom);

                                float area = ((minright - maxleft) * (minbottom - maxtop));
                                float area1 = area/(Newbb.scale * Newbb.scale);
                                float area2 = area/(Oldbb.scale * Oldbb.scale);
                                if (0.20 <= area1  && 0.20 <= area2 )
                                {
                                    Combined = true;
                                    list[i] = MeanCombine(Oldbb, Newbb);
                                }
                            }
                        }

                    }
                }

                if (Combined != true)
                {
                    Add(Newbb);
                }
            }

            public List<bb> Combine(bb Newbb, List<bb> checkedList)
            {
                bool Combined = false;
                for (int i = 0; i < checkedList.Count; i++)
                {
                    bb Oldbb = checkedList[i];
                    if (Combined != true)
                    {
                        if ((Oldbb.Left <= Newbb.Left && Oldbb.Right > Newbb.Left) ||
                            (Newbb.Left <= Oldbb.Left && Oldbb.Left < Newbb.Right))
                        {
                            if ((Oldbb.Top <= Newbb.Top && Oldbb.Bottom > Newbb.Top) ||
                                (Newbb.Top <= Oldbb.Top && Oldbb.Top < Newbb.Bottom))
                            {
                                float maxleft = (float)selectmax(Oldbb.Left, Newbb.Left);
                                float minright = (float)selectmin(Oldbb.Right, Newbb.Right);
                                float maxtop = (float)selectmax(Oldbb.Top, Newbb.Top);
                                float minbottom = (float)selectmin(Oldbb.Bottom, Newbb.Bottom);

                                float area = ((minright - maxleft) * (minbottom - maxtop));
                                float area1 = area / (Newbb.scale * Newbb.scale);
                                float area2 = area / (Oldbb.scale * Oldbb.scale);
                                if (0.20 <= area1 && 0.20 <= area2)
                                {
                                    Combined = true;
                                    checkedList[i] = MeanCombine(Oldbb, Newbb);
                                }
                            }
                        }

                    }
                }

                if (Combined != true)
                {
                   checkedList.Add(Newbb);
                }
                return checkedList;
            }

            private bb MeanCombine(bb OldBb, bb  NewBb)
            {
                float scale = (OldBb.scale * OldBb.weight + NewBb.scale * NewBb.weight) / (OldBb.weight + NewBb.weight);
                float x       = (OldBb.x * OldBb.weight + NewBb.x * NewBb.weight) / (OldBb.weight + NewBb.weight);
                float y       = (OldBb.y * OldBb.weight + NewBb.y * NewBb.weight) / (OldBb.weight + NewBb.weight);
                float score = (OldBb.score  * OldBb.weight + NewBb.score * NewBb.weight) / (OldBb.weight + NewBb.weight);
                float Weight  = OldBb.weight + NewBb.weight;
                return new bb(x, y, scale, colors[1],score,Weight); 
                    
            }

            private bb MaxCombine(bb OldBb, bb NewBb)
            {
                if (OldBb.scale==NewBb.scale)
                {
                    return MeanCombine(OldBb, NewBb);
                }

                if ((NewBb.Left <= OldBb.Left) && (NewBb.Right >= OldBb.Right) && (NewBb.Top <= OldBb.Top) && (NewBb.Bottom >= OldBb.Bottom))
                {
                    float x = selectmin(OldBb.x, NewBb.x);
                    float y = selectmin(OldBb.y, NewBb.y);
                    float scale = selectmax(OldBb.scale, NewBb.scale);
                    float score = (OldBb.score * OldBb.weight + NewBb.score * NewBb.weight) / (OldBb.weight + NewBb.weight);
                    float Weight = OldBb.weight + NewBb.weight;
                    return new bb(x, y, scale, colors[2], score, Weight);
                }
                else
                {
                return MeanCombine(OldBb, NewBb);
                }
            }

            

            private float selectmin(float x, float y)
            {
                if (x <= y)  return x;    else    return y; 
            }

            private float selectmax(float x, float y)
            {
                if (x <= y) return y; else return x;
            }

            public int getListLength()
            {
                return list.Count;
            }
            


        }

        public struct bb
        {
            Rectangle rec;
            System.Windows.Controls.TextBlock textBlock;
            public float x;
            public float y;
            public float scale;
            public float score;
            public float Left, Right, Top, Bottom, weight;
            public bb(float X, float Y, float Scale, Brush color,float Score,float Weight)
            {
                x = X;
                y = Y;
                scale = Scale;
                score = Score;
                Left = x;
                Right = x + (int)scale;
                Top = y;
                Bottom = y + (int)scale;
                weight = Weight;
                rec = new Rectangle()
                {
                    Width = Scale,
                    Height = Scale,
                    Stroke = color,
                    StrokeThickness=2,
                    Visibility = System.Windows.Visibility.Visible,
                    //                        Fill = System.Windows.Media.Brushes.Blue
                };
                textBlock= new System.Windows.Controls.TextBlock()
                {
                Text = score.ToString("0.000")+" - " + weight.ToString(),
                FontSize = 9,
                Foreground = color,
                
                };
            }

            public void addToCanvas(System.Windows.Controls.Canvas C)
            {
                C.Children.Add(rec);
                System.Windows.Controls.Canvas.SetTop(rec, y);
                System.Windows.Controls.Canvas.SetLeft(rec, x);

                C.Children.Add(textBlock);
                System.Windows.Controls.Canvas.SetLeft(textBlock, x +2);
                System.Windows.Controls.Canvas.SetTop(textBlock, y  +2);
        
            }


            public void ChangeVisibility()
            {
                if (rec.Visibility != System.Windows.Visibility.Hidden)
                {
                    rec.Visibility = System.Windows.Visibility.Hidden;
                    textBlock.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    rec.Visibility = System.Windows.Visibility.Hidden;
                    textBlock.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }

    }
}
    