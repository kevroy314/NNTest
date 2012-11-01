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
            this.richTextBox_simpleOut = new System.Windows.Forms.RichTextBox();
            this.textBox_testInput = new System.Windows.Forms.TextBox();
            this.label_testInput = new System.Windows.Forms.Label();
            this.button_testRun = new System.Windows.Forms.Button();
            this.textBox_testOutput = new System.Windows.Forms.TextBox();
            this.button_testSerialization = new System.Windows.Forms.Button();
            this.button_runGenerations = new System.Windows.Forms.Button();
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
            this.button_runGenerations.Location = new System.Drawing.Point(325, 39);
            this.button_runGenerations.Name = "button_runGenerations";
            this.button_runGenerations.Size = new System.Drawing.Size(143, 50);
            this.button_runGenerations.TabIndex = 7;
            this.button_runGenerations.Text = "Run Generations";
            this.button_runGenerations.UseVisualStyleBackColor = true;
            this.button_runGenerations.Click += new System.EventHandler(this.button_runGenerations_Click);
            // 
            // NetworkTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 386);
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
    }
}

