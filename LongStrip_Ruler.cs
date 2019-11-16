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
    public partial class LongStrip_Ruler : UserControl
    {
        public LongStrip_Ruler()
        {
            InitializeComponent();
            EventAndRespone();
        }

        private void EventAndRespone()
        {
            longStrip_1.RangeChange += new RangeChangedHandleEvent(rulerBarH.ResponeEvent);
            longStrip_1.RangeChange += new RangeChangedHandleEvent(rulerBarV.ResponeEvent);
            rulerBarH.RangeChanged += new RangeChangedHandleEvent(longStrip_1.ResponeEvent);
            //rulerBarV.RangeChanged += new RangeChangedHandleEvent(longStrip_1.ResponeEvent);
        }

        private void longStrip_1_MouseMove(object sender, MouseEventArgs e)
        {
            Point np = this.PointToScreen(e.Location);
            Rectangle rc_ls = RectangleToScreen(longStrip_1.ClientRectangle);
            Rectangle rc_rh = RectangleToScreen(rulerBarH.ClientRectangle);
            Rectangle rc_rv = RectangleToScreen(rulerBarV.ClientRectangle);

            if (rc_rh.Contains(np))
                rulerBarH.Select();

            if (rc_rv.Contains(np))
                rulerBarV.Select();

            if (rc_ls.Contains(np))
                longStrip_1.Select();
        }

        private void LongStrip_Ruler_SizeChanged(object sender, EventArgs e)
        {
            longStrip_1.Location = new Point(30, 0);
            longStrip_1.Width = Width - 30;
            longStrip_1.Height = Height - 30;

            rulerBarV.Height = Height - 30;

            rulerBarH.Width = Width - 30;
            rulerBarH.Location = new Point(30, longStrip_1.Height);

            if (rulerBarH.Width != 0 &
                rulerBarV.Height != 0)
            {
                ReDrawRuler();
            }
        }

        public void ReDrawRuler()
        {
            rulerBarH.startValue = longStrip_1.StartX;
            rulerBarH.endValue = longStrip_1.EndX;
            rulerBarV.startValue = longStrip_1.StartY;
            rulerBarV.endValue = longStrip_1.EndY;
            rulerBarH.Draw();
            rulerBarV.Draw();
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
            longStrip_1.StepX = stepX;
            longStrip_1.StepY = stepY;
            longStrip_1.StartX = startX;
            longStrip_1.EndX = endX;
            longStrip_1.StartY = startY;
            longStrip_1.EndY = endY;
            longStrip_1.NewImage(data, true, new float[] { startX, endX, startY, endY });
            longStrip_1.longStripToolLayout1.JudgeLine0.InfoFormat = InfoFormat;
            longStrip_1.longStripToolLayout1.JudgeLine1.InfoFormat = InfoFormat;
            longStrip_1.longStripToolLayout1.JudgeLine0.Y = judge0Y;
            longStrip_1.longStripToolLayout1.JudgeLine1.Y = judge1y;
            longStrip_1.longStripToolLayout1.JudgeLine0.color = judge0Color;
            longStrip_1.longStripToolLayout1.JudgeLine1.color = judge1Color;
            longStrip_1.longStripToolLayout1.JudgeLine0.Enable = judge0Enable;
            longStrip_1.longStripToolLayout1.JudgeLine1.Enable = judge1Enable;

            longStrip_1.longStripToolLayout1.infoShow.data = data;
            longStrip_1.longStripToolLayout1.infoShow.StrFormat = tipFormat;
            longStrip_1.longStripToolLayout1.infoShow.startx0 = startX;
            longStrip_1.longStripToolLayout1.infoShow.font = tipFont;
            longStrip_1.longStripToolLayout1.infoShow.UnitX = tipXunit;
            longStrip_1.longStripToolLayout1.infoShow.UnitY = tipYunit;
            ReDrawRuler();
                        
        }
    }
}
