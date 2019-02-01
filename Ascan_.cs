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
    public partial class Ascan_ : UserControl
    {
        private bool firstZoom;
        public Plot2D ima;
        private float wt, ht;
        public event RangeChangedHandleEvent RangeChange;
        private float _StartX;
        /// <summary>
        /// 当前图像的起始X值
        /// </summary>
        public float StartX
        {
            set
            {
                _StartX = value;
                ascanToolLayout1.StartX = value;
            }
            get { return _StartX; }
        }
        private float _EndX;
        /// <summary>
        /// 当前图像的终止X值
        /// </summary>
        public float EndX
        {
            set
            {
                _EndX = value;
                ascanToolLayout1.EndX =
                value;
            }
            get { return _EndX; }
        }
        private float _StepX;
        /// <summary>
        /// 当前图像的X轴间隔值
        /// </summary>
        public float StepX
        {
            set
            {
                _StepX = value;
                ascanToolLayout1.StepX =
                value;
            }
            get { return _StepX; }
        }
        private float _StartY;
        /// <summary>
        /// 当前图像的起始Y值
        /// </summary>
        public float StartY
        {
            set
            {
                _StartY = value;
                ascanToolLayout1.StartY =
                value;
            }
            get { return _StartY; }
        }
        private float _EndY;
        /// <summary>
        /// 当前图像的终止Y值
        /// </summary>
        public float EndY
        {
            set
            {
                _EndY = value;
                ascanToolLayout1.EndY =
                value;
            }
            get { return _EndY; }
        }
        private float _StepY;
        /// <summary>
        /// 当前图像的Y轴间隔值
        /// </summary>
        public float StepY
        {
            set
            {
                _StepY = value;
                ascanToolLayout1.StepY =
                value;
            }
            get { return _StepY; }
        }

        public Ascan_()
        {
            InitializeComponent();
            
            EventAndRespone();
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
            ima.Refresh(true);
            firstZoom = true;
            Invalidate();
        }

        private void ascanToolLayout1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 32)
            {
                ima.DisplayZoneMin = new float[2] { 0, 0 };
                ima.DisplayZoneMax = new float[2] { 1, 1 };
                
                firstZoom = true;

                StartX = ima.x0;
                EndX = ima.x1;
                StartY = ima.y0;
                EndY = ima.y1;

                RangeChange?.Invoke(new float[] {
                    0, 0, 0, 0,
                    ima.x0, ima.x1, ima.y0, ima.y1});
                Invalidate();
            }
        }

        private void ascanToolLayout1_SizeChanged(object sender, EventArgs e)
        {
            if (ima != null)
            {
                ima.ControlHeight = Height == 0 ? 1 : Height;
                ima.ControlWidth = Width == 0 ? 1 : Width;
                ima.Refresh(true);

                Invalidate();
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

                RangeChange?.Invoke(new float[] {
                    ima.x0 + ima.xw * ima.DisplayZoneMin[0],
                    ima.x0 + ima.xw * ima.DisplayZoneMax[0],
                    ima.y1 - ima.yh * ima.DisplayZoneMax[1],
                    ima.y1 - ima.yh * ima.DisplayZoneMin[1]});

                firstZoom = false;
                ima.Refresh(true);
                Invalidate();
            }
        }

        private void CalRealZoomRect(float[] rectangle)
        {
            CalRealZoomRect(new Rectangle((int)rectangle[0], (int)rectangle[1], (int)rectangle[2], (int)rectangle[3]));
        }

        private void EventAndRespone()
        {
            ascanToolLayout1.RangeChanged += new RangeChangedHandleEvent(CalRealZoomRect);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (ima != null)
            {
                ima.Refresh(true);
                e.Graphics.DrawImage(ima.bmp, 0, 0);
            }
        }
    }
}
