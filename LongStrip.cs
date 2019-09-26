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
    public delegate void RangeChangedHandleEvent(float[] range);
    public partial class LongStrip : UserControl
    {
        public LongStrip()
        {            
            InitializeComponent();
        }

        private bool MouseIsInControl = false;
        private DrawZoomRegion zoomRegion = null;
        public bool AltIsDown, ControlIsDown, ShiftIsDown;
        private DataTips tips;
        private bool firstZoom;
        public Plot2D ima;
        private double wt, ht;
        /// <summary>
        /// 输出信息的字体
        /// </summary>
        public Font infoFont;
        /// <summary>
        /// 输出信息的x轴，y轴名称和单位
        /// </summary>
        public string xName, yName, xUnit, yUnit;
        /// <summary>
        /// 输出信息的数据格式
        /// </summary>
        public string xStringFormat, yStringFormat;
        private Graphics g ;
        /// <summary>
        /// 是否处于输出信息状态
        /// </summary>
        private bool DrawInfoTip;
        private Point p0, p1;

        private bool _JudgeLine0Enable;
        public bool JudgeLine0Enable { get { return _JudgeLine0Enable; } set { _JudgeLine0Enable = value; } }

        private bool _JudgeLine1Enable;
        public bool JudgeLine1Enable { get { return _JudgeLine1Enable; } set { _JudgeLine1Enable = value; } }

        private bool _JudgeLine2Enable;
        public bool JudgeLine2Enable { get { return _JudgeLine2Enable; } set { _JudgeLine2Enable = value; } }

        private float _JudgeLine0;
        public float JudgeLine0 { get { return _JudgeLine0; } set { _JudgeLine0 = value; } }

        private float _JudgeLine1;
        public float JudgeLine1 { get { return _JudgeLine1; } set { _JudgeLine1 = value; } }

        private float _JudgeLine2;
        public float JudgeLine2 { get { return _JudgeLine2; } set { _JudgeLine2 = value; } }
                
        public event RangeChangedHandleEvent RangeChange;
        
        /// <summary>
        /// 创建新的Image,并生成位图
        /// </summary>
        /// <param name="data">数据源</param>
        /// <param name="row">bmp的行数</param>
        /// <param name="col">bmp的列数</param>
        /// <returns></returns>
        public void NewImage(List<series> a, bool fixRange, float[] range)
        {
            if (g == null)
                g = CreateGraphics();

            Initialization();

            // 重设参数
            ima = new Plot2D();
            ima.rawData = a;
            ima.ControlWidth = Width;
            ima.ControlHeight = Height;
            ima.FixRange = fixRange;
            ima.x0 = range[0];
            ima.x1 = range[1];
            ima.y0 = range[2];
            ima.y1 = range[3];
            ima.Refresh(true);
            
            firstZoom = true;
            Invalidate();
        }

        private void Initialization()
        {
            if (infoFont == null)
                infoFont = new Font("宋体", 15, FontStyle.Bold);
            if (xName == null)
                xName = "X";
            if (yName == null)
                yName = "Y";
            if (xUnit == null)
                xUnit = "";
            if (yUnit == null)
                yUnit = "";
            if (xStringFormat == null)
                xStringFormat = "{0:0.0}";
            if (yStringFormat == null)
                yStringFormat = "{0:0.00}";
        }

        private void Plot2D_MouseMove(object sender, MouseEventArgs e)
        {
            if (DrawInfoTip)
                Refresh();
            Point p = new Point(e.X, e.Y);
            JudgeMouseIsInControl(p);

            //画阴影区域
            if (zoomRegion != null)
            {
                p1 = e.Location;
                if (Math.Abs(p1.X-p0.X) < 20)
                {
                    zoomRegion.p0 = new Point(0, p0.Y);
                    DrawRectangle(new Point(Width, p1.Y));
                }
                else
                {
                    if (Math.Abs(p1.Y - p0.Y) < 20)
                    {
                        zoomRegion.p0 = new Point(p0.X, 0);
                        DrawRectangle(new Point(p1.X, Height));
                    }
                    else
                    {
                        zoomRegion.p0 = p0;
                        DrawRectangle(p1);
                    }
                }
            }
        }

        private void Plot2D_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ima.Refresh(true);
            //ima.inputData.ReSet();

            ima.DisplayZoneMin = new double[2] { 0, 0 };
            ima.DisplayZoneMax = new double[2] { 1, 1 };
            ima.Refresh(true);
            firstZoom = true;

            RangeChange?.Invoke(new float[] { ima.x0, ima.x1, ima.y0, ima.y1 });
        }

        private void Plot2D_MouseDown(object sender, MouseEventArgs e)
        {
            
            if (MouseIsInControl)
            {
                if (!AltIsDown)
                {
                    p0 = e.Location;
                    zoomRegion = new DrawZoomRegion();
                    zoomRegion.p0 = p0;
                }
            }
        }

        private void Plot2D_MouseUp(object sender, MouseEventArgs e)
        {
            if (zoomRegion != null & AltIsDown == false)
            {
                if (tips != null)
                { tips.Clear(); tips = null; }
                Invalidate();
                CalRealZoomRect(new Rectangle(zoomRegion.minX, zoomRegion.minY, zoomRegion.width, zoomRegion.height));
                
                zoomRegion = null;

            }
        }

        private void Plot2D_SizeChanged(object sender, EventArgs e)
        {
            ClearToolTip();
            if (ima != null)
            {
                ima.ControlHeight = Height == 0 ? 1 : Height;
                ima.ControlWidth = Width == 0 ? 1 : Width;
                ima.Refresh(true);

                Invalidate();
            }
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
                Cursor = Cursors.Cross;
                MouseIsInControl = true;

                
            }
            else
                MouseIsInControl = false;
        }

        /// <summary>
        /// 画矩形区域
        /// </summary>
        /// <param name="p">矩形区域的末点怕p1</param>
        private void DrawRectangle(Point p)
        {
            Point np = new Point();
            np = p;
            if (p.X >= Width)
                np.X = Width - 1;
            if (p.X < 0)
                np.X = 0;
            if (p.Y >= Height)
                np.Y = Height - 1;
            if (p.Y < 0)
                np.Y = 0;

            
            zoomRegion.p1 = np;

            if (zoomRegion.pChanged)
            {
                Refresh();
                //zoomRegion.Draw(true);
            }
        }

        /// <summary>
        /// 计算放大区域，并进行放大
        /// </summary>
        /// <param name="zoomRectangle"></param>
        private void CalRealZoomRect(Rectangle zoomRectangle)
        {
            ClearToolTip();
            if (zoomRectangle.Width != 0 & zoomRectangle.Height != 0)
            {

                if (firstZoom)
                {
                    ima.DisplayZoneMin[0] = zoomRectangle.X / (float)Width;
                    ima.DisplayZoneMin[1] = zoomRectangle.Y / (float)Height;
                    ima.DisplayZoneMax[0] = (zoomRectangle.X + zoomRectangle.Width) / (float)Width;
                    ima.DisplayZoneMax[1] = (zoomRectangle.Y + zoomRectangle.Height) / (float)Height;
                    wt = ima.DisplayZoneMax[0] - ima.DisplayZoneMin[0];
                    ht = ima.DisplayZoneMax[1] - ima.DisplayZoneMin[1];
                }
                else
                {
                    ima.DisplayZoneMax[0] = (zoomRectangle.X + zoomRectangle.Width) / (float)Width * wt + ima.DisplayZoneMin[0];
                    ima.DisplayZoneMax[1] = (zoomRectangle.Y + zoomRectangle.Height) / (float)Height * ht + ima.DisplayZoneMin[1];
                    ima.DisplayZoneMin[0] += zoomRectangle.X / (float)Width * wt;
                    ima.DisplayZoneMin[1] += zoomRectangle.Y / (float)Height * ht;
                    wt = ima.DisplayZoneMax[0] - ima.DisplayZoneMin[0];
                    ht = ima.DisplayZoneMax[1] - ima.DisplayZoneMin[1];
                }

                RangeChange?.Invoke(new float[] {
                    (float)(ima.x0 + ima.xw * ima.DisplayZoneMin[0]),
                    (float)(ima.x0 + ima.xw * ima.DisplayZoneMax[0]),
                    (float)(ima.y1 - ima.yh * ima.DisplayZoneMax[1]),
                    (float)(ima.y1 - ima.yh * ima.DisplayZoneMin[1])});

                firstZoom = false;
                ima.Refresh(true);
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            if (ima != null)
                graphics.DrawImage(ima.bmp, 0, 0);
            else
                return;
            DrawJudgeLines(graphics);
            //GC.Collect();
        }

        private void ClearToolTip()
        {
            if (tips != null)
                tips.Clear();
            tips = null;
            Invalidate();
        }
        
        private void LongStrip_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keys.Control & Control.ModifierKeys) == Keys.Control & MouseIsInControl)
            {
                DrawTip(PointToClient(MousePosition).X);
                DrawInfoTip = true;
            }
            else
                DrawInfoTip = false;
            
        }

        private void LongStrip_KeyUp(object sender, KeyEventArgs e)
        {
            if ((Control.ModifierKeys) == Keys.None )
            {
                Invalidate();
            }
        }

        /// <summary>
        /// 画单根预警线
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="value"></param>
        /// <param name="pen"></param>
        /// <param name="g"></param>
        private void DrawJudgeLine(bool enable, float value, Pen pen, Graphics g)
        {
            if (enable)
            {
                double yrange = ima.yh * (ima.DisplayZoneMax[1] - ima.DisplayZoneMin[1]);
                double ystart = ima.yh * (1 - ima.DisplayZoneMax[1]) + ima.y0;
                int y = (int)(((-value + ystart) / yrange + 1) * ima.ControlHeight);
                if (y >= 0 & y <= ima.ControlHeight)
                {
                    g.DrawLine(pen, new Point(0, y), new Point(ima.ControlWidth, y));
                    g.DrawString(string.Format(yStringFormat, value), infoFont, 
                        new SolidBrush(Color.Red), new Point(0, y));
                }
            }
        }

        /// <summary>
        /// 画全部预警线
        /// </summary>
        /// <param name="g"></param>
        private void DrawJudgeLines(Graphics g)
        {
            Pen pen = new Pen(Color.Red);
            DrawJudgeLine(_JudgeLine0Enable, _JudgeLine0, pen, g);
            DrawJudgeLine(_JudgeLine1Enable, _JudgeLine1, pen, g);
            DrawJudgeLine(_JudgeLine2Enable, _JudgeLine2, pen, g);
        }

        /// <summary>
        /// 画数据提示tip，包含竖线，圆点和信息
        /// </summary>
        /// <param name="px">鼠标位置</param>
        private void DrawTip(int px)
        {
            Graphics g = this.CreateGraphics();
            int pxl_x;
            List<int> pxl_y;
            List<float[]> info = ima.PixelToData(px, out pxl_x, out pxl_y);
            g.DrawLine(new Pen(Color.Red), pxl_x, 0, pxl_x, Width);
            
            List<Color> color = new List<Color>(ima.seriesNum);
            for (int i = 0; i < ima.seriesNum; i++)
                color.Add(ima.rawData[i].sColor);
            DrawInfoPoint(pxl_x, pxl_y, info, g, color);
        }

        /// <summary>
        /// 画数据信息
        /// </summary>
        /// <param name="px">像素x坐标</param>
        /// <param name="py">像素y坐标</param>
        /// <param name="info">信息集合</param>
        /// <param name="g"></param>
        /// <param name="color">颜色集合</param>
        private void DrawInfoPoint(int px, List<int> py, List<float[]> info, Graphics g, List<Color> color)
        {
            int sn = py.Count();
            List<Rectangle> drawed = new List<Rectangle>(sn);
          
            for (int i = 0; i < sn; i++)
            {
                if (info[i] != null)
                {
                    int DrawPointSize = 2;
                    g.FillEllipse(new SolidBrush(color[i]), new Rectangle(px - DrawPointSize, py[i] - DrawPointSize,
                        2 * DrawPointSize + 1, 2 * DrawPointSize + 1));
                    string infos = xName + "=" + string.Format(xStringFormat, info[i][0]) + xUnit
                        + "  " + yName + "=" + string.Format(yStringFormat, info[i][1]) + yUnit;
                    Point point = new Point(px + DrawPointSize, py[i] + DrawPointSize);
                    SizeF sizeF = g.MeasureString(infos, infoFont);
                    Rectangle rectangle = new Rectangle(point.X, point.Y, (int)sizeF.Width + DrawPointSize, (int)sizeF.Height + DrawPointSize);
                    ///使数据信息在显示范围内
                    if (point.X + rectangle.Width > Width)
                        point.X -= rectangle.Width;
                    if (point.Y + rectangle.Height > Height)
                        point.Y -= rectangle.Height;
                    ResetPoint(drawed, ref point);
                    g.DrawString(infos, infoFont, new SolidBrush(color[i]), point);
                    rectangle = new Rectangle(point.X, point.Y, (int)sizeF.Width + DrawPointSize, (int)sizeF.Height + DrawPointSize);
                    drawed.Add(rectangle);
                }
            }
        }

        private void ResetPoint(List<Rectangle> rec, ref Point a)
        {
            if (rec.Count == 0)
                return ;
            int minH = rec[0].Height;
            while (PointInRectangles(rec, a))
                a.Y += minH;
        }

        private bool PointInRectangles (List<Rectangle> rec, Point a)
        {
            bool result = false;
            for (int i = 0; i < rec.Count(); i++)
                result |= rec[i].Contains(a);
            return result;
        }

        /// <summary>
        /// 对显示范围变化的响应
        /// </summary>
        /// <param name="range">RulerBar.StartV,RulerBar.EndV，0为H，1为v</param>
        public void ResponeEvent(float[] range)
        {
            if (range[2] < 0.1)///HoriBar
            {
                ima.DisplayZoneMin[0] = (range[0] - ima.x0) / ima.xw;
                ima.DisplayZoneMax[0] = (range[1] - ima.x0) / ima.xw;
            }
            else
            {
                ima.DisplayZoneMin[1] = -(range[1] - ima.y1) / ima.yh;
                ima.DisplayZoneMax[1] = -(range[0] - ima.y1) / ima.yh;
            }
            wt = ima.DisplayZoneMax[0] - ima.DisplayZoneMin[0];
            ht = ima.DisplayZoneMax[1] - ima.DisplayZoneMin[1];
            firstZoom = false;
            ima.Refresh(true);
            Invalidate();
        }
    }
}
