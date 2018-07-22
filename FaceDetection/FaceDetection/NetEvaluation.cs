using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.MSR.CNTK.Extensibility.Managed;

namespace FaceDetection
{
    class NetEvaluation
    {
        public    Dictionary<string, List<float>> inputs;

        public    Dictionary<string, int> inDims;

        public    Dictionary<string, int> outDims;

        IEvaluateModelManagedF Model;  // modelo cntk

        String outputLayerName;

        public  NetEvaluation(string modelpath)
        {
            Model = new IEvaluateModelManagedF();
            Model.CreateNetwork(string.Format("modelPath=\"{0}\"", modelpath), deviceId: -1);
            inDims = Model.GetNodeDimensions(NodeGroup.Input);
            outDims = Model.GetNodeDimensions(NodeGroup.Output);
            inputs=new Dictionary<string,List<float>>();
            
        }

        public NetEvaluation(string modelpath,List<string> OutputNodesLists)
        {
            Model = new IEvaluateModelManagedF();
            Model.CreateNetwork(string.Format("modelPath=\"{0}\"", modelpath), deviceId: -1,outputNodeNames: OutputNodesLists);
            inDims = Model.GetNodeDimensions(NodeGroup.Input);
            outDims = Model.GetNodeDimensions(NodeGroup.Output);
            inputs = new Dictionary<string, List<float>>();

        }
   
        public List<float> simulate(float[] a)
        {
            return simulate(a, outDims.First().Key);
        }
        public List<float> simulate(float[] a, string key)
        {

            //  Utils.printFloatArray(a);
            //            inputs.Add( outDims.First().Key,a.ToList());
            inputs[inDims.First().Key] = a.ToList();
            return Model.Evaluate(inputs, key);
        }
   
        
        public float[] softmax(List<float> a)
        {
            float[] b = new float[a.Count];
            float sum = 0;
            for (int i = 0; i < a.Count; i++)
            {
                b[i] = (float)Math.Exp((double)(a[i]));
                sum += b[i];

            }

            for (int i = 0; i < a.Count; i++)
            {
                b[i] = b[i] / sum;

            }
            return b;
        }

        public float[] simulateWithSoftmax(float[] a)
        {

            return softmax(simulate(a, outDims.First().Key));
        }

        public float[] simulateWithSoftmax(float[] a, string OutputKey)
        {

            return softmax(simulate(a,OutputKey)); 
        }

        public List<OutputAndTarget> ReadUciFastReaderAndEvaluate(string txtpath)
        {
            return ReadUciFastReaderAndEvaluate(txtpath, outDims.First().Key);
          }

        public List<OutputAndTarget> ReadUciFastReaderAndEvaluate(string txtpath, string outputKey)
        {
            string[] values;
            string line;
            float[] a = new float[inDims.First().Value];
            
            List<OutputAndTarget> outandtarget = new List<OutputAndTarget>();
            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(txtpath);
            while ((line = file.ReadLine()) != null)
            {
                values = line.Split(new string[] { "\t", "\n" }, StringSplitOptions.None);

                for (int i = 1; i < inDims.First().Value + 1; i++)
                {
                    a[i - 1] = float.Parse(values[i]);
                
                }

                outandtarget.Add(new OutputAndTarget(simulateWithSoftmax(a,outputKey),float.Parse(values[0])));
                Console.WriteLine(outandtarget.Count);
            }

            return outandtarget;
        }

        public List<OutputAndTarget> ReadUciFastReaderAndGetLayer(string txtpath, string outputKey)
        {
            string[] values;
            string line;
            float[] a = new float[inDims.First().Value];

            List<OutputAndTarget> outandtarget = new List<OutputAndTarget>();
            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(txtpath);
            while ((line = file.ReadLine()) != null)
            {
                values = line.Split(new string[] { "\t", "\n" }, StringSplitOptions.None);

                for (int i = 1; i < inDims.First().Value + 1; i++)
                {
                    a[i - 1] = float.Parse(values[i]);

                }

                outandtarget.Add(new OutputAndTarget(simulate(a, outputKey).ToArray(), float.Parse(values[0])));
                Console.WriteLine(outandtarget.Count);
            }

            return outandtarget;
        }

        public List<OutputAndTarget> EvaluateInputandTargetListAndGetLayer(List<InputAndTarget> list, string outputKey)
        {
            List<OutputAndTarget> outandtarget = new List<OutputAndTarget>();

                foreach (InputAndTarget iandt in list)
                {
                outandtarget.Add(new OutputAndTarget(simulate(iandt.Input, outputKey).ToArray(),iandt.Target));
                Console.WriteLine(outandtarget.Count);

                }
            

            return outandtarget;
        }
     
   
        public struct OutputAndTarget
        {
           public float [] Output;
           public float Target;
            public OutputAndTarget(float[] output, float target)
            {
                Output = output;
                Target = target;
            }

        }


        public struct InputAndTarget
        {
            public float[] Input;
            public float Target;
            public InputAndTarget(float[] input, float target)
            {
                Input = input;
                Target = target;
            }

        }

    }
}
