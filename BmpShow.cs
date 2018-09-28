using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZYCControl
{
    public partial class BmpShow : UserControl
    {
        public Bitmap bmp;

        public BmpShow()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            if (bmp != null)
                graphics.DrawImage(bmp, 0, 0);
            GC.Collect();
        }
    }
}
