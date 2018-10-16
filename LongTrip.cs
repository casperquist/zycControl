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
    public partial class LongTrip : UserControl
    {

        public Plot2D ima;

        private bool MouseIsInControl = false;
        private DrawZoomRegion zoomRegion = null;
        public bool firstZoom;
        private float wt, ht;
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
        public string xStringFormat, ystringFormat;
        private Graphics g;

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

        private void Current_MouseMove(object sender, MouseEventArgs e)
        {
            Refresh();
            Point p = new Point(e.X, e.Y);
            JudgeMouseIsInControl(p);

            //画阴影区域
            if (zoomRegion != null)
            {
                DrawRectangle(new Point(e.X, Width));
            }
        }

        private void Current_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ima.DisplayZoneMin = new float[2] { 0, 0 };
            ima.DisplayZoneMax = new float[2] { 1, 1 };
            ima.Refresh(true);
            firstZoom = true;
        }

        private void Current_MouseDown(object sender, MouseEventArgs e)
        {

            if (MouseIsInControl)
            {
                zoomRegion = new DrawZoomRegion();
                zoomRegion.p0 = new Point(e.X, 0);
            }
        }

        private void Current_MouseUp(object sender, MouseEventArgs e)
        {
            if (zoomRegion != null)
            {
                Refresh();
                CalRealZoomRect(new Rectangle(zoomRegion.minX, zoomRegion.minY, zoomRegion.width, zoomRegion.height));
                if (zoomRegion.g != null)
                    zoomRegion.g.Dispose();
                zoomRegion = null;

            }
        }

        private void Current_SizeChanged(object sender, EventArgs e)
        {
            if (ima != null)
            {
                ima.ControlHeight = Height == 0 ? 1 : Height;
                ima.ControlWidth = Width == 0 ? 1 : Width;
                ima.Refresh(true);

            }
        }

        private void JudgeMouseIsInControl(Point p)
        {
            Point np = this.PointToScreen(p);
            Rectangle rc = RectangleToScreen(ClientRectangle);

            if (rc.Contains(np))
            {
                //鼠标形态改变
                Cursor = Cursors.Cross;
                MouseIsInControl = true;
            }
            else
                MouseIsInControl = false;
        }

        private void Current_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keys.Control & Control.ModifierKeys) == Keys.Control & MouseIsInControl)
            {
                DrawTip(PointToClient(MousePosition).X);
            }

        }

        private void Current_KeyUp(object sender, KeyEventArgs e)
        {
            if ((Control.ModifierKeys) == Keys.None)
            {
                Refresh();
            }
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

            if (zoomRegion.g == null)
                zoomRegion.g = g;
            zoomRegion.p1 = np;

            if (zoomRegion.pChanged)
            {
                Refresh();
                zoomRegion.Draw(true);
            }
        }

        /// <summary>
        /// 计算放大区域，并进行放大
        /// </summary>
        /// <param name="zoomRectangle"></param>
        private void CalRealZoomRect(Rectangle zoomRectangle)
        {
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
                firstZoom = false;
                ima.Refresh(true);
                Refresh();
            }
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
                int DrawPointSize = 2;
                g.FillEllipse(new SolidBrush(color[i]), new Rectangle(px - DrawPointSize, py[i] - DrawPointSize,
                    2 * DrawPointSize + 1, 2 * DrawPointSize + 1));
                string infos = xName + "=" + string.Format(xStringFormat, info[i][0]) + xUnit
                    + "  " + yName + "=" + string.Format(xStringFormat, info[i][1]) + yUnit;
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

        private void ResetPoint(List<Rectangle> rec, ref Point a)
        {
            if (rec.Count == 0)
                return;
            int minH = rec[0].Height;
            while (PointInRectangles(rec, a))
                a.Y += minH;
        }

        private bool PointInRectangles(List<Rectangle> rec, Point a)
        {
            bool result = false;
            for (int i = 0; i < rec.Count(); i++)
                result |= rec[i].Contains(a);
            return result;
        }

        public LongTrip()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.  
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲 
            InitializeComponent();
            transparentInfo1.Select();
        }

        /// <summary>
        /// 创建新的Image,并生成位图
        /// </summary>
        /// <param name="data">数据源</param>
        /// <param name="row">bmp的行数</param>
        /// <param name="col">bmp的列数</param>
        /// <returns></returns>
        public void NewImage(List<series> a, bool fixRange, float[] range)
        {

            transparentInfo1.Initialization();
            ima = new Plot2D();

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

            bmpShow1.bmp = ima.bmp;
            //transparentInfo1.ima = ima;

            //transparentInfo1.firstZoom = true;
            Refresh();
        }
    }
}
