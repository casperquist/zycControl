﻿namespace ZYCControl
{
    partial class ToolTest
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
            this.ascan_Ruler1 = new ZYCControl.Ascan_Ruler();
            this.SuspendLayout();
            // 
            // ascan_Ruler1
            // 
            this.ascan_Ruler1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ascan_Ruler1.Location = new System.Drawing.Point(0, 0);
            this.ascan_Ruler1.Name = "ascan_Ruler1";
            this.ascan_Ruler1.Size = new System.Drawing.Size(284, 261);
            this.ascan_Ruler1.TabIndex = 0;
            // 
            // ToolTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.ascan_Ruler1);
            this.Name = "ToolTest";
            this.Text = "ToolTest";
            this.ResumeLayout(false);

        }

        #endregion

        public Ascan_Ruler ascan_Ruler1;
    }
}