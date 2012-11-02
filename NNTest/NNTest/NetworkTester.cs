using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace NNTest
{
    public partial class NetworkTester : Form
    {
        private NN testNN;

        public NetworkTester()
        {
            InitializeComponent();

            testNN = new NN(4, 2, 1, new int[] { 6 });

            richTextBox_simpleOut.Text = getTestDataString();
        }

        public string getTestDataString()
        {
            StringBuilder outStringBuilder = new StringBuilder();
            outStringBuilder.AppendLine("Test Neural Network");
            outStringBuilder.AppendLine("-------------------");

            outStringBuilder.AppendLine("Input Data");
            outStringBuilder.AppendFormat("Number of Inputs: {0}\n", testNN.NumberOfInputs);
            outStringBuilder.AppendFormat("Number of Outputs: {0}\n", testNN.NumberOfOutputs);
            outStringBuilder.AppendFormat("Number of Hidden Layers: {0}\n", testNN.NumberOfHiddenLayers);
            outStringBuilder.AppendLine("Number of Nodes Per Hidden Layer: ");
            Array.ForEach(testNN.NumberOfNodesPerHiddenLayer, x => outStringBuilder.AppendFormat("{0} ", x));
            outStringBuilder.AppendLine();

            outStringBuilder.AppendLine("-------------------");
            outStringBuilder.AppendLine("General Calculated Metadata");
            outStringBuilder.AppendFormat("Total Number of Layers: {0}\n", testNN.NumberOfLayers);
            outStringBuilder.AppendFormat("Total Number of Nodes: {0}\n", testNN.NumberOfNodes);
            outStringBuilder.AppendFormat("Total Number of Hidden Nodes: {0}\n", testNN.NumberOfHiddenNodes);
            outStringBuilder.AppendLine();

            outStringBuilder.AppendLine("-------------------");
            outStringBuilder.AppendLine("Specific Calculated Metadata Per Layer");
            outStringBuilder.AppendLine("Number of Weights Per Layer: ");
            Array.ForEach(testNN.WeightsPerLayer, x => outStringBuilder.AppendFormat("{0} ", x));
            outStringBuilder.AppendLine();
            outStringBuilder.AppendLine("Number of Inputs Per Layer: ");
            Array.ForEach(testNN.InputsPerLayer, x => outStringBuilder.AppendFormat("{0} ", x));
            outStringBuilder.AppendLine();
            outStringBuilder.AppendLine("Number of Nodes Per Layer: ");
            Array.ForEach(testNN.NodesPerLayer, x => outStringBuilder.AppendFormat("{0} ", x));
            outStringBuilder.AppendLine();
            outStringBuilder.AppendLine("Weight Start Index Per Layer: ");
            Array.ForEach(testNN.WeightStartIndexPerLayer, x => outStringBuilder.AppendFormat("{0} ", x));
            outStringBuilder.AppendLine();
            outStringBuilder.AppendLine("Weight Start Index Per Node Per Layer: ");
            for (int i = 0; i < testNN.WeightStartIndexPerLayerPerNode.Length; i++)
            {
                Array.ForEach(testNN.WeightStartIndexPerLayerPerNode[i], x => outStringBuilder.AppendFormat("{0} ", x));
                outStringBuilder.AppendLine();
            }
            outStringBuilder.AppendLine();
            outStringBuilder.AppendLine();

            outStringBuilder.AppendLine("-------------------");
            outStringBuilder.AppendLine("Detailed Data");
            outStringBuilder.AppendFormat("Number of Weights: {0}\n", testNN.Weights.Length);
            outStringBuilder.AppendLine("Weights: ");
            Array.ForEach(testNN.Weights, x => outStringBuilder.AppendFormat("{0}\n", x));
            outStringBuilder.AppendLine();

            return outStringBuilder.ToString();
        }

        private void button_testRun_Click(object sender, EventArgs e)
        {
            string[] tokens = textBox_testInput.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);


            if(tokens.Length!=testNN.NumberOfInputs)
            {
                MessageBox.Show("Error: Incorrect Number of Inputs. Found "+tokens.Length+", expected "+testNN.NumberOfInputs+". Are you separating inputs with spaces?");
                return;
            }

            double[] input = new double[tokens.Length];

            for (int i = 0; i < tokens.Length; i++)
            {
                try
                {
                    input[i] = double.Parse(tokens[i]);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Incorrect token found at item " + i + " (0 is the first item). Confirm that the number is a properly formatted double precision floating point value.");
                    return;
                }
            }

            double[] output = testNN.ComputeNNOutputs(input);

            StringBuilder outputStringBuilder = new StringBuilder();

            for (int i = 0; i < output.Length; i++)
                outputStringBuilder.AppendFormat("{0} ", output[i]);

            textBox_testOutput.Text = outputStringBuilder.ToString();
        }

        private void button_testSerialization_Click(object sender, EventArgs e)
        {
            testNN.SaveNNToFile("testNN.dat");
            NN loadedNN = NN.LoadNNFromFile("testNN.dat");
            if (loadedNN.Equals(testNN))
                MessageBox.Show("Serialization Test Succeeded. Saved Object Is Identical To Original.");
            else
                MessageBox.Show("Serialization Test Failed. Saved Object Is Not Identical To Original.");
        }

        private NNPopulation pop = new NNPopulation(100, new int[] { 4, 6, 2 });
        private void button_runGenerations_Click(object sender, EventArgs e)
        {
            DateTime before = DateTime.Now;
            for (int j = 0; j < 100; j++)
            {
                pop.RunGeneration();
                richTextBox_simpleOut.Clear();
                richTextBox_simpleOut.WordWrap = true;
                StringBuilder outputBuilder = new StringBuilder();
                for (int i = 0; i < pop.LatestFitness.Length; i++)
                    outputBuilder.AppendFormat("{0} ", pop.LatestFitness[i]);
                richTextBox_simpleOut.Text = outputBuilder.ToString();
            }
            DateTime after = DateTime.Now;
            MessageBox.Show("Seconds: " + after.Subtract(before).TotalSeconds);
        }
    }
}
