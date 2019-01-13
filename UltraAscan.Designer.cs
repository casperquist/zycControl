namespace ZYCControl
{
    partial class UltraAscan
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
            this.longStrip1 = new ZYCControl.LongStrip();
            this.SuspendLayout();
            // 
            // rulerBarV
            // 
            this.rulerBarV.BackColor = System.Drawing.Color.Transparent;
            this.rulerBarV.Location = new System.Drawing.Point(0, 0);
            this.rulerBarV.Name = "rulerBarV";
            this.rulerBarV.Size = new System.Drawing.Size(30, 349);
            this.rulerBarV.TabIndex = 0;
            // 
            // rulerBarH
            // 
            this.rulerBarH.BackColor = System.Drawing.Color.Transparent;
            this.rulerBarH.Location = new System.Drawing.Point(30, 349);
            this.rulerBarH.Name = "rulerBarH";
            this.rulerBarH.Size = new System.Drawing.Size(445, 30);
            this.rulerBarH.TabIndex = 1;
            // 
            // longStrip1
            // 
            this.longStrip1.JudgeLine0 = 0F;
            this.longStrip1.JudgeLine0Enable = false;
            this.longStrip1.JudgeLine1 = 0F;
            this.longStrip1.JudgeLine1Enable = false;
            this.longStrip1.JudgeLine2 = 0F;
            this.longStrip1.JudgeLine2Enable = false;
            this.longStrip1.Location = new System.Drawing.Point(30, 0);
            this.longStrip1.Name = "longStrip1";
            this.longStrip1.Size = new System.Drawing.Size(445, 343);
            this.longStrip1.TabIndex = 2;
            // 
            // UltraAscan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.longStrip1);
            this.Controls.Add(this.rulerBarH);
            this.Controls.Add(this.rulerBarV);
            this.Name = "UltraAscan";
            this.Size = new System.Drawing.Size(475, 379);
            this.SizeChanged += new System.EventHandler(this.UltraAscan_SizeChanged);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UltraAscan_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private RulerBar rulerBarV;
        private RulerBar rulerBarH;
        private LongStrip longStrip1;
    }
}
