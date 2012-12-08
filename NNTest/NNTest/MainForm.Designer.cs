namespace NNTest
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.richTextBox_simpleOut = new System.Windows.Forms.RichTextBox();
            this.textBox_testInput = new System.Windows.Forms.TextBox();
            this.label_testInput = new System.Windows.Forms.Label();
            this.button_testRun = new System.Windows.Forms.Button();
            this.textBox_testOutput = new System.Windows.Forms.TextBox();
            this.button_testSerialization = new System.Windows.Forms.Button();
            this.button_runGenerations = new System.Windows.Forms.Button();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.checkBox_showSimulation = new System.Windows.Forms.CheckBox();
            this.numericUpDown_numGenerations = new System.Windows.Forms.NumericUpDown();
            this.label_numGenerations = new System.Windows.Forms.Label();
            this.label_iteration = new System.Windows.Forms.Label();
            this.button_reset = new System.Windows.Forms.Button();
            this.numericUpDown_movingAverageWindowSize = new System.Windows.Forms.NumericUpDown();
            this.label_movingAverageWindowSize = new System.Windows.Forms.Label();
            this.label_totalIterations = new System.Windows.Forms.Label();
            this.comboBox_BreedingType = new System.Windows.Forms.ComboBox();
            this.checkBox_updateChart = new System.Windows.Forms.CheckBox();
            this.numericUpDown_foodCount = new System.Windows.Forms.NumericUpDown();
            this.label_foodCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_numGenerations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_movingAverageWindowSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_foodCount)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox_simpleOut
            // 
            this.richTextBox_simpleOut.Location = new System.Drawing.Point(12, 12);
            this.richTextBox_simpleOut.Name = "richTextBox_simpleOut";
            this.richTextBox_simpleOut.ReadOnly = true;
            this.richTextBox_simpleOut.Size = new System.Drawing.Size(138, 238);
            this.richTextBox_simpleOut.TabIndex = 0;
            this.richTextBox_simpleOut.Text = "";
            this.richTextBox_simpleOut.WordWrap = false;
            // 
            // textBox_testInput
            // 
            this.textBox_testInput.Location = new System.Drawing.Point(12, 271);
            this.textBox_testInput.Name = "textBox_testInput";
            this.textBox_testInput.Size = new System.Drawing.Size(138, 20);
            this.textBox_testInput.TabIndex = 2;
            // 
            // label_testInput
            // 
            this.label_testInput.AutoSize = true;
            this.label_testInput.Location = new System.Drawing.Point(9, 255);
            this.label_testInput.Name = "label_testInput";
            this.label_testInput.Size = new System.Drawing.Size(141, 13);
            this.label_testInput.TabIndex = 3;
            this.label_testInput.Text = "Test Input (Space Delimited)";
            // 
            // button_testRun
            // 
            this.button_testRun.Location = new System.Drawing.Point(12, 297);
            this.button_testRun.Name = "button_testRun";
            this.button_testRun.Size = new System.Drawing.Size(138, 25);
            this.button_testRun.TabIndex = 4;
            this.button_testRun.Text = "Run With Test Input";
            this.button_testRun.UseVisualStyleBackColor = true;
            this.button_testRun.Click += new System.EventHandler(this.button_testRun_Click);
            // 
            // textBox_testOutput
            // 
            this.textBox_testOutput.Location = new System.Drawing.Point(12, 328);
            this.textBox_testOutput.Name = "textBox_testOutput";
            this.textBox_testOutput.ReadOnly = true;
            this.textBox_testOutput.Size = new System.Drawing.Size(138, 20);
            this.textBox_testOutput.TabIndex = 5;
            // 
            // button_testSerialization
            // 
            this.button_testSerialization.Location = new System.Drawing.Point(12, 354);
            this.button_testSerialization.Name = "button_testSerialization";
            this.button_testSerialization.Size = new System.Drawing.Size(138, 25);
            this.button_testSerialization.TabIndex = 6;
            this.button_testSerialization.Text = "Test Serialization";
            this.button_testSerialization.UseVisualStyleBackColor = true;
            this.button_testSerialization.Click += new System.EventHandler(this.button_testSerialization_Click);
            // 
            // button_runGenerations
            // 
            this.button_runGenerations.Location = new System.Drawing.Point(240, 329);
            this.button_runGenerations.Name = "button_runGenerations";
            this.button_runGenerations.Size = new System.Drawing.Size(132, 50);
            this.button_runGenerations.TabIndex = 7;
            this.button_runGenerations.Text = "Run Generations";
            this.button_runGenerations.UseVisualStyleBackColor = true;
            this.button_runGenerations.Click += new System.EventHandler(this.button_runGenerations_Click);
            // 
            // chart
            // 
            this.chart.BackColor = System.Drawing.Color.Transparent;
            chartArea2.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea2);
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend2.Name = "Legend1";
            this.chart.Legends.Add(legend2);
            this.chart.Location = new System.Drawing.Point(156, 12);
            this.chart.Name = "chart";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Color = System.Drawing.Color.Red;
            series5.Legend = "Legend1";
            series5.Name = "Average";
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series6.Color = System.Drawing.Color.Green;
            series6.Legend = "Legend1";
            series6.Name = "Average Moving Average";
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Color = System.Drawing.Color.Orange;
            series7.Legend = "Legend1";
            series7.Name = "Max";
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series8.Color = System.Drawing.Color.Blue;
            series8.Legend = "Legend1";
            series8.Name = "Max Moving Average";
            this.chart.Series.Add(series5);
            this.chart.Series.Add(series6);
            this.chart.Series.Add(series7);
            this.chart.Series.Add(series8);
            this.chart.Size = new System.Drawing.Size(397, 256);
            this.chart.TabIndex = 8;
            this.chart.Text = "chart1";
            // 
            // checkBox_showSimulation
            // 
            this.checkBox_showSimulation.AutoSize = true;
            this.checkBox_showSimulation.Location = new System.Drawing.Point(160, 284);
            this.checkBox_showSimulation.Name = "checkBox_showSimulation";
            this.checkBox_showSimulation.Size = new System.Drawing.Size(110, 17);
            this.checkBox_showSimulation.TabIndex = 9;
            this.checkBox_showSimulation.Text = "Show Simulation?";
            this.checkBox_showSimulation.UseVisualStyleBackColor = true;
            this.checkBox_showSimulation.CheckedChanged += new System.EventHandler(this.checkBox_showSimulation_CheckedChanged);
            // 
            // numericUpDown_numGenerations
            // 
            this.numericUpDown_numGenerations.Location = new System.Drawing.Point(455, 322);
            this.numericUpDown_numGenerations.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numericUpDown_numGenerations.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_numGenerations.Name = "numericUpDown_numGenerations";
            this.numericUpDown_numGenerations.Size = new System.Drawing.Size(94, 20);
            this.numericUpDown_numGenerations.TabIndex = 10;
            this.numericUpDown_numGenerations.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_numGenerations.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label_numGenerations
            // 
            this.label_numGenerations.AutoSize = true;
            this.label_numGenerations.Location = new System.Drawing.Point(433, 306);
            this.label_numGenerations.Name = "label_numGenerations";
            this.label_numGenerations.Size = new System.Drawing.Size(116, 13);
            this.label_numGenerations.TabIndex = 11;
            this.label_numGenerations.Text = "Number of Generations";
            // 
            // label_iteration
            // 
            this.label_iteration.AutoSize = true;
            this.label_iteration.Location = new System.Drawing.Point(156, 267);
            this.label_iteration.Name = "label_iteration";
            this.label_iteration.Size = new System.Drawing.Size(51, 13);
            this.label_iteration.TabIndex = 12;
            this.label_iteration.Text = "Iteration: ";
            // 
            // button_reset
            // 
            this.button_reset.Location = new System.Drawing.Point(159, 345);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(75, 34);
            this.button_reset.TabIndex = 13;
            this.button_reset.Text = "Reset";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // numericUpDown_movingAverageWindowSize
            // 
            this.numericUpDown_movingAverageWindowSize.Location = new System.Drawing.Point(474, 283);
            this.numericUpDown_movingAverageWindowSize.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown_movingAverageWindowSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_movingAverageWindowSize.Name = "numericUpDown_movingAverageWindowSize";
            this.numericUpDown_movingAverageWindowSize.Size = new System.Drawing.Size(75, 20);
            this.numericUpDown_movingAverageWindowSize.TabIndex = 10;
            this.numericUpDown_movingAverageWindowSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_movingAverageWindowSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label_movingAverageWindowSize
            // 
            this.label_movingAverageWindowSize.AutoSize = true;
            this.label_movingAverageWindowSize.Location = new System.Drawing.Point(403, 267);
            this.label_movingAverageWindowSize.Name = "label_movingAverageWindowSize";
            this.label_movingAverageWindowSize.Size = new System.Drawing.Size(150, 13);
            this.label_movingAverageWindowSize.TabIndex = 11;
            this.label_movingAverageWindowSize.Text = "Moving Average Window Size";
            // 
            // label_totalIterations
            // 
            this.label_totalIterations.Location = new System.Drawing.Point(206, 6);
            this.label_totalIterations.Name = "label_totalIterations";
            this.label_totalIterations.Size = new System.Drawing.Size(329, 13);
            this.label_totalIterations.TabIndex = 14;
            this.label_totalIterations.Text = "0 Total Iterations";
            this.label_totalIterations.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBox_BreedingType
            // 
            this.comboBox_BreedingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_BreedingType.FormattingEnabled = true;
            this.comboBox_BreedingType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBox_BreedingType.Items.AddRange(new object[] {
            "Average",
            "Crossover",
            "Pick Each"});
            this.comboBox_BreedingType.Location = new System.Drawing.Point(158, 300);
            this.comboBox_BreedingType.Name = "comboBox_BreedingType";
            this.comboBox_BreedingType.Size = new System.Drawing.Size(112, 21);
            this.comboBox_BreedingType.TabIndex = 15;
            // 
            // checkBox_updateChart
            // 
            this.checkBox_updateChart.AutoSize = true;
            this.checkBox_updateChart.Checked = true;
            this.checkBox_updateChart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_updateChart.Location = new System.Drawing.Point(175, 2);
            this.checkBox_updateChart.Name = "checkBox_updateChart";
            this.checkBox_updateChart.Size = new System.Drawing.Size(95, 17);
            this.checkBox_updateChart.TabIndex = 16;
            this.checkBox_updateChart.Text = "Update Chart?";
            this.checkBox_updateChart.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_foodCount
            // 
            this.numericUpDown_foodCount.Location = new System.Drawing.Point(455, 359);
            this.numericUpDown_foodCount.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numericUpDown_foodCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_foodCount.Name = "numericUpDown_foodCount";
            this.numericUpDown_foodCount.Size = new System.Drawing.Size(94, 20);
            this.numericUpDown_foodCount.TabIndex = 10;
            this.numericUpDown_foodCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_foodCount.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // label_foodCount
            // 
            this.label_foodCount.AutoSize = true;
            this.label_foodCount.Location = new System.Drawing.Point(487, 343);
            this.label_foodCount.Name = "label_foodCount";
            this.label_foodCount.Size = new System.Drawing.Size(62, 13);
            this.label_foodCount.TabIndex = 11;
            this.label_foodCount.Text = "Food Count";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 386);
            this.Controls.Add(this.checkBox_updateChart);
            this.Controls.Add(this.comboBox_BreedingType);
            this.Controls.Add(this.label_totalIterations);
            this.Controls.Add(this.button_reset);
            this.Controls.Add(this.label_iteration);
            this.Controls.Add(this.label_movingAverageWindowSize);
            this.Controls.Add(this.label_foodCount);
            this.Controls.Add(this.label_numGenerations);
            this.Controls.Add(this.numericUpDown_foodCount);
            this.Controls.Add(this.numericUpDown_movingAverageWindowSize);
            this.Controls.Add(this.numericUpDown_numGenerations);
            this.Controls.Add(this.checkBox_showSimulation);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.button_runGenerations);
            this.Controls.Add(this.button_testSerialization);
            this.Controls.Add(this.textBox_testOutput);
            this.Controls.Add(this.button_testRun);
            this.Controls.Add(this.label_testInput);
            this.Controls.Add(this.textBox_testInput);
            this.Controls.Add(this.richTextBox_simpleOut);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "NNTest";
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_numGenerations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_movingAverageWindowSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_foodCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_simpleOut;
        private System.Windows.Forms.TextBox textBox_testInput;
        private System.Windows.Forms.Label label_testInput;
        private System.Windows.Forms.Button button_testRun;
        private System.Windows.Forms.TextBox textBox_testOutput;
        private System.Windows.Forms.Button button_testSerialization;
        private System.Windows.Forms.Button button_runGenerations;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.CheckBox checkBox_showSimulation;
        private System.Windows.Forms.NumericUpDown numericUpDown_numGenerations;
        private System.Windows.Forms.Label label_numGenerations;
        private System.Windows.Forms.Label label_iteration;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.NumericUpDown numericUpDown_movingAverageWindowSize;
        private System.Windows.Forms.Label label_movingAverageWindowSize;
        private System.Windows.Forms.Label label_totalIterations;
        private System.Windows.Forms.ComboBox comboBox_BreedingType;
        private System.Windows.Forms.CheckBox checkBox_updateChart;
        private System.Windows.Forms.NumericUpDown numericUpDown_foodCount;
        private System.Windows.Forms.Label label_foodCount;
    }
}

