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
    public partial class UltravisionField : UserControl
    {        
        public UltravisionField()
        {
            InitializeComponent();
            EventAndRespone();            
        }

        /// <summary>
        /// 数据图像
        /// </summary>
        public ImageCommentUT ima = null;
        public OutputBmp outBmp = null;
        public InputImageData inputData = null;        
        public float zeroRatio;
        public event RangeChangedHandleEvent RangeChanged;
        private bool firstZoom;
        /// <summary>
        /// 图像显示区域的width和height，0~1
        /// </summary>
        private float wt = 0, ht = 0;
        /// <summary>
        /// 当前数据的起末点信息
        /// </summary>
        public double startX { set { x0 = value; } get { return x0; } }
        public double endX { set { x1 = value; } get { return x1; } }
        public double startY { set { y0 = value; } get { return y0; } }
        public double endY { set { y1 = value; } get { return y1; } }
        public double xGap, yGap;
        private double x0, y0, x1, y1;

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
            SetToolLayoutPara(true);
            firstZoom = true;

        }

        /// <summary>
        /// 计算放大区域，并进行放大
        /// </summary>
        /// <param name="zoomRectangle"></param>
        private void CalRealZoomRect(Rectangle zoomRectangle)
        {
            if (zoomRectangle.Width > 0 & zoomRectangle.Height > 0)
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

                //SetToolLayoutPara(false);
                firstZoom = false;
                ima.RefreshBMP();
                Invalidate();
            }
        }

        private void CalRealZoomRect(float[] rectangle)
        {
            
           CalRealZoomRect(new Rectangle((int)rectangle[0], (int)rectangle[1], (int)rectangle[2], (int)rectangle[3]));
        }

        private void ultraToolLayout1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 32)
            {
                ResetAll();
            }
        }

        private void ResetAll()
        {
            ima.inputData.ReSet();
            ima.DisplayZoneMin = new float[2] { 0, 0 };
            ima.DisplayZoneMax = new float[2] { 1, 1 };
            ima.RefreshBMP();
            SetToolLayoutPara(true);
            Invalidate();
            firstZoom = true;
            wt = ht = 0;
            RangeChanged?.Invoke(new float[] {0,0,0,0,
                    (float)x0, (float)x1, (float)y0, (float)y1 });
        }

        private void ultraToolLayout1_SizeChanged(object sender, EventArgs e)
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
                
        private void EventAndRespone()
        {            
            ultraToolLayout1.RangeChanged += new RangeChangedHandleEvent(CalRealZoomRect);
        }

        private void SetToolLayoutPara(bool initail)
        {
            if (initail)
            {
                ultraToolLayout1.startX = x0;
                ultraToolLayout1.endX = x1;
                ultraToolLayout1.startY = y0;
                ultraToolLayout1.endY = y1;
                ultraToolLayout1.xGap = xGap;
                ultraToolLayout1.yGap = yGap;
                ultraToolLayout1.DrawBeamLine = false;
                
            }
            else
            {
                           
            }
        }
    }
}
