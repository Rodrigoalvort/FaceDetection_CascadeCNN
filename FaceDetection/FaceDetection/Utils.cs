using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceDetection
{
    class Utils
    {
        /// <summary>
        /// Escribir arreglo float en en txt
        /// </summary>
        /// <param name="a"> arreglo</param>
        public static  void  printFloatArray(float [] a)
        {
                string []  b= new string [a.Length];
                for (int i = 0; i < a.Length; i++)
                {
                    b[i] = a[i].ToString();

                }
                System.IO.File.WriteAllLines(@"data.txt", b);

        }

        /// <summary>
        /// Escribir arreglo de 2 dimensiones en un txt
        /// </summary>
        /// <param name="a"> arreglo 2D</param>
        public static void printFloatArray(float[,] a)
        {
            string[] b = new string[a.GetLongLength(0)];

            for (int j = 0; j < a.GetLength(0);j++)
            {
                b[j]="";
                for (int i = 0; i < a.GetLength(1); i++)
                {
                    b[j] = b[j] + a[j, i].ToString()+"\t";

                }
            }
            System.IO.File.WriteAllLines(@"data2d.txt", b);

        }

        /// <summary>
        /// Escribir Lista de Output And Target en un txt
        /// </summary>
        /// <param name="a"></param>
        public static void printOutputAndTargetList(List<NetEvaluation.OutputAndTarget> a )
        {
            string[] b = new string[a.Count];

            for (int i = 0; i < a.Count; i++)
            {

                b[i] = a[i].Target.ToString();
                for (int j = 0; j < a[i].Output.Length; j++)
                {
                    b[i] += "\t" + a[i].Output[j].ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US"));
 
                }
        
 
            }
            System.IO.File.WriteAllLines(@"Outputandtarget1.txt", b);


        }

        /// <summary>
        ///  lee información de los Folds de FDDB
        /// </summary>
        /// <param name="txtpath"> ruta del fold de fddb</param>
        /// <returns></returns>
        public static List<ImageData> ReadFDDBFile(string txtpath)
        {
            List<ImageData> imageData = new List<ImageData>();
            string line;
            string[] values;
            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(txtpath);
            while ((line = file.ReadLine()) != null)
            {
                string File;
                int Faces;
                List<float[]> list = new List<float[]>();

                File = line;
                Faces = int.Parse(file.ReadLine());
                for (int i = 0; i < Faces; i++)
                {
                    values = file.ReadLine().Split(new string[] { "\t", "\n", " " }, StringSplitOptions.None);
                    float[] FloatArray = new float[5];

                    for (int j = 0; j < 5; j++)
                    {
                        FloatArray[j] = float.Parse(values[j], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                    }
                    list.Add(FloatArray);

                }

                imageData.Add(new ImageData(File, Faces, list));
            }
            return imageData;
        }

        /// <summary>
        ///  estructura para información de imagenes FDDB
        /// </summary>
        public struct ImageData
        {
            public string file;
            public int faces;
            public List<float[]> array;
            public ImageData(string File, int Faces, List<float[]> Array)
            {
                file = File;
                faces = Faces;
                array = Array;

            }

        }

        /// <summary>
        /// recibe un Lista de InputAndTarget y escribe un archivo en formato UciFastReader
        /// </summary>
        /// <param name="path"> ruta del archivo destino</param>
        /// <param name="data"> lista de InputAndTarget</param>
        public static void writeUciFastReaderData(string path, List<NetEvaluation.InputAndTarget> data)
        {
            string[] array = new string[data.Count];
            for (int i = 0; i < data.Count; i++)
            {
                array[i] = data[i].Target.ToString();
                Console.WriteLine(i);
                for (int j = 0; j < data[i].Input.Length; j++)
                {
                    array[i] += "\t" + data[i].Input[j].ToString("000");
                }


            }
            System.IO.File.WriteAllLines(path, array);

        }

        /// <summary>
        /// Escribir Labelmap para CNTK
        /// </summary>
        /// <param name="inicio"> Label Inicial</param>
        /// <param name="fin">Label Final</param>
        /// <param name="path"> Ruta de destino</param>
        public static void WriteLabelMapCNTK(int inicio, int fin, string path)
        {
            List<string> list= new List<string>();
            for (int i = inicio; i <= fin; i++)
            {
                list.Add(i.ToString());
            }
            System.IO.File.WriteAllLines(path,list);
        }

        //public static void ShowTextInCanvas(double x, double y, string text, System.Windows.Media.Color color ,System.Windows.Controls.Canvas canvasObj )
        //{

        //    System.Windows.Controls.TextBlock textBlock = new System.Windows.Controls.TextBlock();
        //    textBlock.Text = text;
        //    textBlock.FontSize = 20;
        //    textBlock.Foreground = new System.Windows.Media.SolidColorBrush(color);
        //    System.Windows.Controls.Canvas.SetLeft(textBlock, x);
        //    System.Windows.Controls.Canvas.SetTop(textBlock, y);
        //    canvasObj.Children.Add(textBlock);
        //}

        /// <summary>
        ///  Lee información de un archivo UciFastReader y la almacena en un listado
        /// </summary>
        /// <param name="features">Numero de caracteristicas</param>
        /// <param name="outputs"> Numero de salidas salidas</param>
        /// <param name="path"> Ruta de origen del archivo</param>
        /// <returns></returns>
        public static List<UciFastReaderData> readCNTK(int features, int outputs, string path)
        {
            List<UciFastReaderData> values = new List<UciFastReaderData>();
            string line;

            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                var sprint = line.Split(new string[] { "\n", "\t" }, StringSplitOptions.None);

                if (sprint.Length >= features + 1)
                {
                    float label = float.Parse(sprint[0]);
                    float[] array = new float[features];
                    for (int i = 1; i < features + 1; i++)
                    {
                        array[i - 1] = float.Parse(sprint[i]);
                    }

                    values.Add(new UciFastReaderData(array, label));
                }
            }

            file.Close();
            return values;
        }

        /// <summary>
        /// Estructura para la lectura de archivos UciFastReader
        /// </summary>
        public struct UciFastReaderData
        {
            float[] Inputs;
            float Label;
            public UciFastReaderData(float[] inputs, float label)
            {
                Inputs = inputs;
                Label = label;
            }
        }


        public static string OpenFileDialog(string path,string title)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Title = title;
            openFileDialog1.InitialDirectory = path;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }
            else
            {
                return null;
        
            }
        }


        public static string SaveFileDialog(string path)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog1.InitialDirectory = path;
            saveFileDialog1.RestoreDirectory = true;
            
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return saveFileDialog1.FileName;
            }
            else
            {
                return null;
            }
        }


        public static string selectFolder(string path,string name)
        {

            System.Windows.Forms.SaveFileDialog sf = new System.Windows.Forms.SaveFileDialog();
            // Feed the dummy name to the save dialog
            sf.FileName = name;
            sf.InitialDirectory = path;
            if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Now here's our save folder
                return System.IO.Path.GetDirectoryName(sf.FileName);
                // Do whatever
            }
            else
            {
                return null;
            }
        }


    }
}
