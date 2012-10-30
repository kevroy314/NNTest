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
            this.richTextBox_simpleOut = new System.Windows.Forms.RichTextBox();
            this.button_buildTestNN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox_simpleOut
            // 
            this.richTextBox_simpleOut.Location = new System.Drawing.Point(12, 12);
            this.richTextBox_simpleOut.Name = "richTextBox_simpleOut";
            this.richTextBox_simpleOut.Size = new System.Drawing.Size(269, 238);
            this.richTextBox_simpleOut.TabIndex = 0;
            this.richTextBox_simpleOut.Text = "";
            // 
            // button_buildTestNN
            // 
            this.button_buildTestNN.Location = new System.Drawing.Point(103, 270);
            this.button_buildTestNN.Name = "button_buildTestNN";
            this.button_buildTestNN.Size = new System.Drawing.Size(88, 25);
            this.button_buildTestNN.TabIndex = 1;
            this.button_buildTestNN.Text = "Build Test NN";
            this.button_buildTestNN.UseVisualStyleBackColor = true;
            this.button_buildTestNN.Click += new System.EventHandler(this.button_buildTestNN_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 316);
            this.Controls.Add(this.button_buildTestNN);
            this.Controls.Add(this.richTextBox_simpleOut);
            this.Name = "MainForm";
            this.Text = "NNTest";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_simpleOut;
        private System.Windows.Forms.Button button_buildTestNN;
    }
}

