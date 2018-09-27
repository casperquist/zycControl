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
    public partial class FigurCommentUT : UserControl
    {
        /// <summary>
        /// 数据图像
        /// </summary>
        public ImageCommentUT ima = null;
        /// <summary>
        /// 鼠标是否在控件中
        /// </summary>
        private bool MouseIsInControl = false;
        /// <summary>
        /// 画放大区域的矩形框
        /// </summary>
        private DrawZoomRegion zoomRegion = null;
        public OutputBmp outBmp = null;
        public InputImageData inputData = null;
        public bool AltIsDown, ControlIsDown, ShiftIsDown;
        /// <summary>
        /// 临时存储数据游标位置处的数据
        /// </summary>
        private DataTips tips;
        public float zeroRatio;
        private bool firstZoom;
        private float wt, ht;

        public FigurCommentUT()
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
        public void NewImage()
        {
            // 重设参数
            outBmp.width = Width;
            outBmp.height = Height;
            outBmp.ReSet();
            inputData.stepBmp = outBmp.stepBmp;
            inputData.minBmp = outBmp.minBmp;
            inputData.ReSet();

            ima = new ImageCommentUT();
            ima.inputData = inputData;
            ima.outBmp = outBmp;
            ima.ZeroColRatio = zeroRatio;
            ima.RefreshBMP();
            //pictureBox.Image = ima.bmp;
            //Invalidate();
            firstZoom = true;

        }

        private void Figure_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            JudgeMouseIsInControl(p);

            //画阴影区域
            if (zoomRegion != null)
            {
                DrawRectangle(p);
            }
        }

        private void Figure_MouseDown(object sender, MouseEventArgs e)
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
                    AddToolTip(e.X, e.Y);
                }
            }
        }

        private void Figure_MouseUp(object sender, MouseEventArgs e)
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

        private void Figure_DoubleClick(object sender, EventArgs e)
        {
            ima.inputData.ReSet();
            ima._DisplayZoneMin = new float[2] { 0, 0 };
            ima._DisplayZoneMax = new float[2] { 1, 1 };
            ima.RefreshBMP();
            firstZoom = true;
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
                    ima._DisplayZoneMin[0] = zoomRectangle.X / (float)Width;
                    ima._DisplayZoneMin[1] = zoomRectangle.Y / (float)Height;
                    ima._DisplayZoneMax[0] = (zoomRectangle.X + zoomRectangle.Width) / (float)Width;
                    ima._DisplayZoneMax[1] = (zoomRectangle.Y + zoomRectangle.Height) / (float)Height;
                    wt = ima._DisplayZoneMax[0] - ima._DisplayZoneMin[0];
                    ht = ima._DisplayZoneMax[1] - ima._DisplayZoneMin[1];
                }
                else
                {
                    ima._DisplayZoneMin[0] += zoomRectangle.X / (float)Width * wt;
                    ima._DisplayZoneMin[1] += zoomRectangle.Y / (float)Height * ht;
                    ima._DisplayZoneMax[0] = (zoomRectangle.X + zoomRectangle.Width) / (float)Width * wt + ima._DisplayZoneMin[0];
                    ima._DisplayZoneMax[1] = (zoomRectangle.Y + zoomRectangle.Height) / (float)Height * ht + ima._DisplayZoneMin[1];
                    wt = ima._DisplayZoneMax[0] - ima._DisplayZoneMin[0];
                    ht = ima._DisplayZoneMax[1] - ima._DisplayZoneMin[1];
                }
                firstZoom = false;
                ima.RefreshBMP();
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
                zoomRegion.g = CreateGraphics();
            zoomRegion.p1 = np;

            if (zoomRegion.pChanged)
            {
                Refresh();
                zoomRegion.Draw(true);
            }
        }

        /// <summary>
        /// 判断点是否在控件中
        /// </summary>
        /// <param name="p">被判断的点</param>
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

        private void Figure_KeyDown(object sender, KeyEventArgs e)
        {
            var m = ModifierKeys;
            if (m == Keys.Alt)
                AltIsDown = true;
            if (m == Keys.Control)
                ControlIsDown = true;
            if (m == Keys.Shift)
                ShiftIsDown = true;
        }

        private void Figure_KeyUp(object sender, KeyEventArgs e)
        {
            var m = ModifierKeys;
            if (m == Keys.None)
            {
                AltIsDown = false;
                ControlIsDown = false;
                ShiftIsDown = false;
            }
        }

        private void Figure_SizeChanged(object sender, EventArgs e)
        {
            ClearToolTip();
            if (ima != null)
            {
                ima.outBmp.height = Height == 0 ? 1 : Height;
                ima.outBmp.width = Width == 0 ? 1 : Width;
                ima.RefreshBMP();

                Refresh();
            }
        }

        /// <summary>
        /// 显示数据游标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void AddToolTip(int x, int y)
        {
            ClearToolTip();
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
            tips.Add(matc, matr, data, colorIndex);
        }

        private void ClearToolTip()
        {
            if (tips != null)
                tips.Clear();
            tips = null;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;
            if (ima != null)
                graphics.DrawImage(ima.bmp, 0, 0);
        }
    }
}
