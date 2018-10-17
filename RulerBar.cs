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
    public partial class RulerBar : UserControl
    {
        public RulerBar()
        {            
            InitializeComponent();
        }

        #region 全局量
        /// <summary>
        /// 是否时水平标尺
        /// </summary>
        public bool HoriBar;
        /// <summary>
        /// 起始，结束值
        /// </summary>
        public float startValue, endValue;
        /// <summary>
        /// 最少高标尺数量
        /// </summary>
        public int minScalePN = 2;
        /// <summary>
        /// 最大高标尺数量
        /// </summary>
        public int maxScalePN = 10;

        private Bitmap bmp;
        /// <summary>
        /// 缩放之后的起始和结束值
        /// </summary>
        private float currentSV, currentEV;
        private Graphics g;
        /// <summary>
        /// （Tall线）像素间隔
        /// </summary>
        private float GapPixl;
        /// <summary>
        /// （Tall线）数值间隔
        /// </summary>
        private float GapV;
        /// <summary>
        /// value/pixel
        /// </summary>
        private float k;
        /// <summary>
        /// 水平标尺为width，数值标尺为height
        /// </summary>
        private float diffP;
        /// <summary>
        /// 高中低标尺线对应的像素位置
        /// </summary>
        private List<int> ScalePixelTall, ScalePixelMiddle, ScalePixelShort;
        /// <summary>
        /// 高标尺线对应的数值
        /// </summary>
        private List<float> tallInfo;
        /// <summary>
        /// 标尺线信息格式
        /// </summary>
        private string infoStringFormat;
        private bool MouseIsInControl;
#endregion

        #region Core

        public void Draw()
        {
            if (bmp != null)
                bmp.Dispose();
            bmp = new Bitmap(Width, Height);
            g = Graphics.FromImage(bmp);
            float diffV = endValue - startValue;
            
            if (HoriBar)
                diffP = Width;
            else
                diffP = Height;
            k = diffV / diffP;

            float minGap = diffV / (maxScalePN + 1);
            float maxGap = diffV / ((minScalePN - 1) < 1 ? 1 : minScalePN - 1);

            AdaptGap(minGap, maxGap, diffP);
            CalculateScaleList();
            DrawScale(10, 7, 5);
        }        

        /// <summary>
        /// 最合适10的n次方使得a处于10到1之间
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private float GoodMultiNum(float a)
        {
            float m = a;
            int n = 0;
            while (m>10|m<1)
            {
                if (m > 10)
                {
                    m /= 10.0f;
                    n--;
                }
                else
                {
                    m *= 10.0f;
                    n++;
                }
            }
                        
            return (float)Math.Pow(10,n);
        }

        /// <summary>
        /// 计算最合适的GapV和GapPixel
        /// </summary>
        /// <param name="minGap"></param>
        /// <param name="maxGap"></param>
        /// <param name="diffP"></param>
        private void AdaptGap(float minGap, float maxGap, float diffP)
        {
            float tmp = GoodMultiNum(maxGap-minGap);
            float newMinGap = minGap * tmp;
            if (newMinGap <= 0.2)
            {
                GapV = 0.2f / tmp;
                GapPixl = GapV / k;
                if (GapPixl < 30)
                {
                    GapV = 0.5f / tmp;
                    GapPixl = GapV / k;
                    if (GapPixl < 30)
                    {
                        GapV = 1.0f / tmp;
                        GapPixl = GapV / k;
                    }
                }
            }
            else
            {
                if (newMinGap <= 0.5)
                {
                    GapV = 0.5f / tmp;
                    GapPixl = GapV / k;
                    if (GapPixl < 30)
                    {
                        GapV = 1.0f / tmp;
                        GapPixl = GapV / k;
                    }
                }
                else
                {
                    GapV = 1.0f / tmp;
                    GapPixl = GapV / k;
                }
            }
            SetStringFormat();
        }

        /// <summary>
        /// 计算所有刻度的坐标
        /// </summary>
        private void CalculateScaleList()
        {
            tallInfo = new List<float>(maxScalePN * 2);
            ScalePixelTall = new List<int>(maxScalePN * 2);
            float tmp = (float)(Math.Floor(startValue / GapV) * GapV);
            int n = 1;
            while (tmp+GapV*n <= endValue)
            {
                tallInfo.Add((tmp + GapV * n));
                ScalePixelTall.Add((int)(Math.Round((tmp + GapV * n - startValue)/ k)));
                n++;
            }
            n--;

            ScalePixelMiddle = new List<int>(n * 2);
            float t0 = ScalePixelTall[0] - GapPixl * 0.5f ;
            t0 = t0 < 0 ? t0 + GapPixl : t0;
            while (t0 >= 0 & t0 <= diffP )
            {
                ScalePixelMiddle.Add((int)(Math.Round(t0)));
                t0 += GapPixl;
            }

            ScalePixelShort = new List<int>(n * 20);
            float minGapP = GapPixl * 0.1f;
            t0 = ScalePixelMiddle[0] - minGapP*4 ;
            while (t0 <= diffP)
            {
                if (t0 >= 0)
                    ScalePixelShort.Add((int)(Math.Round(t0)));
                t0 += minGapP;
            }
            DeletNearShortScale();
        }
                
        private void DrawScale(int TallLength, int MiddleLength, int ShortLength)
        {
            Pen pen = new Pen(Color.Black);
            Font font = new Font("宋体", 8, FontStyle.Regular);
            int n = ScalePixelTall.Count;

            if (HoriBar)
            {
                g.DrawLine(pen, new Point(1,Height), new Point(Width,Height));
                
                for (int i = 0; i < n; i++)
                {
                    g.DrawLine(pen, new Point(ScalePixelTall[i], Height), 
                        new Point(ScalePixelTall[i], Height - TallLength));
                    g.DrawString(string.Format(infoStringFormat, tallInfo[i]), font, 
                        new SolidBrush(Color.Black), new Point(ScalePixelTall[i], Height-TallLength*2));
                }

                foreach (int m in ScalePixelMiddle)
                    g.DrawLine(pen, new Point(m, Height), new Point(m, Height - MiddleLength));

                foreach (int m in ScalePixelShort)
                    g.DrawLine(pen, new Point(m, Height), new Point(m, Height - ShortLength));
            }
            else
            {
                g.DrawLine(pen, new Point(1, 1), new Point(1, Height));
                for (int i = 0; i < n; i++)
                {
                    int vp = Height - ScalePixelTall[i];
                    g.DrawLine(pen, new Point(1, vp), new Point(TallLength, vp));
                    g.DrawString(string.Format(infoStringFormat, tallInfo[i]), font,
                        new SolidBrush(Color.Black), new Point(1, vp));
                }

                foreach (int m in ScalePixelMiddle)
                    g.DrawLine(pen, new Point(1, Height - m), 
                        new Point(MiddleLength, Height - m));

                foreach (int m in ScalePixelShort)
                    g.DrawLine(pen, new Point(1, Height - m), 
                        new Point(ShortLength, Height - m));

            }
        }
               
        private void SetStringFormat()
        {
            int n = (int)Math.Log10(GoodMultiNum(GapV));
            if (n > 0)
                infoStringFormat = "{0:f" + n.ToString() + "}";
            if (n == 0)
                infoStringFormat = "{0:g}";
            if (n < 0)
                infoStringFormat = "{0:e" + (-n).ToString() + "}";


        }

        /// <summary>
        /// 剔除重叠刻度线
        /// </summary>
        private void DeletNearShortScale()
        {
            int n = ScalePixelShort.Count();
            int[] tmp = new int[n];
            ScalePixelShort.CopyTo(tmp);
            ScalePixelShort.Clear();

            Parallel.For(0, n, i =>
              {
                  if (!IsNearTOList(tmp[i], ScalePixelMiddle)
                  & !IsNearTOList(tmp[i], ScalePixelTall))
                      ScalePixelShort.Add(tmp[i]);
              });
        }

        private bool IsNearTOList(int a, List<int> x)
        {
            int n = x.Count();
            for (int i = 0; i < n; i++)
            {
                int y = x[i] - a;
                if (y < 2 & y > -2)
                    return true;
            }
            return false;
        }
        #endregion

        #region Control
        private void RublerBar_MouseMove(object sender, MouseEventArgs e)
        {
            Invalidate();
            Point p = new Point(e.X, e.Y);
            JudgeMouseIsInControl(p);
            
        }

        private void RublerBar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }

        private void RublerBar_MouseDown(object sender, MouseEventArgs e)
        {

            if (MouseIsInControl)
            {
                /*if (!AltIsDown)
                {
                    zoomRegion = new DrawZoomRegion();
                    zoomRegion.p0 = new Point(e.X, 0);
                }*/
            }
        }

        private void RublerBar_MouseUp(object sender, MouseEventArgs e)
        {
            /*if (zoomRegion != null & AltIsDown == false)
            {
                

            }*/
        }

        private void RublerBar_SizeChanged(object sender, EventArgs e)
        {
            
        }

        private void JudgeMouseIsInControl(Point p)
        {
            Point np = this.PointToScreen(p);
            Rectangle rc = RectangleToScreen(ClientRectangle);

            //Console.WriteLine(np.X.ToString() + " " + np.Y.ToString());
            //Console.WriteLine(rc.X.ToString() + " " + rc.Y.ToString());
            //Console.WriteLine((rc.X+rc.Width).ToString() + " " + (rc.Y+rc.Height).ToString());

            if (rc.Contains(np))
            {
                //鼠标形态改变
                Cursor = Cursors.Hand;
                MouseIsInControl = true;
            }
            else
                MouseIsInControl = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            if (bmp != null)
                graphics.DrawImage(bmp, 0, 0);
            GC.Collect();
        }
        #endregion





    }
}
