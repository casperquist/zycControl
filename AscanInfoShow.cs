using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace ZYCControl
{
    public class AscanInfoShow
    {
        public bool Enable;
        public List<series> data;
        public Font font;
        public string StrFormat;
        public float startx0;
        
        public void Draw(Graphics g, float startx, float endx, float starty, float endy, 
            int posX, int width, int height, float stepx)
        {
            if (Enable)
            {
                float x = posX / (width - 1.0f) * (endx - startx) + startx;
                x = (float)(Math.Round((x - startx) / stepx) * stepx + startx);
                int posX1 = (int)(Math.Round(
                    (x - startx) * (width - 1) / (endx - startx)));

                int index = (int)((x - startx0) / stepx);
                if (index >0 && index < data[0].Count)
                {
                    int n = data.Count;
                    g.DrawLine(new Pen(Color.Blue), posX1, 0, posX1, height);

                    int[] py = new int[n];
                    float[] y = new float[n];
                    Color[] c = new Color[n];
                    for (int i = 0; i < n; i++)
                    {
                        y[i] = data[i].y[index];
                        py[i] = (int)Math.Round(height - 1 - (y[i] - starty) * (height - 1.0) / (endy - starty));
                        c[i] = data[i].sColor;
                    }
                    bool drawAll = DrawInfo(g, posX, py, y, c, height, n, width);
                }
            }
        }

        private bool DrawInfo(Graphics g, int posx, int[] posy, float[] y, Color[] c, int height, int n, int width)
        {           

            SizeF ss = g.MeasureString(StrFormat, font);
            Size s = new Size((int)ss.Width, (int)ss.Height);
            List<Rectangle> existR = new List<Rectangle>(n);
            //posx = posx > width / 2 ? posx - (int)s.Width+1 : posx;
            bool infoR = false;
            if (posx > width / 2)
                infoR = true;

            int[] partIndex = ResetOrder(ref posy, ref y, ref c, n, height);
            int si, ei;
            string info;
            /// 画上半部分信息
            si = partIndex[0];
            ei = partIndex[1];
            Rectangle rectangle = new Rectangle();
            if (si > -1)
            {
                ei = ei == -1 ? n - 1 : ei;
                for (int i = si; i<= ei; i++)
                {
                    g.DrawRectangle(new Pen(c[i]), posx - 1, posy[i] - 1, 2, 2);
                    int py = posy[i];
                    if (rectangle.Contains(posx, py))
                        py = rectangle.Bottom + 1;
                    if (py >= height)
                        return false;
                    info = y[i].ToString(StrFormat);
                    if (infoR)
                    { }
                    g.DrawString(info, font, new SolidBrush(c[i]), posx + 1, py);
                    rectangle = new Rectangle(posx, py, s.Width, s.Height);
                }
            }
            int bottom = rectangle.Bottom;

            /// 画下半部分信息
            si = partIndex[1]+1;
            ei = partIndex[2];
            rectangle = new Rectangle();
            if (si > 0)
            {
                ei = ei < 0 ? n - 1 : ei;
                for (int i = ei; i <= si; i--)
                {
                    g.DrawRectangle(new Pen(c[i]), posx , posy[i] - 1, 2, 2);
                    int py = posy[i] - s.Height;
                    if (rectangle.Contains(posx, py))
                        py = rectangle.Top - s.Height;
                    if (py < bottom)
                        return false;
                    g.DrawString(y[i].ToString(StrFormat), font, new SolidBrush(c[i]), posx + 1, py);
                    rectangle = new Rectangle(posx, py, s.Width, s.Height);
                }
            }
            return true;

        }

        private int[] ResetOrder(ref int[] posy, ref float[] y, ref Color[] c, int n, int height)
        {
            int midpy;
            float midy;
            Color midc;
            float halfH = height / 2.0f;

            for (int i = 0; i < n-1; i++)
                for (int j = i; j < n-1; j++)
                {
                    if (y[j] < y[j+1])
                    {
                        midc = c[j];
                        midpy = posy[j];
                        midy = y[j];

                        c[j] = c[j + 1];
                        posy[j] = posy[j + 1];
                        y[j] = y[j + 1];

                        c[j + 1] = midc;
                        y[j + 1] = midy;
                        posy[j + 1] = midpy;
                    }
                }

            List<int> list = new List<int>(n);
            for (int i = 0; i < n; i++)
                list.Add(posy[i]);

            int[] index = new int[3];
            index[2] = list.FindIndex(t => t >= height) - 1;
            index[1] = list.FindIndex(t => t <= height / 2);
            index[0] = list.FindIndex(t => t >= 0);
            return index;         
        }     

        
        
    }
}
