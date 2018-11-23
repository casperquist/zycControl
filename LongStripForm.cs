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
    public partial class LongStripForm : Form
    {
        private int num;

        public LongStripForm()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.  
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲  
            InitializeComponent();

            EventAndRespone();
        }

        public void FigureInitial(List<series> a, bool fixRange, float[] range)
        {
            longStrip1.NewImage(a, fixRange, range);

            rulerBarH.HoriBar = true;
            rulerBarH.startValue = a[0].x.Min();
            rulerBarH.endValue = a[0].x.Max();
            rulerBarH.Draw();

            rulerBarV.HoriBar = false;
            rulerBarV.startValue = longStrip1.ima.y0;
            rulerBarV.endValue = longStrip1.ima.y1;
            rulerBarV.Draw();

            num = 0;
            timer1.Interval = 10;
            //timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            longStrip1.ima.preData = copy(longStrip1.ima.rawData);
            int step = 10;
            int count = longStrip1.ima.rawData[0].Count;
            if (num < count)
            {
                num += step;
            }

            int s;
            for (int i = 0; i < step; i++)
            {
                s = (i + num) % count;
                longStrip1.ima.rawData[0].y[s] = 0;
                longStrip1.ima.rawData[1].y[s] = 0;
            }
            if (WindowState != FormWindowState.Minimized)
                longStrip1.ima.ControlActived = true;
            else
                longStrip1.ima.ControlActived = false;
            longStrip1.ima.Refresh(false);
            longStrip1.Refresh();
        }

        private List<series> copy(List<series> a)
        {
            int n = a.Count;
            List<series> b = new List<series>(n);
            for (int i = 0; i < n; i++)
            {
                series tmp = new series();
                a[i].CopyTo(tmp);
                b.Add(tmp);
            }
            return b;
        }

        private void LongStripForm_SizeChanged(object sender, EventArgs e)
        {
            longStrip1.Location = new Point(30, 0);
            longStrip1.Width = ClientSize.Width - 30;
            longStrip1.Height = ClientSize.Height - 30;

            rulerBarV.Height = ClientSize.Height - 30;;

            rulerBarH.Width = ClientSize.Width - 30;
            rulerBarH.Location = new Point(30, longStrip1.Height);

            if (rulerBarH.Width != 0 &
                rulerBarV.Height != 0 )
            {
                rulerBarH.Draw();
                rulerBarV.Draw();
            }
        }

        private void EventAndRespone()
        {
            longStrip1.RangeChange += new RangeChangedHandleEvent(rulerBarH.ResponeEvent);
            longStrip1.RangeChange += new RangeChangedHandleEvent(rulerBarV.ResponeEvent);
            rulerBarH.RangeChanged += new RangeChangedHandleEvent(longStrip1.ResponeEvent);
            rulerBarV.RangeChanged += new RangeChangedHandleEvent(longStrip1.ResponeEvent);

        }

        private void LongStripForm_MouseMove(object sender, MouseEventArgs e)
        {
            Point np = this.PointToScreen(e.Location);
            Rectangle rc_ls = RectangleToScreen(longStrip1.ClientRectangle);
            Rectangle rc_rh = RectangleToScreen(rulerBarH.ClientRectangle);
            Rectangle rc_rv = RectangleToScreen(rulerBarV.ClientRectangle);

            if (rc_rh.Contains(np))
                rulerBarH.Select();

            if (rc_rv.Contains(np))
                rulerBarV.Select();

            if (rc_ls.Contains(np))
                longStrip1.Select();

        }
    }
}
