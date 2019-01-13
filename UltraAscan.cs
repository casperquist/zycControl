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
    public partial class UltraAscan : UserControl
    {
        public bool envelopActived;
        public series data;
        private series envelop;
        public float xStart;
        public float xEnd;
        private List<series> listData;
        public Gate gate0, gate1, gate2, gate3;

        public UltraAscan()
        {
            InitializeComponent();

            EventAndRespone();
        }

        public void FigureInitial()
        {
            listData = new List<series>(2);
            listData.Add(data);
            longStrip1.NewImage(listData, true, new float[] {xStart,xEnd,0,100});

            rulerBarH.HoriBar = true;
            rulerBarH.startValue = xStart;
            rulerBarH.endValue = xEnd;
            rulerBarH.Draw();

            rulerBarV.HoriBar = false;
            rulerBarV.startValue = 0;
            rulerBarV.endValue = 100;
            rulerBarV.Draw();

            gate0 = new Gate();
            gate0.g = longStrip1.CreateGraphics();
            gate1 = new Gate();
            gate1.g = longStrip1.CreateGraphics();
            gate2 = new Gate();
            gate2.g = longStrip1.CreateGraphics();
            gate3 = new Gate();
            gate3.g = longStrip1.CreateGraphics();
        }

        private void UltraAscan_SizeChanged(object sender, EventArgs e)
        {
            longStrip1.Location = new Point(30, 0);
            longStrip1.Width = Width - 30;
            longStrip1.Height = Height - 30;

            rulerBarV.Height = Height - 30; ;

            rulerBarH.Width = Width - 30;
            rulerBarH.Location = new Point(30, longStrip1.Height);

            if (rulerBarH.Width != 0 &
                rulerBarV.Height != 0)
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

        private void UltraAscan_MouseMove(object sender, MouseEventArgs e)
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

        private void UpdataEnvelop()
        {
            if (envelop == null)
            {
                data.CopyTo(envelop);
                listData.Add(envelop);
            }
            else
            {
                int n = envelop.Count;
                for (int i = 0; i < n; i++)
                    envelop.y[i] = envelop.y[i] >= data.y[i] ? envelop.y[i] : data.y[i];
            }
        }

        public void ReCal()
        {
            if (envelopActived)
                UpdataEnvelop();
            else
            {
                envelop = null;
                if (listData !=null)
                    listData.Remove(envelop);
            }
                        
            gate0.DrawGate();
            gate1.DrawGate();
            gate2.DrawGate();
            gate3.DrawGate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            if (longStrip1.ima != null)
                graphics.DrawImage(longStrip1.ima.bmp, 0, 0);
            try { ReCal(); } catch { }
            
            //GC.Collect();
        }
    }

    public class Gate
    {
        public Color color;
        public float Threshold;
        public int locationY;
        public float startD;
        public float endD;
        public int startPixel;
        public int endPixel;
        public Graphics g;
        public bool Actived;
        private int lineWidth = 3;
        public int gHeight,gWidth;
        public bool CourseAtEnd;
        public bool CourseAtMid;
        public float startX, endX, startY, endY;
        public Point courseLocation;
               

        public void Data2Pxiel()
        {
            
            locationY = (int)((endY - Threshold) / (endY - startY) * gHeight);
            startPixel = (int)((startD - startX) / (endX - startX) * gWidth);
            endPixel = (int)((startD - endX) / (endX - startX) * gWidth);
        }

        public void Pixel2Data()
        {
            Threshold = endY - locationY * (endY - startY) / gHeight;
            startD = startX + startPixel * (endX - startX) / gWidth;
            startD = startX + startPixel * (endX - startX) / gWidth;
        }

        public void CourseState()
        {
            Rectangle rectangle1 = new Rectangle(startPixel + 1, locationY - 1, endPixel - startPixel-2, 3);
            Rectangle rectangle0 = new Rectangle(startPixel, locationY - 1, 1, 3);
            Rectangle rectangle2 = new Rectangle(endPixel, locationY - 1, 1, 3);

            if (rectangle1.Contains(courseLocation))
            {
                CourseAtMid = true;
            }
            else
                CourseAtMid = false;

            if (rectangle0.Contains(courseLocation) || rectangle2.Contains(courseLocation))
                CourseAtEnd = true;
            else
                CourseAtEnd = false;

        }

        public void DrawGate()
        {
            Data2Pxiel();
            CourseState();
            if (Actived)
                g.DrawLine(new Pen(color, lineWidth), new Point(startPixel,locationY), new Point(endPixel,locationY));
        }

    }
}
