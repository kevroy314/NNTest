namespace NNTest
{
    partial class NetworkTester
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_numGenerations)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox_simpleOut
            // 
            this.richTextBox_simpleOut.Location = new System.Drawing.Point(12, 12);
            this.richTextBox_simpleOut.Name = "richTextBox_simpleOut";
            this.richTextBox_simpleOut.ReadOnly = true;
            this.richTextBox_simpleOut.Size = new System.Drawing.Size(269, 238);
            this.richTextBox_simpleOut.TabIndex = 0;
            this.richTextBox_simpleOut.Text = "";
            this.richTextBox_simpleOut.WordWrap = false;
            // 
            // textBox_testInput
            // 
            this.textBox_testInput.Location = new System.Drawing.Point(12, 271);
            this.textBox_testInput.Name = "textBox_testInput";
            this.textBox_testInput.Size = new System.Drawing.Size(269, 20);
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
            this.button_testRun.Size = new System.Drawing.Size(269, 25);
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
            this.textBox_testOutput.Size = new System.Drawing.Size(269, 20);
            this.textBox_testOutput.TabIndex = 5;
            // 
            // button_testSerialization
            // 
            this.button_testSerialization.Location = new System.Drawing.Point(12, 354);
            this.button_testSerialization.Name = "button_testSerialization";
            this.button_testSerialization.Size = new System.Drawing.Size(267, 25);
            this.button_testSerialization.TabIndex = 6;
            this.button_testSerialization.Text = "Test Serialization";
            this.button_testSerialization.UseVisualStyleBackColor = true;
            this.button_testSerialization.Click += new System.EventHandler(this.button_testSerialization_Click);
            // 
            // button_runGenerations
            // 
            this.button_runGenerations.Location = new System.Drawing.Point(374, 312);
            this.button_runGenerations.Name = "button_runGenerations";
            this.button_runGenerations.Size = new System.Drawing.Size(143, 50);
            this.button_runGenerations.TabIndex = 7;
            this.button_runGenerations.Text = "Run Generations";
            this.button_runGenerations.UseVisualStyleBackColor = true;
            this.button_runGenerations.Click += new System.EventHandler(this.button_runGenerations_Click);
            // 
            // chart
            // 
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(287, 12);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Average";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Max";
            this.chart.Series.Add(series1);
            this.chart.Series.Add(series2);
            this.chart.Size = new System.Drawing.Size(398, 256);
            this.chart.TabIndex = 8;
            this.chart.Text = "chart1";
            // 
            // checkBox_showSimulation
            // 
            this.checkBox_showSimulation.AutoSize = true;
            this.checkBox_showSimulation.Location = new System.Drawing.Point(539, 302);
            this.checkBox_showSimulation.Name = "checkBox_showSimulation";
            this.checkBox_showSimulation.Size = new System.Drawing.Size(110, 17);
            this.checkBox_showSimulation.TabIndex = 9;
            this.checkBox_showSimulation.Text = "Show Simulation?";
            this.checkBox_showSimulation.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_numGenerations
            // 
            this.numericUpDown_numGenerations.Location = new System.Drawing.Point(539, 354);
            this.numericUpDown_numGenerations.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown_numGenerations.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_numGenerations.Name = "numericUpDown_numGenerations";
            this.numericUpDown_numGenerations.Size = new System.Drawing.Size(95, 20);
            this.numericUpDown_numGenerations.TabIndex = 10;
            this.numericUpDown_numGenerations.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label_numGenerations
            // 
            this.label_numGenerations.AutoSize = true;
            this.label_numGenerations.Location = new System.Drawing.Point(536, 337);
            this.label_numGenerations.Name = "label_numGenerations";
            this.label_numGenerations.Size = new System.Drawing.Size(116, 13);
            this.label_numGenerations.TabIndex = 11;
            this.label_numGenerations.Text = "Number of Generations";
            // 
            // NetworkTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 386);
            this.Controls.Add(this.label_numGenerations);
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
            this.Name = "NetworkTester";
            this.Text = "NNTest";
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_numGenerations)).EndInit();
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
    }
}

