namespace TextToSpeech
{
    partial class fmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmMain));
            this.btStartStop = new System.Windows.Forms.Button();
            this.mmText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btStartStop
            // 
            this.btStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btStartStop.Location = new System.Drawing.Point(2, 212);
            this.btStartStop.Name = "btStartStop";
            this.btStartStop.Size = new System.Drawing.Size(454, 29);
            this.btStartStop.TabIndex = 0;
            this.btStartStop.Text = "Читати";
            this.btStartStop.UseVisualStyleBackColor = true;
            this.btStartStop.Click += new System.EventHandler(this.btStartStop_Click);
            // 
            // mmText
            // 
            this.mmText.AcceptsReturn = true;
            this.mmText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mmText.Location = new System.Drawing.Point(2, 2);
            this.mmText.Multiline = true;
            this.mmText.Name = "mmText";
            this.mmText.Size = new System.Drawing.Size(454, 210);
            this.mmText.TabIndex = 1;
            this.mmText.Text = "Ви знаєте, як липа шелестить\r\nу місячні весняні ночі?";
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 242);
            this.Controls.Add(this.mmText);
            this.Controls.Add(this.btStartStop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fmMain";
            this.Text = "Озвучування тексту";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fmMain_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btStartStop;
        private System.Windows.Forms.TextBox mmText;
    }
}

