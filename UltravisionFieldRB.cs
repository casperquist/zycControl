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
    public partial class UltravisionFieldRB : UserControl
    {
        public UltravisionFieldRB()
        {
            InitializeComponent();
            EventAndRespone();
        }
        
        public void Start()
        {
            ResetRulerBar();
        }

        private void ResetRulerBar()
        {
            ResetRulerBar(new float[] {0,0,0,0,
                (float)ultravisionField1.startX ,
                (float)ultravisionField1.endX,
                (float)ultravisionField1.startY ,
                (float)ultravisionField1.endY,
            });
        }

        private void ResetRulerBar(float[] para)
        {
            rulerBarH.startValue = para[4];
            rulerBarH.endValue = para[5];            
            rulerBarV.startValue = para[6];
            rulerBarV.endValue = para[7];
            RefreshRuleBar();
        }

        private void RefreshRuleBar()
        {
            if (rulerBarH.Width != 0 &
                rulerBarV.Height != 0)
            {
                rulerBarH.Draw();
                rulerBarV.Draw();
            }
        }

        private void ResetUltraField(float[] para)
        {
            ///Hori
            if (para[2] == 0)
            {
                ultravisionField1.startX = para[0];
                ultravisionField1.endX = para[1];
            }
            else
            {
                ultravisionField1.startY = para[0];
                ultravisionField1.endY = para[1];
            }
        }

        private void UltravisionFieldRB_SizeChanged(object sender, EventArgs e)
        {
            rulerBarV.Height = Height - 30;
            rulerBarH.Width = Width - 30;
            rulerBarH.Location = new Point(30, Height - 30);
            ultravisionField1.Size = new Size(Width - 30, Height - 30);
            RefreshRuleBar();
        }

        private void EventAndRespone()
        {
            ultravisionField1.ultraToolLayout1.RangeChanged +=
                new RangeChangedHandleEvent(ResetRulerBar);
            rulerBarH.RangeChanged += new RangeChangedHandleEvent(ResetUltraField);
            rulerBarV.RangeChanged += new RangeChangedHandleEvent(ResetUltraField);
            ultravisionField1.RangeChanged += new RangeChangedHandleEvent(ResetRulerBar);
        }

        private void UltravisionFieldRB_MouseMove(object sender, MouseEventArgs e)
        {
            if (RectangleToScreen(ultravisionField1.ClientRectangle).Contains(PointToScreen(e.Location)))
                ultravisionField1.Select();
            if (RectangleToScreen(rulerBarH.ClientRectangle).Contains(PointToScreen(e.Location)))
                rulerBarH.Select();
            if (RectangleToScreen(rulerBarV.ClientRectangle).Contains(PointToScreen(e.Location)))
                rulerBarV.Select();
        }
    }
}
