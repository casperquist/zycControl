using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ZYCControl
{
    public class FigCursor
    {        
        /// <summary>
        /// beam的角度
        /// </summary>
        public double angle;
        /// <summary>
        /// 光标的颜色
        /// </summary>
        public Color color;
        /// <summary>
        /// 光标的交点位置
        /// </summary>
        public Point p;
        /// <summary>
        /// 光标的交点所对应的数据点
        /// </summary>
        public PointF pdata;
        /// <summary>
        /// beam起点为(beamPx,0)
        /// </summary>
        public int beamPx;
        public Point beamEndP;
        public string InfoFormat;
        public Font InfoFont;
        /// <summary>
        /// 光标的名称
        /// </summary>
        public string Name;        
        /// <summary>
        /// x轴光标状态
        /// </summary>
        public bool xSelected
        {
            set { _xSelected = value; _ySelected = !value;}
            get { return _xSelected; }
        }
        /// <summary>
        /// y轴光标状态
        /// </summary>
        public bool ySelected
        {
            set { _xSelected = !value; _ySelected = value; }
            get { return _ySelected; }
        }
        /// <summary>
        /// 光标状态
        /// </summary>
        public bool Selected
        {
            set { _xSelected = _ySelected = value; }
            get { return _xSelected || _ySelected; }
        }

        /// <summary>
        /// x轴，y轴光标状态
        /// </summary>
        private bool _xSelected, _ySelected;

        public void DrawCursor(Graphics g, Size ParentControlSize, bool DrawBeamLine)
        {
            int w = ParentControlSize.Width;
            int h = ParentControlSize.Height;
            Pen pen = new Pen(color);
            Pen blank = new Pen(Color.White);
            g.DrawLine(pen, 0, p.Y, w - 1, p.Y);
            g.DrawLine(blank, 0, p.Y+1, w - 1, p.Y+1);
            g.DrawLine(pen, p.X, 0, p.X, h - 1);
            g.DrawLine(blank, p.X+1, 0, p.X+1, h - 1);

            string strx = string.Format(InfoFormat, pdata.X);
            string stry = string.Format(InfoFormat, pdata.Y);

            SizeF sizex = g.MeasureString(strx, InfoFont);
            Size sizesx = new Size((int)sizex.Width + 1, (int)sizex.Height + 1);
            SizeF sizey = g.MeasureString(stry, InfoFont);
            Size sizesy = new Size((int)sizey.Width + 1, (int)sizey.Height + 1);

            Point ps = new Point();
            if (p.X < w / 2)
                ps = new Point(p.X, 0);
            else
                ps = new Point(p.X - sizesx.Width, 0);
            g.FillRectangle(new SolidBrush(Color.White), ps.X, ps.Y, sizesx.Width, sizesx.Height);
            g.DrawString(strx, InfoFont, new SolidBrush(Color.Black), ps);

            if (p.Y < h / 2)
                ps = new Point(0, p.Y);
            else
                ps = new Point(0, p.Y - sizesy.Height);
            g.FillRectangle(new SolidBrush(Color.White), ps.X, ps.Y, sizesx.Width, sizesx.Height);
            g.DrawString(stry, InfoFont, new SolidBrush(Color.Black), ps);

            if (DrawBeamLine)
                DrawBeam(g, ParentControlSize);
        }

        public string MouseState(Point mouse)
        {
            Point dp = new Point(mouse.X - p.X, mouse.Y - p.Y);
            double dpx = Math.Abs(dp.X);
            double dpy = Math.Abs(dp.Y);
            if (dpx < 5)
                return "X";
            if (dpy < 5)
                return "Y";
            return null;
        }

        public void DrawBeam(Graphics g, Size ParentControlSize)
        {
            Color beamColor;
            if (color == Color.Red)
                beamColor = Color.Pink;
            else
                beamColor = Color.Green;

            int w = ParentControlSize.Width;
            int h = ParentControlSize.Height;

            double ang = angle * Math.PI / 180.0;
            double length0, length1;
            length1 = (h-1) / Math.Cos(ang);
            Point tmp0,tmp1 = new Point(beamPx+(int)(Math.Sin(ang)*length1),h-1);
            if (ang > 0)
            {
                length0 = (w - 1 - beamPx) / Math.Sin(ang);
                tmp0 = new Point(w - 1, (int)(Math.Cos(ang) * length0));
            }
            else
            {
                if (ang == 0)
                {
                    length0 = h - 1;
                    tmp0 = new Point(beamPx, h - 1);
                }
                else
                {
                    length0 = beamPx / Math.Sin(ang);
                    tmp0 = new Point(0, (int)(Math.Cos(ang) * length0));
                }
            }

            bool flage = true;
            if (length0 < length1)
            {
                if (length0 < 0)
                {
                    beamEndP = new Point(-1, -1);
                    flage = false;
                }
                else
                    beamEndP = tmp0;
            }
            else
                beamEndP = tmp1;

            if (flage)
            {
                g.DrawLine(new Pen(beamColor), new Point(beamPx, 0), beamEndP);
                g.FillRectangle(new SolidBrush(beamColor), beamPx - 1, 0, 3, 3);
                g.FillRectangle(new SolidBrush(beamColor), beamEndP.X - 1, beamEndP.Y - 1, 3, 3);
            }
        }
    }
}
