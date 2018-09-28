namespace ZYCControl
{
    partial class LongTrip
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
            this.bmpShow1 = new ZYCControl.BmpShow();
            this.transparentInfo1 = new ZYCControl.TransparentInfo();
            this.SuspendLayout();
            // 
            // bmpShow1
            // 
            this.bmpShow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bmpShow1.Location = new System.Drawing.Point(0, 0);
            this.bmpShow1.Name = "bmpShow1";
            this.bmpShow1.Size = new System.Drawing.Size(150, 150);
            this.bmpShow1.TabIndex = 0;
            // 
            // transparentInfo1
            // 
            this.transparentInfo1.BackColor = System.Drawing.Color.Transparent;
            this.transparentInfo1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.transparentInfo1.JudgeLine0 = 0F;
            this.transparentInfo1.JudgeLine0Enable = false;
            this.transparentInfo1.JudgeLine1 = 0F;
            this.transparentInfo1.JudgeLine1Enable = false;
            this.transparentInfo1.JudgeLine2 = 0F;
            this.transparentInfo1.JudgeLine2Enable = false;
            this.transparentInfo1.Location = new System.Drawing.Point(0, 0);
            this.transparentInfo1.Name = "transparentInfo1";
            this.transparentInfo1.Size = new System.Drawing.Size(150, 150);
            this.transparentInfo1.TabIndex = 1;
            // 
            // LongTrip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.transparentInfo1);
            this.Controls.Add(this.bmpShow1);
            this.Name = "LongTrip";
            this.ResumeLayout(false);

        }

        #endregion

        public BmpShow bmpShow1;
        public TransparentInfo transparentInfo1;
    }
}
