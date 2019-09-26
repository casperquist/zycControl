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
    public partial class LongStripToolLayout : UserControl
    {
        
        public ZoomRegion zoomRegion;
        public JudgeLine JudgeLine0, JudgeLine1;
        public AscanInfoShow infoShow;
        private bool AltIsDown, ControlIsDown, ShiftIsDown, SpaceIsDown;
        public event RangeChangedHandleEvent RangeChanged;
        private bool MouseIsInControl;
        private string MouseState;
        /// <summary>
        /// 当前光标对应的数据
        /// </summary>
        private double[] CurrentRefP, CurrentMeaP, CurrentGate0, CurrentGate1, CurrentGate2, CurrentGate3;
        /// <summary>
        /// 最后一次选择的光标
        /// </summary>
        private string LastMouseState;
        private bool MouseIsDoubleClick, MouseLeftIsDown;
        private float _StartX;
        /// <summary>
        /// 当前图像的起始X值
        /// </summary>
        public float StartX
        {
            set
            {
                _StartX = value;
                JudgeLine0.StartX = JudgeLine1.StartX = value;
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
                JudgeLine0.EndX = JudgeLine1.EndX = value;
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
                JudgeLine0.StepX = JudgeLine1.StepX = value;
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
                JudgeLine0.StartY = JudgeLine1.StartY = value;
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
                JudgeLine0.EndY = JudgeLine1.EndY = value;
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
                JudgeLine0.StepY = JudgeLine1.StepY = value;
            }
            get { return _StepY; }
        }
        private int mouseX;

        public LongStripToolLayout()
        {
            InitializeComponent();
            InitialJudgeLine();
            InitialInfoShow();
        }

        /// <summary>
        /// 用于计算放大区域
        /// </summary>
        /// <param name="ft"></param>
        private void InitialFigCursors(ref FigCursors ft)
        {
            ft.width = Width;
            ft.StepX = StepX;
            ft.height = Height;
            ft.StartX = StartX;
            ft.StepY = StepY;
            ft.EndX = EndX;
            ft.StartY = StartY;
            ft.EndY = EndY;
        }

        private void InitialJudgeLine()
        {
            Font font = new Font("宋体", 8);
            string format = "{0:0.00}";

            JudgeLine0 = new JudgeLine();
            JudgeLine0.color = Color.Red;
            JudgeLine0.InfoFormat = format;
            JudgeLine0.InfoFont = font;
            JudgeLine0.Name = "JudgeLine0";

            JudgeLine1 = new JudgeLine();
            JudgeLine1.color = Color.Blue;
            JudgeLine1.InfoFormat = format;
            JudgeLine1.InfoFont = font;
            JudgeLine1.Name = "JudgeLine1";

        }

        private void InitialInfoShow()
        {
            infoShow = new AscanInfoShow();
            infoShow.Enable = false;
            if (infoShow.font == null)
                infoShow.font = new Font("宋体", 8);
            if (infoShow.StrFormat == null)
                infoShow.StrFormat = "0.00";
            
        }

        public void EnableJudgeLine(ref JudgeLine fc, int Y)
        {
            fc.Enable = true;
            fc.XPixel = 0;
            fc.YPixel = Y;
        }


        private void LongSrtipToolLayout_KeyUp(object sender, KeyEventArgs e)
        {
            var m = ModifierKeys;
            if (m == Keys.None)
            {
                AltIsDown = false;
                ControlIsDown = false;
                ShiftIsDown = false;
                SpaceIsDown = false;
                Invalidate();
            }

            infoShow.Enable = false;
            Invalidate();
        }

        private void LongSrtipToolLayout_KeyDown(object sender, KeyEventArgs e)
        {
            var m = ModifierKeys;
            if (m == Keys.Alt)
                AltIsDown = true;
            if (m == Keys.Control)
            {
                ControlIsDown = true;
                infoShow.Enable = true;
                Invalidate();
            }
            if (m == Keys.Shift)
                ShiftIsDown = true;
            if (m == Keys.Space)
                SpaceIsDown = true;
        }

        private void LongSrtipToolLayout_MouseMove(object sender, MouseEventArgs e)
        {
            JudgeMouseIsInControl(e.Location);
            mouseX = e.X;
            ///判断鼠标状态，并移动光标
            

            ///画放大区域
            if (zoomRegion != null)
            {
                zoomRegion.p1 = e.Location;
                Invalidate();
            }
        }

        private void LongSrtipToolLayout_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MouseIsDoubleClick = true;
        }

        private void LongSrtipToolLayout_MouseDown(object sender, MouseEventArgs e)
        {
            ///确定鼠标状态
            if (e.Button == MouseButtons.Left)
            {
                
                LastMouseState = MouseState;
            }


            ///新建缩放区域，准备画图
            if (MouseIsInControl )
            {
                zoomRegion = new ZoomRegion();
                zoomRegion.p0 = e.Location;
            }
        }

        private void LongSrtipToolLayout_MouseUp(object sender, MouseEventArgs e)
        {
            ///重置缩放区域
            if (zoomRegion != null &&
                zoomRegion.width != 0 &&
                zoomRegion.height != 0 &&
                !MouseIsDoubleClick)
            {
                FigCursors ft = new FigCursors();
                InitialFigCursors(ref ft);
                ft.XPixel = zoomRegion.minX;
                ft.YPixel = zoomRegion.minY;
                float sx = ft.X;
                float ey = ft.Y;
                int xa = ft.XPixel;
                int ya = ft.YPixel;

                ft.XPixel = zoomRegion.minX + zoomRegion.width - 1;
                ft.YPixel = zoomRegion.minY + zoomRegion.height - 1;
                float ex = ft.X;
                float sy = ft.Y;
                int xb = ft.XPixel;
                int yb = ft.YPixel;

                /*Console.WriteLine(new Rectangle(xa, ya, xb - xa, yb - ya));
                Console.WriteLine("sx=" + sx.ToString());
                Console.WriteLine("ex=" + ex.ToString());
                Console.WriteLine("sy=" + sy.ToString());
                Console.WriteLine("ey=" + ey.ToString());*/

                if (xb > xa & yb > ya)
                {
                    StartX = sx;
                    EndX = ex;
                    StartY = sy;
                    EndY = ey;
                    RangeChanged?.Invoke(new float[] {
                        StartX,EndX,StartY,EndY,
                    zoomRegion.minX,zoomRegion.minY,zoomRegion.width,zoomRegion.height});
                }
            }
            zoomRegion = null;

            
            Invalidate();

            ///重置光标选定信息和状态信息
            MouseLeftIsDown = false;
            
            MouseState = "+";
        }

        private void LongSrtipToolLayout_SizeChanged(object sender, EventArgs e)
        {
            JudgeLine0.width = JudgeLine1.width = Width;
            JudgeLine0.height = JudgeLine1.height = Height;

        }

        private void RefreshAll(Graphics g)
        {
            JudgeLine0.Draw(g);
            JudgeLine1.Draw(g);
            if (zoomRegion != null)
                zoomRegion.DrawRegion(g, Size);
            infoShow.Draw(g, _StartX, _EndX, _StartY, _EndY, mouseX, Width, Height, _StepX);
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            RefreshAll(e.Graphics);
        }
        
    }
}
