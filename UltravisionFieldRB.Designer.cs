namespace ZYCControl
{
    partial class UltravisionFieldRB
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.rulerBarV = new ZYCControl.RulerBar();
            this.rulerBarH = new ZYCControl.RulerBar();
            this.ultravisionField1 = new ZYCControl.UltravisionField();
            this.SuspendLayout();
            // 
            // rulerBarV
            // 
            this.rulerBarV.BackColor = System.Drawing.Color.Transparent;
            this.rulerBarV.Location = new System.Drawing.Point(0, 0);
            this.rulerBarV.Name = "rulerBarV";
            this.rulerBarV.Size = new System.Drawing.Size(30, 370);
            this.rulerBarV.TabIndex = 0;
            this.rulerBarV.HoriBar = false;
            // 
            // rulerBarH
            // 
            this.rulerBarH.BackColor = System.Drawing.Color.Transparent;
            this.rulerBarH.Location = new System.Drawing.Point(30, 370);
            this.rulerBarH.Name = "rulerBarH";
            this.rulerBarH.Size = new System.Drawing.Size(350, 30);
            this.rulerBarH.TabIndex = 1;
            this.rulerBarH.HoriBar = true;
            // 
            // ultravisionField1
            // 
            this.ultravisionField1.endX = 0D;
            this.ultravisionField1.endY = 0D;
            this.ultravisionField1.Location = new System.Drawing.Point(30, 0);
            this.ultravisionField1.Name = "ultravisionField1";
            this.ultravisionField1.Size = new System.Drawing.Size(370, 350);
            this.ultravisionField1.startX = 0D;
            this.ultravisionField1.startY = 0D;
            this.ultravisionField1.TabIndex = 2;
            // 
            // UltravisionFieldRB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultravisionField1);
            this.Controls.Add(this.rulerBarH);
            this.Controls.Add(this.rulerBarV);
            this.Name = "UltravisionFieldRB";
            this.Size = new System.Drawing.Size(400, 380);
            this.SizeChanged += new System.EventHandler(this.UltravisionFieldRB_SizeChanged);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UltravisionFieldRB_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private RulerBar rulerBarV;
        private RulerBar rulerBarH;
        public UltravisionField ultravisionField1;
    }
}
