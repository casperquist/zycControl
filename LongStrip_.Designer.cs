namespace ZYCControl
{
    partial class LongStrip_
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
            this.longStripToolLayout1 = new ZYCControl.LongStripToolLayout();
            this.SuspendLayout();
            // 
            // longStripToolLayout1
            // 
            this.longStripToolLayout1.BackColor = System.Drawing.Color.Transparent;
            this.longStripToolLayout1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.longStripToolLayout1.EndX = 0F;
            this.longStripToolLayout1.EndY = 0F;
            this.longStripToolLayout1.Location = new System.Drawing.Point(0, 0);
            this.longStripToolLayout1.Name = "longStripToolLayout1";
            this.longStripToolLayout1.Size = new System.Drawing.Size(150, 150);
            this.longStripToolLayout1.StartX = 0F;
            this.longStripToolLayout1.StartY = 0F;
            this.longStripToolLayout1.StepX = 0F;
            this.longStripToolLayout1.StepY = 0F;
            this.longStripToolLayout1.TabIndex = 0;
            this.longStripToolLayout1.SizeChanged += new System.EventHandler(this.longStripToolLayout1_SizeChanged);
            this.longStripToolLayout1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.longStripToolLayout1_KeyPress);
            // 
            // LongStrip_
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.longStripToolLayout1);
            this.Name = "LongStrip_";
            this.ResumeLayout(false);

        }

        #endregion

        public LongStripToolLayout longStripToolLayout1;
    }
}
