﻿using System;
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
        private DrawZoomRegion zoomRegion;
        public OutputBmp outBmp = null;
        public InputImageData inputData = null;
        public bool AltIsDown, ControlIsDown, ShiftIsDown;
        public float zeroRatio;
        private bool firstZoom;
        private float wt, ht;
               

        public FigurCommentUT()
        {
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
            
            if (zoomRegion!= null)
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
            
        }

        private void Figure_MouseUp(object sender, MouseEventArgs e)
        {
            if (zoomRegion!= null & AltIsDown == false)
            {
                CalRealZoomRect(
                    new Rectangle(zoomRegion.minX, zoomRegion.minY,zoomRegion.width,zoomRegion.height));
                                
            }
            
            zoomRegion = null;
        }

        private void Figure_DoubleClick(object sender, EventArgs e)
        {
            ima.inputData.ReSet();
            ima.DisplayZoneMin = new float[2] { 0, 0 };
            ima.DisplayZoneMax = new float[2] { 1, 1 };
            ima.RefreshBMP();
            firstZoom = true;
            Invalidate();
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
                    ima.DisplayZoneMin[0] += zoomRectangle.X / (float)Width * wt;
                    ima.DisplayZoneMin[1] += zoomRectangle.Y / (float)Height * ht;
                    ima.DisplayZoneMax[0] = (zoomRectangle.X + zoomRectangle.Width) / (float)Width * wt + ima.DisplayZoneMin[0];
                    ima.DisplayZoneMax[1] = (zoomRectangle.Y + zoomRectangle.Height) / (float)Height * ht + ima.DisplayZoneMin[1];
                    wt = ima.DisplayZoneMax[0] - ima.DisplayZoneMin[0];
                    ht = ima.DisplayZoneMax[1] - ima.DisplayZoneMin[1];
                }
                firstZoom = false;
                ima.RefreshBMP();
                Invalidate();
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

            zoomRegion.p1 = np;
            zoomRegion.ReSet();
            if (zoomRegion.width < 5)
            {
                zoomRegion.minX = 0;
                zoomRegion.width = Width;
            }
            if (zoomRegion.height < 5)
            {
                zoomRegion.minY = 0;
                zoomRegion.height = Height;
            }
            pictureBox1.Invalidate();
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

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            if (zoomRegion != null)
            {
                SolidBrush b = new SolidBrush(Color.FromArgb(96, Color.Gray));
                Rectangle r = new Rectangle(zoomRegion.minX, 
                    zoomRegion.minY, 
                    zoomRegion.width, 
                    zoomRegion.height);
                if (zoomRegion.height!=0 && zoomRegion.width!=0)
                    e.Graphics.FillRectangle(b, r);
            }

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
            if (ima != null)
            {
                ima.outBmp.height = Height == 0 ? 1 : Height;
                ima.outBmp.width = Width == 0 ? 1 : Width;
                ima.RefreshBMP();

                Invalidate();
            }
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
