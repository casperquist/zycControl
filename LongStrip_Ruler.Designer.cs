namespace ZYCControl
{
    partial class LongStrip_Ruler
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
            this.longStrip_1 = new ZYCControl.LongStrip_();
            this.SuspendLayout();
            // 
            // rulerBarV
            // 
            this.rulerBarV.BackColor = System.Drawing.Color.Transparent;
            this.rulerBarV.Location = new System.Drawing.Point(0, 0);
            this.rulerBarV.Name = "rulerBarV";
            this.rulerBarV.Size = new System.Drawing.Size(30, 300);
            this.rulerBarV.TabIndex = 0;
            // 
            // rulerBarH
            // 
            this.rulerBarH.BackColor = System.Drawing.Color.Transparent;
            this.rulerBarH.Location = new System.Drawing.Point(30, 300);
            this.rulerBarH.Name = "rulerBarH";
            this.rulerBarH.Size = new System.Drawing.Size(300, 30);
            this.rulerBarH.TabIndex = 1;
            this.rulerBarH.HoriBar = true;
            // 
            // longStrip_1
            // 
            this.longStrip_1.EndX = 0F;
            this.longStrip_1.EndY = 0F;
            this.longStrip_1.Location = new System.Drawing.Point(30, 0);
            this.longStrip_1.Name = "longStrip_1";
            this.longStrip_1.Size = new System.Drawing.Size(300, 300);
            this.longStrip_1.StartX = 0F;
            this.longStrip_1.StartY = 0F;
            this.longStrip_1.StepX = 0F;
            this.longStrip_1.StepY = 0F;
            this.longStrip_1.TabIndex = 2;
            // 
            // LongStrip_Ruler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.longStrip_1);
            this.Controls.Add(this.rulerBarH);
            this.Controls.Add(this.rulerBarV);
            this.Name = "LongStrip_Ruler";
            this.Size = new System.Drawing.Size(330, 330);
            this.SizeChanged += new System.EventHandler(this.LongStrip_Ruler_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public RulerBar rulerBarV;
        public RulerBar rulerBarH;
        public LongStrip_ longStrip_1;
    }
}
