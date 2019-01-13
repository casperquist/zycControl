using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ZYCControl
{
    public class ZoomRegion
    {
        public Point p0;
        public Point p1;
        

        public int minX, minY, width, height;


        /// <summary>
        /// 缩放区域的参数重置
        /// </summary>
        /// <param name="ParentsControl">父控件尺寸</param>
        private void ReSet(Size ParentsControlSize)
        {
            int w = ParentsControlSize.Width;
            int h = ParentsControlSize.Height;
            p1.X  = p1.X < 0 ? 0 : p1.X ;
            p1.X = p1.X >= w ? w - 1 : p1.X;
            p1.Y = p1.Y < 0 ? 0 : p1.Y;
            p1.Y = p1.Y >= h ? h - 1 : p1.Y;

            minX = (p0.X < p1.X ? p0.X : p1.X);
            minY = (p0.Y < p1.Y ? p0.Y : p1.Y);
            width = Math.Abs(p0.X - p1.X) + 1;
            height = Math.Abs(p0.Y - p1.Y) + 1;

            if (width <= 15)
            {
                minX = 0;
                width = w;
            }
            else
            {
                if (height <= 15)
                {
                    minY = 0;
                    height = h;
                }
            }
        }

        public void DrawRegion(Graphics g, Size ParentsControlSize)
        {
            ReSet(ParentsControlSize);
            if (width > ParentsControlSize.Width*0.9)
                Console.WriteLine("hehe");
            SolidBrush b = new SolidBrush(Color.FromArgb(96, Color.Gray));
            Rectangle r = new Rectangle(minX, minY, width, height);
            if (height > 1 && width > 1)
                g.FillRectangle(b, r);
        }
    }
}
