﻿using System;
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
    public partial class Ascan_Ruler : UserControl
    {
        public Ascan_Ruler()
        {
            InitializeComponent();
            EventAndRespone();
        }
        
        private void EventAndRespone()
        {
            ascan_1.RangeChange += new RangeChangedHandleEvent(rulerBarH.ResponeEvent);
            ascan_1.RangeChange += new RangeChangedHandleEvent(rulerBarV.ResponeEvent);
            //rulerBarH.RangeChanged += new RangeChangedHandleEvent(ascan_1.ResponeEvent);
            //rulerBarV.RangeChanged += new RangeChangedHandleEvent(ascan_1.ResponeEvent);
        }

        private void ascan_1_MouseMove(object sender, MouseEventArgs e)
        {
            Point np = this.PointToScreen(e.Location);
            Rectangle rc_ls = RectangleToScreen(ascan_1.ClientRectangle);
            Rectangle rc_rh = RectangleToScreen(rulerBarH.ClientRectangle);
            Rectangle rc_rv = RectangleToScreen(rulerBarV.ClientRectangle);

            if (rc_rh.Contains(np))
                rulerBarH.Select();

            if (rc_rv.Contains(np))
                rulerBarV.Select();

            if (rc_ls.Contains(np))
                ascan_1.Select();
        }

        private void Ascan_Ruler_SizeChanged(object sender, EventArgs e)
        {
            ascan_1.Location = new Point(30, 0);
            ascan_1.Width = Width - 30;
            ascan_1.Height = Height - 30;

            rulerBarV.Height = Height - 30;

            rulerBarH.Width = Width - 30;
            rulerBarH.Location = new Point(30, ascan_1.Height);

            if (rulerBarH.Width != 0 &
                rulerBarV.Height != 0)
            {
                ReDrawRuler();
            }
        }

        public void ReDrawRuler()
        {
            rulerBarH.startValue = ascan_1.StartX;
            rulerBarH.endValue = ascan_1.EndX;
            rulerBarV.startValue = ascan_1.StartY;
            rulerBarV.endValue = ascan_1.EndY;
            rulerBarH.Draw();
            rulerBarV.Draw();
        }
    }
}
