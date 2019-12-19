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
    public partial class LongStrip_ : UserControl
    {
        private bool firstZoom;
        public Plot2D ima;
        private double wt, ht;
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
                longStripToolLayout1.StartX = value;
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
                longStripToolLayout1.EndX =
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
                longStripToolLayout1.StepX =
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
                longStripToolLayout1.StartY =
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
                longStripToolLayout1.EndY =
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
                longStripToolLayout1.StepY =
                value;
            }
            get { return _StepY; }
        }
        private bool ParaChanged;
        
        public LongStrip_()
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
            //ima.Refresh(true);
            firstZoom = true;
            UpdataDataAndState(-1, 0);
                Invalidate();
        }

        private void longStripToolLayout1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 32)
            {
                ima.DisplayZoneMin = new double[2] { 0, 0 };
                ima.DisplayZoneMax = new double[2] { 1, 1 };

                firstZoom = true;

                StartX = ima.x0;
                EndX = ima.x1;
                StartY = ima.y0;
                EndY = ima.y1;

                RangeChange?.Invoke(new float[] {
                    ima.x0, ima.x1, ima.y0, ima.y1});
                UpdataDataAndState(-1, 0);
                Invalidate();
            }
        }

        private void longStripToolLayout1_SizeChanged(object sender, EventArgs e)
        {
            if (ima != null)
            {
                ima.ControlHeight = Height == 0 ? 1 : Height;
                ima.ControlWidth = Width == 0 ? 1 : Width;
                UpdataDataAndState(-1, 0);
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
                    ima.DisplayZoneMin[0] = zoomRectangle.X / (float)(Width - 1);
                    ima.DisplayZoneMin[1] = zoomRectangle.Y / (float)(Height - 1);
                    ima.DisplayZoneMax[0] = (zoomRectangle.X + zoomRectangle.Width - 1) / (float)(Width - 1);
                    ima.DisplayZoneMax[1] = (zoomRectangle.Y + zoomRectangle.Height - 1) / (float)(Height - 1);
                    wt = ima.DisplayZoneMax[0] - ima.DisplayZoneMin[0];
                    ht = ima.DisplayZoneMax[1] - ima.DisplayZoneMin[1];
                }
                else
                {
                    ima.DisplayZoneMax[0] = (zoomRectangle.X + zoomRectangle.Width - 1) / (float)(Width - 1) * wt + ima.DisplayZoneMin[0];
                    ima.DisplayZoneMax[1] = (zoomRectangle.Y + zoomRectangle.Height - 1) / (float)(Height - 1) * ht + ima.DisplayZoneMin[1];
                    ima.DisplayZoneMin[0] += zoomRectangle.X / (float)(Width - 1) * wt;
                    ima.DisplayZoneMin[1] += zoomRectangle.Y / (float)(Height - 1) * ht;
                    wt = ima.DisplayZoneMax[0] - ima.DisplayZoneMin[0];
                    ht = ima.DisplayZoneMax[1] - ima.DisplayZoneMin[1];
                }

                RangeChange?.Invoke(new float[] {
                    (float)(ima.x0 + ima.xw * ima.DisplayZoneMin[0]),
                    (float)(ima.x0 + ima.xw * ima.DisplayZoneMax[0]),
                    (float)(ima.y1 - ima.yh * ima.DisplayZoneMax[1]),
                    (float)(ima.y1 - ima.yh * ima.DisplayZoneMin[1])});

                firstZoom = false;
                UpdataDataAndState(-1, 0);
                Invalidate();
            }
        }

        private void CalRealZoomRect(float[] rectangle)
        {
            CalRealZoomRect(new Rectangle((int)rectangle[4], (int)rectangle[5], (int)rectangle[6], (int)rectangle[7]));
            RangeChange?.Invoke(new float[] { rectangle[0], rectangle[1], rectangle[2], rectangle[3] });
        }

        private void EventAndRespone()
        {
            longStripToolLayout1.RangeChanged += new RangeChangedHandleEvent(CalRealZoomRect);
        }

        protected override void OnPaint(PaintEventArgs e)
        {            
            if (ima != null)
            {                
                e.Graphics.DrawImage(ima.bmp, 0, 0);
            }
            
        }

        public void UpdataDataAndState(int index, int width)
        {
            ima.Refresh(index, width);
            Invalidate();
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
                longStripToolLayout1.StartX = range[0];
                longStripToolLayout1.EndX = range[1];
            }
            else
            {
                ima.DisplayZoneMin[1] = -(range[1] - ima.y1) / ima.yh;
                ima.DisplayZoneMax[1] = -(range[0] - ima.y1) / ima.yh;
                longStripToolLayout1.StartY = range[0];
                longStripToolLayout1.EndY = range[1];
            }
            wt = ima.DisplayZoneMax[0] - ima.DisplayZoneMin[0];
            ht = ima.DisplayZoneMax[1] - ima.DisplayZoneMin[1];
            firstZoom = false;
            UpdataDataAndState(-1, 0);
            Invalidate();
        }
    }
}
