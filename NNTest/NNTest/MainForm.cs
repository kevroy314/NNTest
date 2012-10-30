using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NNTest
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button_buildTestNN_Click(object sender, EventArgs e)
        {
            NN testNN = new NN(4, 2, 3, new int[] { 6, 8, 13 });
            StringBuilder outStringBuilder = new StringBuilder();
            outStringBuilder.AppendLine("Test Neural Network");
            outStringBuilder.AppendFormat("Number of Inputs: {0}\n", testNN.NumberOfInputs);
            outStringBuilder.AppendFormat("Number of Outputs: {0}\n", testNN.NumberOfOutputs);
            outStringBuilder.AppendFormat("Number of Hidden Layers: {0}\n", testNN.NumberOfHiddenLayers);
            outStringBuilder.AppendLine("Number of Nodes Per Hidden Layer: ");
            Array.ForEach(testNN.NumberOfNodesPerHiddenLayer, x => outStringBuilder.AppendFormat("{0} ", x));
            outStringBuilder.AppendLine();
            outStringBuilder.AppendLine();

            outStringBuilder.AppendFormat("Total Number of Layers: {0}\n", testNN.NumberOfLayers);

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

            outStringBuilder.AppendLine("Weights: ");
            Array.ForEach(testNN.Weights, x => outStringBuilder.AppendFormat("{0} ", x));
            outStringBuilder.AppendLine();

            richTextBox_simpleOut.Text = outStringBuilder.ToString();
        }
    }
}
