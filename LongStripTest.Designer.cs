﻿using System;

namespace ZYCControl
{
    partial class LongStripTest
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
            this.components = new System.ComponentModel.Container();
            this.longStrip_Ruler1 = new ZYCControl.LongStrip_Ruler();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // longStrip_Ruler1
            // 
            this.longStrip_Ruler1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.longStrip_Ruler1.Location = new System.Drawing.Point(0, 0);
            this.longStrip_Ruler1.Name = "longStrip_Ruler1";
            this.longStrip_Ruler1.Size = new System.Drawing.Size(348, 322);
            this.longStrip_Ruler1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // LongStripTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 322);
            this.Controls.Add(this.longStrip_Ruler1);
            this.Name = "LongStripTest";
            this.Text = "LongStripTest";
            this.ResumeLayout(false);

        }

        

        #endregion

        public LongStrip_Ruler longStrip_Ruler1;
        private System.Windows.Forms.Timer timer1;
    }
}