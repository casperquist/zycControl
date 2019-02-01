namespace ZYCControl
{
    partial class UltravisionField
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
            this.ultraToolLayout1 = new ZYCControl.UltraToolLayout();
            this.SuspendLayout();
            // 
            // ultraToolLayout1
            // 
            this.ultraToolLayout1.BackColor = System.Drawing.Color.Transparent;
            this.ultraToolLayout1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraToolLayout1.endX = 0D;
            this.ultraToolLayout1.endY = 0D;
            this.ultraToolLayout1.Location = new System.Drawing.Point(0, 0);
            this.ultraToolLayout1.Name = "ultraToolLayout1";
            this.ultraToolLayout1.Size = new System.Drawing.Size(326, 293);
            this.ultraToolLayout1.startX = 0D;
            this.ultraToolLayout1.startY = 0D;
            this.ultraToolLayout1.TabIndex = 0;
            this.ultraToolLayout1.SizeChanged += new System.EventHandler(this.ultraToolLayout1_SizeChanged);
            this.ultraToolLayout1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ultraToolLayout1_KeyPress);
            // 
            // UltravisionField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraToolLayout1);
            this.Name = "UltravisionField";
            this.Size = new System.Drawing.Size(326, 293);
            this.ResumeLayout(false);

        }

        #endregion

        public UltraToolLayout ultraToolLayout1;
    }
}
