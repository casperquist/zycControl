﻿using System;
using System.Collections.Generic;

namespace ZYCControl
{
    partial class Figure
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {             
            this.SuspendLayout();
            // 
            // Figure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Name = "Figure";
            this.SizeChanged += new System.EventHandler(this.Figure_SizeChanged);
            this.DoubleClick += new System.EventHandler(this.Figure_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Figure_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Figure_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Figure_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Figure_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Figure_MouseUp);
            this.ResumeLayout(false);

        }

        

        #endregion
    }
}
