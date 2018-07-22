using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaceDetection;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Data.Matlab;
namespace EvaluateBNFaceRecognition
{
    class UtilsMathnet
    {

      /// <summary>
      /// Escribe la salida del clasificador como una matriz 
      /// </summary>
      /// <param name="list"> lista de objetos OutputAndTarget o salidas del clasificador cntk</param>
      /// <param name="path"> direccion de destino del archivo .mat a crear</param>
 
        public static void saveDataMatlabMatrix(List<NetEvaluation.OutputAndTarget> list, string path)
        {
            Matrix<float> target = Matrix<float>.Build.Dense(list.Count,1);
            //rows  igual a el numero de registros
            //colums igual al numero de valores en el registro
            Matrix<float> salida = Matrix<float>.Build.Dense(list.Count, list[0].Output.Length);
            for (int i = 0; i < list.Count; i++)
            {
                target.At(i,0,list[i].Target);
                salida.SetRow(i, list[i].Output);             
            }
            var dict = new Dictionary<string, Matrix<float>>();
            dict.Add("target", target);
            dict.Add("output", salida);
            MatlabWriter.Write(path, dict);
            
        }
        /// <summary>
        /// Lee una matriz con  una matriz de entrada  de filas como  numero de muestras y columnas como cararateristicas  
        /// </summary>
        /// <param name="path">Ruta al archivo .mat</param>
        /// <param name="InputKey"> nombre de la matriz de caracteristicas en matlab</param>
        /// <param name="TargetKey">nombre de la matriz (n x 1) de targets en Matlab</param>
        /// <returns>entrega una lista con los registros de eentradas y objetivo</returns>
        public static List<NetEvaluation.InputAndTarget> ReadDataMatlabMatrix(string path, string InputKey, string TargetKey)
        {

            Dictionary<string, Matrix<float>> ms =    MatlabReader.ReadAll<float>(path);
            List<NetEvaluation.InputAndTarget> list = new List<NetEvaluation.InputAndTarget>();
            for (int i=0;i<ms.Values.First().RowCount; i++)
            {
            list.Add(new NetEvaluation.InputAndTarget(ms[InputKey].Row(i).ToArray(), ms[TargetKey].At(i,0)));
            }
            return list;
      
        }
    
    }
}
