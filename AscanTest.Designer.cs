namespace ZYCControl
{
    partial class AscanTest
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
            this.ultraAscan1 = new ZYCControl.UltraAscan();
            this.SuspendLayout();
            // 
            // ultraAscan1
            // 
            this.ultraAscan1.Location = new System.Drawing.Point(1, 1);
            this.ultraAscan1.Name = "ultraAscan1";
            this.ultraAscan1.Size = new System.Drawing.Size(475, 379);
            this.ultraAscan1.TabIndex = 0;
            // 
            // AscanTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 385);
            this.Controls.Add(this.ultraAscan1);
            this.Name = "AscanTest";
            this.Text = "AscanTest";
            this.ResumeLayout(false);

        }

        #endregion

        private UltraAscan ultraAscan1;
    }
}