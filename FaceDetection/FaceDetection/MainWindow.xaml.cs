using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;

namespace FaceDetection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FDDB fddb;

    
        #region variables globales 
        CaptureWebCam CW;

        FaceDetector EV;

        NetEvaluation FDC32_2;

        NetEvaluation FDC32;

        NetEvaluation FDC24;
        
        NetEvaluation FDC16;
        
        NetEvaluation FDC12;

        Dictionary<string, float> CascadeFaceDetectorDic;
        string[] images;

        int idx;

        #endregion 
        public MainWindow()
        {
            InitializeComponent();

            #region instanciar variables globales
            string modelpath;
            
           CW = new CaptureWebCam(Image1);
        
            modelpath = @"E:\DeepLearning\Faces12Final\01_Convolution.293";
        
            //   modelpath = @"E:\DeepLearning\Faces12Final\02_BatchNormConv.417";
            FDC12 = new NetEvaluation(modelpath);

            //modelpath = @"E:\DeepLearning\cntk\Examples\FaceDetection16X16X3_VF\Output\Models\01_Convolution.77";
            modelpath = @"E:\DeepLearning\cntk\Examples\FaceDetection16X16X3_VF\Output\Models\01_Convolution.16";
            FDC16 = new NetEvaluation(modelpath);

            modelpath = @"E:\DeepLearning\cntk\Examples\FaceDetection24X24X3V2\Output\Models\01_Convolution.21";
            FDC24 = new NetEvaluation(modelpath);

            modelpath = @"E:\DeepLearning\cntk\Examples\FaceDetectionModels\Models\01_Convolution.100";
            FDC32 = new NetEvaluation(modelpath);





            modelpath = @"E:\DeepLearning\cntk\Examples\FaceDetection32X32X3\Output\Models\01_Convolution2.63";
            FDC32_2 = new NetEvaluation(modelpath);

            EV  = new FaceDetector(Canvas1,128, 4,1.5f);
            EV.AddClassifier("322", FDC32_2, 32);
            EV.AddClassifier("32", FDC32   , 24);
            EV.AddClassifier("24", FDC24   , 24,Brushes.SkyBlue);
            EV.AddClassifier("16", FDC16   , 16,Brushes.Violet);
            EV.AddClassifier("12", FDC12   , 12,Brushes.YellowGreen);
           
            
            CascadeFaceDetectorDic = new Dictionary<string, float>();
            CascadeFaceDetectorDic.Add("12", 0.4f);
            CascadeFaceDetectorDic.Add("24", 0.6f);
           //s CascadeFaceDetectorDic.Add("322",0.645f);
//      
               
            #endregion

            #region procesos temporales
          //  Utils.ReadFDDBFile(@"E:\Bases de datos\FDDB\FDDB-folds\FDDB-fold-01-ellipseList.txt");
          //  /    Utils.printOutputAndTargetList(FDC16.ReadUciFastReaderAndEvaluate(@"E:\Bases de datos\DatasetFacedetectionUCIreader\facedetector16X16X3Test.txt"));
          //fddb = new FDDB(EV);
          // fddbdatagenerate();
           
            #endregion
       }
        #region Captura
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if (EV != null)
            {
                if (CW != null)
                {
                    DateTime tiempo1 = DateTime.Now;

                    EV.ScanImageWithCascadeClassifier(CW.ImageFrame, CascadeFaceDetectorDic);
             //       EV.ShowListWithoutCombine("12", Canvas1);
                    EV.ShowList(CascadeFaceDetectorDic.Keys.Last());

                    DateTime tiempo2 = DateTime.Now;
                    TimeSpan total = new TimeSpan(tiempo2.Ticks - tiempo1.Ticks);
                    TB1.Text = total.ToString()+ "  "+EV.cnt;            

                }
            }
        }

        #endregion

        #region Archivo
        
        
        private void Sig_Click(object sender, RoutedEventArgs e)
        {
            if (images == null)
            {
                images = System.IO.Directory.GetFiles(@"E:\Bases de datos\FDDBppm\2002\07\25\big", "*.ppm");
                idx = 0;
            }


            if (idx >= images.Length) idx = 0;

            Image<Bgr, Byte> im1 = new Image<Bgr, byte>(images[idx++]); ;
            if (im1 != null)
            {

                ImageArchivo.Width = im1.Width;
                ImageArchivo.Height = im1.Height;
                ImageArchivo.Source = CaptureWebCam.BitmapToImageSource(im1.ToBitmap());
                EV.ScanImageWithCascadeClassifier(im1, CascadeFaceDetectorDic);
                
        //       EV.ShowListWithoutCombine("12", CanvasArchivo);
          //     EV.ShowListWithoutCombine("24", CanvasArchivo);
                EV.ShowListWithoutCombine(CascadeFaceDetectorDic.Keys.Last(), CanvasArchivo);
//                EV.ShowList(CascadeFaceDetectorDic.Keys.Last(), CanvasArchivo);
            
            }


        }

        #endregion

        #region afwl

        private void Siguiente_Click(object sender, RoutedEventArgs e)
        {
            if (images==null)
            {
             //   images = System.IO.Directory.GetFiles(@"E:\Bases de datos\AFWL\aflw\data\flickr", "*.jpg");
                images = System.IO.Directory.GetFiles(@"E:\Bases de datos\AFW\testimages", "*.jpg");
                images = System.IO.Directory.GetFiles(@"E:\Bases de datos\AFW", "*.jpg");
           
                //images = System.IO.Directory.GetFiles(@"E:\Bases de datos\ImageNet\Imagenes", "*.jpg");

                images = System.IO.Directory.GetFiles(@"C:\Users\Ideapad 300\Pictures", "*.jpg");
                idx = 0;
            }

            if (idx >= images.Length) idx = 0;

            Image<Bgr, Byte> im1;
            try
            {
                im1 = new Image<Bgr, byte>(images[idx++]); 
                if (im1 != null)
                {

                    im1 = im1.Resize(ImageFalsos.Height / ((double)im1.Height), Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    ImageFalsos.Width = im1.Width;
                    ImageFalsos.Source = CaptureWebCam.BitmapToImageSource(im1.ToBitmap());
                    EV.ScanImageWithCascadeClassifier(im1, CascadeFaceDetectorDic);
                    //EV.ShowListWithoutCombine("12", CanvasFalsos);
                    //EV.ShowListWithoutCombine("24", CanvasFalsos);
                    EV.ShowList(CascadeFaceDetectorDic.Keys.Last(), CanvasFalsos);

                }

            }
            catch { 
            }
         
     

        }

        #endregion 

        public void fddbdatagenerate()
        {

            
            for (int i = 1; i < 11; i++)
            {
                string[] names = FDDB.ReadFDDBFile(@"E:\Bases de datos\FDDB\FDDB-folds\FDDB-fold-" + i.ToString("00") + ".txt");
                fddb.EvaluateimagesCascadeClass(@"E:\Bases de datos\FDDB\", names, CascadeFaceDetectorDic, i);

            }
        }

        private void SiguienteCompleta_Click(object sender, RoutedEventArgs e)
        {
            if (images == null)
            {
                images = System.IO.Directory.GetFiles(@"E:\Bases de datos\FDDBppm\2002\07\26\big", "*.ppm");
                idx = 0;
            }


            if (idx >= images.Length) idx = 0;

            Image<Bgr, Byte> im1 = new Image<Bgr, byte>(images[idx++]); ;
            if (im1 != null)
            {
                ImageCompleta1.Width = im1.Width; ImageCompleta2.Width = im1.Width; ImageCompleta3.Width = im1.Width;
                ImageCompleta1.Height = im1.Height; ImageCompleta2.Height = im1.Height; ImageCompleta3.Height = im1.Height;
                ImageCompleta1.Source = CaptureWebCam.BitmapToImageSource(im1.ToBitmap());
                ImageCompleta2.Source = CaptureWebCam.BitmapToImageSource(im1.ToBitmap());
                ImageCompleta3.Source = CaptureWebCam.BitmapToImageSource(im1.ToBitmap());
                ImageCompleta2.Margin = new Thickness(ImageCompleta2.Width,0,0,0);
                ImageCompleta3.Margin = new Thickness(2*ImageCompleta2.Width, 0, 0, 0);
                CanvasCompleta2.Margin = new Thickness(ImageCompleta2.Width, 0, 0, 0);
                CanvasCompleta3.Margin = new Thickness(2*ImageCompleta2.Width, 0, 0, 0);

                EV.ScanImageWithCascadeClassifier(im1, CascadeFaceDetectorDic);

                //EV.ShowListWithoutCombine("12", CanvasCompleta1);
                //EV.ShowListWithoutCombine("24", CanvasCompleta2);
               // EV.ShowListWithoutCombine(CascadeFaceDetectorDic.Keys.Last(), CanvasCompleta3);

               EV.ShowList("12", CanvasCompleta1);
               EV.ShowList("24", CanvasCompleta2);
               EV.ShowList(CascadeFaceDetectorDic.Keys.Last(), CanvasCompleta3);
                

                //                EV.ShowList(CascadeFaceDetectorDic.Keys.Last(), CanvasArchivo);

            }

        }

    }
}
