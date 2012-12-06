/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace NNXNA
{
    public partial class NetworkTester : Form
    {

        

        #region Thread Safe Delegates and Functions for ITC

        //For setting the enable state of the form
        delegate void SetFormEnabledCallback(bool state);
        private void SetFormEnabled(bool state)
        {
            if (this.InvokeRequired)
            {
                SetFormEnabledCallback c = new SetFormEnabledCallback(SetFormEnabled);
                this.Invoke(c, new object[] { state });
            }
            else
            {
                this.Enabled = state;
            }
        }

        //For setting the Run Generations button text
        delegate int GetSelectedBreedingIndexCallback();
        private int GetSelectedBreedingIndex()
        {
            if (this.button_runGenerations.InvokeRequired)
            {
                GetSelectedBreedingIndexCallback c = new GetSelectedBreedingIndexCallback(GetSelectedBreedingIndex);
                return (int)this.Invoke(c, new object[] { });
            }
            else
            {
                return comboBox_BreedingType.SelectedIndex;
            }
        }

        //For setting the Run Generations button text
        delegate void SetRunGenerationsButtonTextCallback(string text);
        private void SetRunGenerationsButtonText(string text)
        {
            if (this.button_runGenerations.InvokeRequired)
            {
                SetRunGenerationsButtonTextCallback c = new SetRunGenerationsButtonTextCallback(SetRunGenerationsButtonText);
                this.Invoke(c, new object[] { text });
            }
            else
            {
                this.button_runGenerations.Text = text;
            }
        }

        //For adding an element to the chart
        delegate void AddChartElementsCallback(double average, double max);
        private void AddChartElements(double average, double max)
        {
            if (this.chart.InvokeRequired)
            {
                AddChartElementsCallback c = new AddChartElementsCallback(AddChartElements);
                this.Invoke(c, new object[] { average, max });
            }
            else
            {
                //If this is the first point
                if (chart.Series[0].Points.Count == 0)
                {
                    //Set the averages to the first point and set the points
                    chart.Series["Average Moving Average"].Points.AddY(average);
                    chart.Series["Average"].Points.AddY(average);
                    chart.Series["Max Moving Average"].Points.AddY(max);
                    chart.Series["Max"].Points.AddY(max);
                }
                //If there are fewer points than the window size
                else if (chart.Series[0].Points.Count < (int)numericUpDown_movingAverageWindowSize.Value)
                {
                    //Average with the number of points as the window size and set the points
                    chart.Series["Average Moving Average"].Points.AddY(Util.CalculateMovingAverage(chart.Series["Average Moving Average"].Points[chart.Series["Average Moving Average"].Points.Count - 1].YValues[0], average, chart.Series[0].Points.Count + 1));
                    chart.Series["Average"].Points.AddY(average);
                    chart.Series["Max Moving Average"].Points.AddY(Util.CalculateMovingAverage(chart.Series["Max Moving Average"].Points[chart.Series["Max Moving Average"].Points.Count - 1].YValues[0], max, chart.Series[0].Points.Count + 1));
                    chart.Series["Max"].Points.AddY(max);
                }
                //If there are more points than the window size
                else
                {
                    //Average with the window size and set the points
                    chart.Series["Average Moving Average"].Points.AddY(Util.CalculateMovingAverage(chart.Series["Average Moving Average"].Points[chart.Series["Average Moving Average"].Points.Count - 1].YValues[0], average, (int)numericUpDown_movingAverageWindowSize.Value));
                    chart.Series["Average"].Points.AddY(average);
                    chart.Series["Max Moving Average"].Points.AddY(Util.CalculateMovingAverage(chart.Series["Max Moving Average"].Points[chart.Series["Max Moving Average"].Points.Count - 1].YValues[0], max, (int)numericUpDown_movingAverageWindowSize.Value));
                    chart.Series["Max"].Points.AddY(max);
                }

                //chart_geneViewer.Series["genome0"].Points.Clear();
                //chart_geneViewer.Series["genome1"].Points.Clear();
                //chart_geneViewer.Series["genome2"].Points.Clear();

                //chart_geneViewer.Series["genome0"].Points.Add(pop.Population[0].Weights);
                //chart_geneViewer.Series["genome1"].Points.Add(pop.Population[1].Weights);
                //chart_geneViewer.Series["genome2"].Points.Add(pop.Population[2].Weights);

                chart.Update();
                //chart_geneViewer.Update();
            }
        }

        //For setting the iteration label text
        delegate void SetIterationLabelTextCallback(string text);
        private void SetIterationLabelText(string text)
        {
            if (this.label_iteration.InvokeRequired)
            {
                SetIterationLabelTextCallback c = new SetIterationLabelTextCallback(SetIterationLabelText);
                this.Invoke(c, new object[] { text });
            }
            else
            {
                label_iteration.Text = text;
            }
        }

        //For setting the total iteration label text
        delegate void SetTotalIterationLabelTextCallback(string text);
        private void SetTotalIterationLabelText(string text)
        {
            if (this.label_iteration.InvokeRequired)
            {
                SetTotalIterationLabelTextCallback c = new SetTotalIterationLabelTextCallback(SetTotalIterationLabelText);
                this.Invoke(c, new object[] { text });
            }
            else
            {
                label_totalIterations.Text = text;
            }
        }

        //For forcing an update of the form
        delegate void UpdateFormCallback();
        private void UpdateForm()
        {
            if (this.InvokeRequired)
            {
                UpdateFormCallback c = new UpdateFormCallback(UpdateForm);
                this.Invoke(c, new object[] { });
            }
            else
            {
                this.Update();
            }
        }

        //For forcing a refresh of the form
        delegate void RefreshFormCallback();
        private void RefreshForm()
        {
            if (this.InvokeRequired)
            {
                RefreshFormCallback c = new RefreshFormCallback(RefreshForm);
                this.Invoke(c, new object[] { });
            }
            else
            {
                this.Refresh();
            }
        }

        //For clearing the simple output rich text box
        delegate void ClearSimpleOutRichTextBoxBoxCallback();
        private void ClearSimpleOutRichTextBoxBox()
        {
            if (this.richTextBox_simpleOut.InvokeRequired)
            {
                ClearSimpleOutRichTextBoxBoxCallback c = new ClearSimpleOutRichTextBoxBoxCallback(ClearSimpleOutRichTextBoxBox);
                this.Invoke(c, new object[] { });
            }
            else
            {
                this.richTextBox_simpleOut.Clear();
            }
        }

        //For setting the text in the simple out rich text box
        delegate void SetSimpleOutRichTextBoxBoxTextCallback(string text);
        private void SetSimpleOutRichTextBoxBoxText(string text)
        {
            if (this.richTextBox_simpleOut.InvokeRequired)
            {
                SetSimpleOutRichTextBoxBoxTextCallback c = new SetSimpleOutRichTextBoxBoxTextCallback(SetSimpleOutRichTextBoxBoxText);
                this.Invoke(c, new object[] { text });
            }
            else
            {
                this.richTextBox_simpleOut.Text = text;
            }
        }

        #endregion

        #region Form Event Callbacks

        //This function executes a basic input/output tests on the neural network
        private void button_testRun_Click(object sender, EventArgs e)
        {
            //Set the word wrap to false so we can know what line represents what
            richTextBox_simpleOut.WordWrap = false;

            //Get the input tokens from the user
            string[] tokens = textBox_testInput.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            //If the user hasn't put in the correct number of inputs, show an error and exit the function
            if(tokens.Length!=testNN.NumberOfInputs)
            {
                MessageBox.Show("Error: Incorrect Number of Inputs. Found "+tokens.Length+", expected "+testNN.NumberOfInputs+". Are you separating inputs with spaces?");
                return;
            }

            //Build an input array
            double[] input = new double[tokens.Length];

            for (int i = 0; i < tokens.Length; i++)
            {
                try
                {
                    //Parse the token to a double
                    input[i] = double.Parse(tokens[i]);
                }
                catch (FormatException)
                {
                    //If the parse fails, show an error and exit the function
                    MessageBox.Show("Incorrect token found at item " + i + " (0 is the first item). Confirm that the number is a properly formatted double precision floating point value.");
                    return;
                }
            }

            //Compute an output based on the inputs
            double[] output = testNN.ComputeNNOutputs(input);

            //Create a string builder for quick output
            StringBuilder outputStringBuilder = new StringBuilder();

            //Fill it with the output data
            for (int i = 0; i < output.Length; i++)
                outputStringBuilder.AppendFormat("{0} ", output[i]);

            //Show the data in the text box
            textBox_testOutput.Text = outputStringBuilder.ToString();
        }

        

        //This function will run a test simulation in an attempt to see if the population object
        //runs as expected.
        private void button_runGenerations_Click(object sender, EventArgs e)
        {

            //Set the word wrap state to true so we can see the lists more easily
            richTextBox_simpleOut.WordWrap = true;

            //Fill the simThread with a default value (not running)
            simThread = new Thread(new ThreadStart(runGeneticSimulation));
            simThread.Start();
        }

        //This function is a callback for the reset button for the simulation
        private void button_reset_Click(object sender, EventArgs e)
        {
            //Clear the chart
            for (int i = 0; i < chart.Series.Count; i++)
                chart.Series[i].Points.Clear();

            //Clear the iteration text
            label_iteration.Text = "Iteration: ";

            //Build a test population
            pop = new NNPopulation(simulationPopulationSize, neuralNetworkStructure);
        }

        //This function activates when the Show Simulation? check box is checked or unchecked. 
        //It saves the previous value of the numGenerations control and disables it if it's being checked, and reverts it if unchecked.
        //This prevents users from accidentally running a ton of visible simulations and prevents me from having to build a mechnisim which 
        //aborts the simulation prematurely (or flags a series of simulations for abortion).
        private void checkBox_showSimulation_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                //The box was checked, save the current value
                this.numericUpDown_numGenerations.Tag = this.numericUpDown_numGenerations.Value;
                //Set the current value to 1
                this.numericUpDown_numGenerations.Value = 1;
                //Disable the control
                this.numericUpDown_numGenerations.Enabled = false;
            }
            else
            {
                //The box was unchecked, revert the value
                this.numericUpDown_numGenerations.Value = (decimal)this.numericUpDown_numGenerations.Tag;
                //Enable the control
                this.numericUpDown_numGenerations.Enabled = true;
            }
        }

        #endregion
    }
}
*/