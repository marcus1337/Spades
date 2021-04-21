using System;
using System.Collections.Generic;
using System.Text;

namespace Spades
{
    [Serializable]
    public class NeuralNet
    {
        private float[] hiddenNodes, outputNodes;
        private float[] edgesAToB, edgesBToC;

        public void setHiddenLayer(float[] nodes)
        {
            this.hiddenNodes = (float[])nodes.Clone();
        }

        public void setOutputLayer(float[] nodes)
        {
            this.outputNodes = (float[])nodes.Clone();
        }

        public void setEdgesToLayer1(float[] nodes)
        {
            this.edgesAToB = (float[])nodes.Clone();
        }

        public void setEdgesToLayer2(float[] nodes)
        {
            this.edgesBToC = (float[])nodes.Clone();
        }

        public NeuralNet copy()
        {
            NeuralNet nn = new NeuralNet();
            nn.setHiddenLayer(hiddenNodes);
            nn.setOutputLayer(outputNodes);
            nn.setEdgesToLayer1(edgesAToB);
            nn.setEdgesToLayer2(edgesBToC);
            return nn;
        }

        public NeuralNet()
        {
            reset();
            mutateEdges();
        }

        private void reset()
        {
            hiddenNodes = new float[4];
            outputNodes = new float[2];
            edgesAToB = new float[16];
            edgesBToC = new float[8];
            for (int i = 0; i < edgesAToB.Length; i++)
                edgesAToB[i] = 0.5f;
            for (int i = 0; i < edgesBToC.Length; i++)
                edgesBToC[i] = 0.5f;
        }

        public void mutateEdges()
        {
            int numEdgesToMutate = Utils.Instance.rnd.Next(1, 6);
            for (int i = 0; i < numEdgesToMutate; i++)
            {
                if (Utils.Instance.rnd.Next(0, 3) == 0)
                {
                    int linkIndex = Utils.Instance.rnd.Next(0, 8);
                    edgesBToC[linkIndex] += (float)(Utils.Instance.rnd.NextDouble() - 0.5) / 5.0f;
                    edgesBToC[linkIndex] = Math.Clamp(edgesBToC[linkIndex], -1.0f, 1.0f);
                }
                else
                {
                    int linkIndex = Utils.Instance.rnd.Next(0, 16);
                    edgesAToB[linkIndex] += (float)(Utils.Instance.rnd.NextDouble() - 0.5) / 5.0f;
                    edgesAToB[linkIndex] = Math.Clamp(edgesAToB[linkIndex], -1.0f, 1.0f);
                }
            }
        }

        public int propagateNetworkAndGetAction(float[] inputNodes)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    hiddenNodes[j] += inputNodes[i] * edgesAToB[i * 4 + j];
                }
            }
            for (int i = 0; i < 4; i++)
            {
                hiddenNodes[i] = Math.Max(0, hiddenNodes[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    outputNodes[j] += hiddenNodes[i] * edgesBToC[i * 2 + j];
                }
            }
            int maxIndex = 0;
            float maxOutputValue = outputNodes[0];
            for (int i = 1; i < 2; i++)
            {
                if (outputNodes[i] > maxOutputValue)
                {
                    maxIndex = i;
                    maxOutputValue = outputNodes[i];
                }
            }

            return maxIndex;
        }

    }
}
