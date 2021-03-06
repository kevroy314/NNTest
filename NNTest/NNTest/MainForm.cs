﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;

namespace NNTest
{
    /* This is the main testing form used for experimenting and displaying results.
     */ 

    public partial class MainForm : Form
    {
        #region Member Variables

        //An internal neural network which is created purely for testing without an application
        private NN testNN;

        //The neural network population for test
        private NNPopulation pop;

        //This thread will parallel code which shouldn't run in the GUI thread
        private Thread simThread;
        //This bool will be used to request the simThread be stopped
        private bool requestStop;

        private int numIterations;

        private List<double> avgScoreList;
        private List<double> movAvgScoreList;
        private List<double> maxScoreList;
        private List<double> movMaxScoreList;

        #endregion

        #region Constructors

        public MainForm()
        {
            InitializeComponent();

            //Initialize a test network
            testNN = new NN(4, 2, 1, new int[] { 6 });

            //Build a test population
            pop = new NNPopulation(Params.simulationPopulationSize, Params.neuralNetworkStructure);

            //Output the default object test results
            richTextBox_simpleOut.Text = getTestDataString();

            //Fill the simThread with a default value (not running)
            simThread = new Thread(new ThreadStart(runGeneticSimulation));
            //No stop is request at the beginning of execution
            requestStop = false;

            label_totalIterations.Text = "0 Total Iterations, Population Size: " + pop.StartPopulationSize;

            comboBox_BreedingType.SelectedIndex = 1;

            numIterations = 0;

            avgScoreList = new List<double>();
            movAvgScoreList = new List<double>();
            maxScoreList = new List<double>();
            movMaxScoreList = new List<double>();

            Series mAvgSeries = new Series("Average Moving Average");
            Series avgSeries = new Series("Average");
            Series mMaxSeries = new Series("Max Moving Average");
            Series maxSeries = new Series("Max");
            
            mAvgSeries.ChartType = SeriesChartType.Spline;
            avgSeries.ChartType = SeriesChartType.Line;
            mMaxSeries.ChartType = SeriesChartType.Spline;
            maxSeries.ChartType = SeriesChartType.Line;

            chart.Series.Add(mAvgSeries);
            chart.Series.Add(avgSeries);
            chart.Series.Add(mMaxSeries);
            chart.Series.Add(maxSeries);
        }

        #endregion

        #region Keyboard Event Callbacks

        //This will capture KeyDown events anywhere in the form
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //If the key is Escape
            if (keyData == Keys.Escape)
                //Request that the simulation loop thread stop
                requestStop = true;
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region Internal Test Functions

        //This is the default test for the neural network to confirm it is performing as expected
        public string getTestDataString()
        {
            StringBuilder outStringBuilder = new StringBuilder();
            outStringBuilder.AppendLine("Test Neural Network");
            outStringBuilder.AppendLine("-------------------");

            //Print the input data
            outStringBuilder.AppendLine("Input Data");
            outStringBuilder.AppendFormat("Number of Inputs: {0}\n", testNN.NumberOfInputs);
            outStringBuilder.AppendFormat("Number of Outputs: {0}\n", testNN.NumberOfOutputs);
            outStringBuilder.AppendFormat("Number of Hidden Layers: {0}\n", testNN.NumberOfHiddenLayers);
            outStringBuilder.AppendLine("Number of Nodes Per Hidden Layer: ");
            Array.ForEach(testNN.NumberOfNodesPerHiddenLayer, x => outStringBuilder.AppendFormat("{0} ", x));
            outStringBuilder.AppendLine();

            //Print the calculated data
            outStringBuilder.AppendLine("-------------------");
            outStringBuilder.AppendLine("General Calculated Metadata");
            outStringBuilder.AppendFormat("Total Number of Layers: {0}\n", testNN.NumberOfLayers);
            outStringBuilder.AppendFormat("Total Number of Nodes: {0}\n", testNN.NumberOfNodes);
            outStringBuilder.AppendFormat("Total Number of Hidden Nodes: {0}\n", testNN.NumberOfHiddenNodes);
            outStringBuilder.AppendLine();

            //Print the layer data
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

            //Print the detailed data
            outStringBuilder.AppendLine("-------------------");
            outStringBuilder.AppendLine("Detailed Data");
            outStringBuilder.AppendFormat("Number of Weights: {0}\n", testNN.Weights.Length);
            outStringBuilder.AppendLine("Weights: ");
            Array.ForEach(testNN.Weights, x => outStringBuilder.AppendFormat("{0}\n", x));
            outStringBuilder.AppendLine();

            //Return the results string
            return outStringBuilder.ToString();
        }

        private void runGeneticSimulation()
        {
            //Disable the form and change the button text
            SetFormEnabled(false);
            SetRunGenerationsButtonText("Click Escape to Abort");

            //Store the start time for benchmarking purposes
            DateTime before = DateTime.Now;

            //Create a string builder for output
            StringBuilder outputBuilder = new StringBuilder();

            //Iterate the simulation
            int j = 0;
            for (j = 0; j < numericUpDown_numGenerations.Value; j++)
            {
                numIterations++;

                //Run a single generation in the neural network ant simulation
                //pop.RunGeneration(typeof(NNAntSimulation), 
                //                  checkBox_showSimulation.Checked, 
                //                  (NNPopulation.BreedingFunction)GetSelectedBreedingIndex());

                pop.RunGeneration(typeof(NNAntSimulation),
                                  checkBox_showSimulation.Checked,
                                  (NNPopulation.BreedingFunction)GetSelectedBreedingIndex());

                //Get the latest fitness array from the simulation
                double[] latestFitness = pop.LatestFitness;

                //Find hte maximum fitness
                double maxFitness = -1;
                double sum = 0;
                for (int i = 0; i < latestFitness.Length; i++)
                {
                    if (latestFitness[i] > maxFitness) maxFitness = latestFitness[i];
                    sum += latestFitness[i];
                }

                //Update the chart
                AddChartElements(sum / latestFitness.Length, maxFitness);
                
                if (!checkBox_fastMode.Checked)
                {
                    if (checkBox_updateChart.Checked)
                        UpdateChart();

                    //Update the iteration counters
                    SetIterationLabelText("Iteration: " + (j + 1) + "/" + numericUpDown_numGenerations.Value);
                    SetTotalIterationLabelText(numIterations + " Total Iterations, Population Size: " + pop.Population.Count);

                    //Repaint the form
                    UpdateForm();

                    //Clear the output field
                    ClearSimpleOutRichTextBoxBox();

                    //Fill the output string builder with the fitness data from this generation
                    for (int i = 0; i < pop.LatestFitness.Length; i++)
                        outputBuilder.AppendFormat("{0} ", pop.LatestFitness[i]);

                    //Output the fitness data
                    SetSimpleOutRichTextBoxBoxText(outputBuilder.ToString());

                    //Reset the output builder
                    outputBuilder.Clear();
                }

                //If this function is running in the simulation thread and it was aborted
                if (requestStop)
                {
                    //Reset the stop request
                    requestStop = false;

                    //Break out of the loop
                    break;
                }
            }

            //Reenable the form and set the button text
            SetFormEnabled(true);
            SetRunGenerationsButtonText("Run Generations");

            //Store the end time of the simulation
            DateTime after = DateTime.Now;

            //Show a message box with the total seconds it took to run the complete program
            SetIterationLabelText("Iteration: " + j + "/" + numericUpDown_numGenerations.Value + ", Seconds: " + after.Subtract(before).TotalSeconds);

            //Finally update and refresh the form
            UpdateForm();
            RefreshForm();
            UpdateChart();
        }

        #endregion

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

        //Update the chart graphics (can be very slow with a large number of elements
        delegate void UpdateChartCallback();
        private void UpdateChart()
        {
            if (this.chart.InvokeRequired)
            {
                UpdateChartCallback c = new UpdateChartCallback(UpdateChart);
                this.Invoke(c, new object[] { });
            }
            else
            {
                chart.Series["Average Moving Average"].Points.DataBindY(movAvgScoreList);
                chart.Series["Average"].Points.DataBindY(avgScoreList);
                chart.Series["Max Moving Average"].Points.DataBindY(movMaxScoreList);
                chart.Series["Max"].Points.DataBindY(maxScoreList);
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
                if (avgScoreList.Count == 0)
                {
                    //Set the averages to the first point and set the points
                    movAvgScoreList.Add(average);
                    avgScoreList.Add(average);
                    movMaxScoreList.Add(max);
                    maxScoreList.Add(max);
                }
                //If there are fewer points than the window size
                else if (avgScoreList.Count < (int)numericUpDown_movingAverageWindowSize.Value)
                {
                    //Average with the number of points as the window size and set the points
                    movAvgScoreList.Add(Util.CalculateMovingAverage(movAvgScoreList[movAvgScoreList.Count - 1], average, movAvgScoreList.Count + 1));
                    avgScoreList.Add(average);
                    movMaxScoreList.Add(Util.CalculateMovingAverage(movMaxScoreList[movMaxScoreList.Count - 1], max, movMaxScoreList.Count + 1));
                    maxScoreList.Add(max);
                }
                //If there are more points than the window size
                else
                {
                    //Average with the window size and set the points
                    movAvgScoreList.Add(Util.CalculateMovingAverage(movAvgScoreList[movAvgScoreList.Count - 1], average, movAvgScoreList.Count + 1));
                    avgScoreList.Add(average);
                    movMaxScoreList.Add(Util.CalculateMovingAverage(movMaxScoreList[movMaxScoreList.Count - 1], max, (int)numericUpDown_movingAverageWindowSize.Value));
                    maxScoreList.Add(max);
                }
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
                MessageBox.Show("Error: Incorrect Number of Inputs. Found " + 
                                tokens.Length + 
                                ", expected " + 
                                testNN.NumberOfInputs + 
                                ". Are you separating inputs with spaces?");
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
                    MessageBox.Show("Incorrect token found at item " + 
                                    i + 
                                    " (0 is the first item). Confirm that the number is a properly formatted double precision floating point value.");
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

        //This function tests the serial functionality of the Neural Network object.
        //This can be useful if a particularly great neural network is consturcted and should be saved/loaded.
        private void button_testSerialization_Click(object sender, EventArgs e)
        {
            //Save the neural network created when this object was built
            testNN.SaveNNToFile("testNN.dat");
            //Load a new neural network from that file
            NN loadedNN = NN.LoadNNFromFile("testNN.dat");

            //Using the custom equals function, test to see if the networks are equal then show the results.
            if (loadedNN.Equals(testNN))
                MessageBox.Show("Serialization Test Succeeded. Saved Object Is Identical To Original.");
            else
                MessageBox.Show("Serialization Test Failed. Saved Object Is Not Identical To Original.");
        }

        //This function will run a test simulation in an attempt to see if the population object
        //runs as expected.
        private void button_runGenerations_Click(object sender, EventArgs e)
        {
            Params.foodCount = (int)numericUpDown_foodCount.Value;

            //Set the word wrap state to true so we can see the lists more easily
            richTextBox_simpleOut.WordWrap = true;

            //Fill the simThread with a default value (not running)
            simThread = new Thread(new ThreadStart(runGeneticSimulation));
            simThread.Start();
        }

        //This function is a callback for the reset button for the simulation
        private void button_reset_Click(object sender, EventArgs e)
        {
            numIterations = 0;

            //Clear the chart
            for (int i = 0; i < chart.Series.Count; i++)
                chart.Series[i].Points.Clear();

            avgScoreList.Clear();
            movAvgScoreList.Clear();
            maxScoreList.Clear();
            movMaxScoreList.Clear();

            //Clear the iteration text
            label_iteration.Text = "Iteration: ";

            //Build a test population
            pop = new NNPopulation(Params.simulationPopulationSize, Params.neuralNetworkStructure);
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
