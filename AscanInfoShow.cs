using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public string UnitX, UnitY;
        
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
                    bool drawAll = DrawInfo(g, posX, py, data[0].x[index], y, c, height, n, width);
                }
            }
        }

        private bool DrawInfo(Graphics g, int posx, int[] posy, float x, float[] y, Color[] c, int height, int n, int width)
        {    
            ResetOrder(ref posy, ref y, ref c, n);
            Rectangle preRect = new Rectangle();
            int k = 0;
            for (k = 0; k <n; k++)
            {
                if (posy[k] < height / 2)
                    preRect = DarwSingle(g, posx, posy[k], x, y[k], c[k], height, width, preRect);
                else
                    break;
            }
            preRect = new Rectangle();
            preRect.Y = height;
            for (int i = n-1; i>=k; i--)
            {
                preRect = DarwSingle(g, posx, posy[i], x, y[i], c[i], height, width, preRect);
            }
            
            return true;

        }

        private void ResetOrder(ref int[] posy, ref float[] y, ref Color[] c, int n)
        {            
            Dictionary<int, paraOrder> a = new Dictionary<int, paraOrder>();
            for (int i = 0; i < n; i++)
                a.Add(i,new paraOrder(posy[i],y[i],c[i]));
            Dictionary<int, paraOrder> a1 = a.OrderBy(o => o.Value.posy).ToDictionary(o => o.Key, p => p.Value);
            int k = 0;
            foreach (var t in a1)
            {
                posy[k] = t.Value.posy;
                y[k] = t.Value.y;
                c[k] = t.Value.c;
                k++;
            }

        }     
        
        private Rectangle DarwSingle(Graphics g, int posx, int posy, float x, float y, Color c, int height, int width, Rectangle preRect)
        {
            if (font == null)
                font = new Font("宋体", 8);
            string info = x.ToString() +UnitX+ ";" + y.ToString(StrFormat)+UnitY;
            SizeF ss = g.MeasureString(info, font);
            Size s = new Size((int)ss.Width, (int)ss.Height);
            Point p = new Point();
            if (posx > width / 2)
                p.X = posx - s.Width;
            else
                p.X = posx;

            if (posy > height / 2)
            {
                p.Y = posy - s.Height;
                if (posy> preRect.Y)
                    p.Y = preRect.Y - s.Height;
            }
            else
            {
                p.Y = posy;
                if (p.Y < preRect.Bottom)
                    p.Y = preRect.Y + s.Height;
            }
            g.DrawString(info, font, new SolidBrush(c), p);
            return new Rectangle(p, s);
        }

        private void ExchangeOder(ref int[] posy, ref float[] y, ref Color[] c, int a, int b)
        {
            int midp = posy[a];
            float midy = y[a];
            Color midc = c[a];
            posy[a] = posy[b];
            y[a] = y[b];
            c[a] = c[b];
            posy[b] = midp; ;
            y[b] = midy;
            c[b] = midc;
        }
        
        private float StringWidth(Graphics g, string info)
        {
            return g.MeasureString(info, font).Width;
        }

        class paraOrder
        {
            public int posy;
            public float y;
            public Color c;

            public paraOrder(int a, float b, Color s)
            {
                posy = a;
                y = b;
                c = s;
            }
        }

    }

    

}
