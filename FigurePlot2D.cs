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
    public partial class FigurePlot2D : UserControl
    {
        private bool MouseIsInControl = false;
        private DrawZoomRegion zoomRegion = null;
        public bool AltIsDown, ControlIsDown, ShiftIsDown;
        private DataTips tips;
        private bool firstZoom;
        public Plot2D ima;
        private float wt, ht;

        public FigurePlot2D()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.  
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲  
            InitializeComponent();
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
            ima.Refresh();
            firstZoom = true;
            
            Refresh();

        }

        private void Plot2D_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            JudgeMouseIsInControl(p);

            //画阴影区域
            if (zoomRegion != null)
            {
                DrawRectangle(p);
            }
        }

        private void Plot2D_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ima.Refresh();
            //ima.inputData.ReSet();
            
            ima.DisplayZoneMin = new float[2] { 0, 0 };
            ima.DisplayZoneMax = new float[2] { 1, 1 };
            ima.Refresh();
            firstZoom = true;
        }

        private void Plot2D_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseIsInControl & AltIsDown == false)
            {
                zoomRegion = new DrawZoomRegion();
                zoomRegion.p0 = e.Location;
            }
            else
            {
                if (MouseIsInControl)
                {
                    //AddToolTip(e.X, e.Y);
                }
            }
        }

        private void Plot2D_MouseUp(object sender, MouseEventArgs e)
        {
            if (zoomRegion != null & AltIsDown == false)
            {
                if (tips != null)
                { tips.Clear(); tips = null; }
                Refresh();
                CalRealZoomRect(new Rectangle(zoomRegion.minX, zoomRegion.minY, zoomRegion.width, zoomRegion.height));
                if (zoomRegion.g != null)
                    zoomRegion.g.Dispose();
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
                ima.Refresh();

                Refresh();
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
                zoomRegion.g = CreateGraphics();
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
                firstZoom = false;
                ima.Refresh();
                Refresh();
            }
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;
            if (ima != null)
                graphics.DrawImage(ima.bmp, 0, 0);
        }

        private void ClearToolTip()
        {
            if (tips != null)
                tips.Clear();
            tips = null;
            Invalidate();
        }

        /// <summary>
        /// 显示数据游标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void AddToolTip(int x, int y)
        {
            /*ClearToolTip();
            float data;
            uint colorIndex;
            int matr = 0, matc = 0, index = 0;
            ima.GetColorIndex(x, y, ref matr, ref matc, ref index);
            colorIndex = ima.inputData.matrix[matr, matc];
            data = ima.inputData.data[index];
            tips = new DataTips();

            tips.bmp = ima.bmp;
            tips.parent = this;
            int halfPointSize = (tips.PointSize - 1) / 2;
            tips.Location = new Point(x - halfPointSize, y - halfPointSize);
            tips.DataLocation = new Point(x, y);
            tips.Add(matc, matr, data, colorIndex);*/
        }
    }
}
