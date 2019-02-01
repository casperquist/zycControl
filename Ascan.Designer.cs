namespace ZYCControl
{
    partial class Ascan_
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
            this.ascanToolLayout1 = new ZYCControl.AscanToolLayout();
            this.SuspendLayout();
            // 
            // ascanToolLayout1
            // 
            this.ascanToolLayout1.BackColor = System.Drawing.Color.Transparent;
            this.ascanToolLayout1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ascanToolLayout1.EndX = 0F;
            this.ascanToolLayout1.EndY = 0F;
            this.ascanToolLayout1.Location = new System.Drawing.Point(0, 0);
            this.ascanToolLayout1.Name = "ascanToolLayout1";
            this.ascanToolLayout1.Size = new System.Drawing.Size(318, 268);
            this.ascanToolLayout1.StartX = 0F;
            this.ascanToolLayout1.StartY = 0F;
            this.ascanToolLayout1.StepX = 0F;
            this.ascanToolLayout1.StepY = 0F;
            this.ascanToolLayout1.TabIndex = 0;
            this.ascanToolLayout1.SizeChanged += new System.EventHandler(this.ascanToolLayout1_SizeChanged);
            this.ascanToolLayout1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ascanToolLayout1_KeyPress);
            // 
            // Ascan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ascanToolLayout1);
            this.Name = "Ascan";
            this.Size = new System.Drawing.Size(318, 268);
            this.ResumeLayout(false);

        }

        #endregion

        public AscanToolLayout ascanToolLayout1;
    }
}
