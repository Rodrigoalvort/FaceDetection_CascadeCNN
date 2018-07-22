using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
namespace FaceDetection
{
    class FDDB
    {

        FaceDetector FD;

        Dictionary <string,List<result>> imagesevaluated;
        List<string> keys;
        string ResultFolder;
        int foldnumber = 0;
        public FDDB(FaceDetector fd)
        {
            FD = fd;
            imagesevaluated= new Dictionary<string,List<result>>();
            keys = new List<string>();
            ResultFolder = @"C:\Users\Ideapad 300\Google Drive\Temporales\";
        }
       

        public static string[] ReadFDDBFile(string txtpath)
        {
            
            string line;
            List<string> names= new List<string>();
            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(txtpath);
            while ((line = file.ReadLine()) != null)
            {
                names.Add(line);

            }

            return names.ToArray();
        }

        public void Evaluateimages(string path,string [] names,string key,float thd,int fn)
        {
            imagesevaluated.Clear();
            keys.Clear();
            foldnumber = fn;
            
            for (int i = 0; i < names.Length; i++)
            {
                Image<Bgr, Byte> image = new Image<Bgr, byte>(path + @names[i]+".jpg");
                FD.ScanImageSingleClassifier(image, thd, key);
                List<result> resultList= new List<result>(); 

                for (int j=0;j<FD.BoundingBox[key].list.Count;j++)
                {
                    resultList.Add(new result(FD.BoundingBox[key].list[j].x,
                                              FD.BoundingBox[key].list[j].y,
                                              FD.BoundingBox[key].list[j].scale,
                                              FD.BoundingBox[key].list[j].score,
                                              FD.BoundingBox[key].list[j].weight));

                }
                keys.Add(names[i]);
                imagesevaluated.Add(names[i],resultList);
            }
            printimagesevaluated(key, thd);
        }

        public void EvaluateimagesCascadeClass(string path, string[] names, Dictionary<string,float> dic, int fn)
        {
            imagesevaluated.Clear();
            keys.Clear();
            foldnumber = fn;

            for (int i = 0; i < names.Length; i++)
            {
                Image<Bgr, Byte> image = new Image<Bgr, byte>(path + @names[i] + ".jpg");
                FD.ScanImageWithCascadeClassifier(image,dic);
                List<result> resultList = new List<result>();

                for (int j = 0; j < FD.BoundingBox[dic.Keys.Last()].list.Count; j++)
                {
                    resultList.Add(new result(FD.BoundingBox[dic.Keys.Last()].list[j].x,
                                              FD.BoundingBox[dic.Keys.Last()].list[j].y,
                                              FD.BoundingBox[dic.Keys.Last()].list[j].scale,
                                              FD.BoundingBox[dic.Keys.Last()].list[j].score,
                                              FD.BoundingBox[dic.Keys.Last()].list[j].weight));

                }
                keys.Add(names[i]);
                imagesevaluated.Add(names[i], resultList);
            }

            string keysNames="";
            string TresholdValues = "";
            for (int i = 0; i < dic.Count; i++)
            {
                keysNames       += dic.Keys.ToArray()[i]+"_";
                TresholdValues  += dic.Values.ToArray()[i]+"_";
     
            }
                printimagesevaluated("Cas" + keysNames, TresholdValues);
        }


        private void printimagesevaluated(string key, float treshold)
        {
            List<string> txt= new List<string>();
            for (int i = 0; i < keys.Count; i++)
            {
                txt.Add(keys[i]);
                txt.Add(imagesevaluated[keys[i]].Count.ToString());
                for (int j = 0; j < imagesevaluated[keys[i]].Count;j++ )
                {
                    float scorenew = imagesevaluated[keys[i]][j].score * imagesevaluated[keys[i]][j].weight;
                    txt.Add(((float)imagesevaluated[keys[i]][j].x).ToString(      "0.0000", System.Globalization.CultureInfo.InvariantCulture) + " " +
                              ((float)imagesevaluated[keys[i]][j].y).ToString(    "0.0000", System.Globalization.CultureInfo.InvariantCulture) + " " +
                              ((float)imagesevaluated[keys[i]][j].scale).ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture) + " " +
                              ((float)imagesevaluated[keys[i]][j].scale).ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture) + " " +
                               scorenew.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            string pathfolder = ResultFolder + @"\" + key + "-" + treshold.ToString();
            System.IO.Directory.CreateDirectory(pathfolder);
            System.IO.File.WriteAllLines(pathfolder + @"\" + "fold-" + foldnumber.ToString("00") + "-out.txt", txt.ToArray());
    


        }

        private void printimagesevaluated(string key, string treshold)
        {
            List<string> txt = new List<string>();
            for (int i = 0; i < keys.Count; i++)
            {
                txt.Add(keys[i]);
                txt.Add(imagesevaluated[keys[i]].Count.ToString());
                for (int j = 0; j < imagesevaluated[keys[i]].Count; j++)
                {
                    float scorenew = imagesevaluated[keys[i]][j].score * imagesevaluated[keys[i]][j].weight;
                    txt.Add(((float)imagesevaluated[keys[i]][j].x).ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture) + " " +
                              ((float)imagesevaluated[keys[i]][j].y).ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture) + " " +
                              ((float)imagesevaluated[keys[i]][j].scale).ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture) + " " +
                              ((float)imagesevaluated[keys[i]][j].scale).ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture) + " " +
                               scorenew.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            string pathfolder = ResultFolder + @"\" + key + "-" + treshold;
            System.IO.Directory.CreateDirectory(pathfolder);
            System.IO.File.WriteAllLines(pathfolder + @"\" + "fold-" + foldnumber.ToString("00") + "-out.txt", txt.ToArray());



        }

        struct result
        {
           public float score;
           public  float x;
           public float y;
           public float scale;
           public float weight;
           public result(float X, float Y, float Scale, float Score,float Weight)
           {
               x = X;
               y = Y;
               scale = Scale;
               score = Score;
               weight = Weight;
           }
        }

    }
}
