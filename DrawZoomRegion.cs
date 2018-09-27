using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZYCControl
{
    class DrawZoomRegion
    {
        private Point _p1;
        public Point p0;
        public Point p1
        {
            get { return _p1; }
            set { if (value != _p1)
                {
                    pChanged = true;
                    _p1 = value;
                }
                else
                    pChanged = false;
            }
        }
        
        public int minX,  minY, width, height;
        public bool pChanged;
        public Graphics g;

                             
        /// <summary>
        /// 画矩形
        /// </summary>
        /// <param name="FillOrEdge">填充，还是只画边框</param>
        public void Draw(bool FillOrEdge)
        {
            minX = (p0.X < _p1.X ? p0.X : _p1.X);
            minY = (p0.Y < _p1.Y ? p0.Y : _p1.Y);
            width = Math.Abs(p0.X - _p1.X);
            height = Math.Abs(p0.Y - _p1.Y);

            SolidBrush b = new SolidBrush(Color.FromArgb(125, Color.Gray));
            Rectangle r = new Rectangle(minX, minY, width, height);
            if (FillOrEdge)
                g.FillRectangle(b, r);
            else
                g.DrawRectangle(new Pen(b), r);
                
            
        }
    }
}
