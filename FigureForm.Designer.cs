namespace ZYCControl
{
    partial class FigureForm
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
            this.figure1 = new ZYCControl.FigurCommentUT();
            this.SuspendLayout();
            // 
            // figure1
            // 
            this.figure1.BackColor = System.Drawing.SystemColors.Control;
            this.figure1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.figure1.Location = new System.Drawing.Point(0, 0);
            this.figure1.Name = "figure1";
            this.figure1.Size = new System.Drawing.Size(176, 169);
            this.figure1.TabIndex = 0;
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(176, 169);
            this.Controls.Add(this.figure1);
            this.Name = "Form";
            this.Text = "Form3";
            this.ResumeLayout(false);

        }



        #endregion

        public FigurCommentUT figure1;
    }
}