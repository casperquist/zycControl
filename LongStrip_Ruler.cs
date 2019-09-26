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
    }
}
