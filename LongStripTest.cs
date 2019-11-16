using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZYCControl
{
    public partial class LongStripTest : Form
    {
        public LongStripTest()
        {
            InitializeComponent();
            
        }

        public float stepX;
        public float stepY;
        public float startX;
        public float endX;
        public float startY;
        public float endY;
        public string InfoFormat;
        public float judge0Y;
        public float judge1y;
        public bool judge0Enable;
        public bool judge1Enable;
        public Color judge0Color;
        public Color judge1Color;
        public List<series> data;
        public Font tipFont;
        public string tipFormat;
        public string tipXunit;
        public string tipYunit;
        public void Initial(bool showDialog)
        {
            longStrip_Ruler1.longStrip_1.StepX = stepX;
            longStrip_Ruler1.longStrip_1.StepY = stepY;
            longStrip_Ruler1.longStrip_1.StartX = startX;
            longStrip_Ruler1.longStrip_1.EndX = endX;
            longStrip_Ruler1.longStrip_1.StartY = startY;
            longStrip_Ruler1.longStrip_1.EndY = endY;
            longStrip_Ruler1.longStrip_1.NewImage(data, true, new float[] { startX, endX, startY, endY });
            longStrip_Ruler1.longStrip_1.longStripToolLayout1.JudgeLine0.InfoFormat = InfoFormat;
            longStrip_Ruler1.longStrip_1.longStripToolLayout1.JudgeLine1.InfoFormat = InfoFormat;
            longStrip_Ruler1.longStrip_1.longStripToolLayout1.JudgeLine0.Y = judge0Y;
            longStrip_Ruler1.longStrip_1.longStripToolLayout1.JudgeLine1.Y = judge1y;
            longStrip_Ruler1.longStrip_1.longStripToolLayout1.JudgeLine0.color = judge0Color;
            longStrip_Ruler1.longStrip_1.longStripToolLayout1.JudgeLine1.color = judge1Color;
            longStrip_Ruler1.longStrip_1.longStripToolLayout1.JudgeLine0.Enable = judge0Enable;
            longStrip_Ruler1.longStrip_1.longStripToolLayout1.JudgeLine1.Enable = judge1Enable;

            longStrip_Ruler1.longStrip_1.longStripToolLayout1.infoShow.data = data;
            longStrip_Ruler1.longStrip_1.longStripToolLayout1.infoShow.StrFormat = tipFormat;
            longStrip_Ruler1.longStrip_1.longStripToolLayout1.infoShow.startx0 = startX;
            longStrip_Ruler1.longStrip_1.longStripToolLayout1.infoShow.font = tipFont;
            longStrip_Ruler1.longStrip_1.longStripToolLayout1.infoShow.UnitX = tipXunit;
            longStrip_Ruler1.longStrip_1.longStripToolLayout1.infoShow.UnitY = tipYunit;
            longStrip_Ruler1.ReDrawRuler();

            StartPosition = FormStartPosition.CenterParent;
            if (showDialog)
                ShowDialog();
            else
                Show();
        }
        
    }
}
