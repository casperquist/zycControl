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
    public partial class AscanToolLayout : UserControl
    {
        /// <summary>
        /// Red,Blue,Green,Blcak闸门
        /// </summary>
        public Gates gate0, gate1, gate2, gate3;
        public FigCursors ReferenceC, MeasureC;
        public ZoomRegion zoomRegion;
        private bool AltIsDown, ControlIsDown, ShiftIsDown, SpaceIsDown;
        public event RangeChangedHandleEvent RangeChanged;
        private bool MouseIsInControl;
        private string MouseState;
        /// <summary>
        /// 当前光标对应的数据
        /// </summary>
        private double[] CurrentRefP, CurrentMeaP,CurrentGate0,CurrentGate1,CurrentGate2,CurrentGate3;
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
            set { _StartX = value;
                    gate0.StartX = gate1.StartX = 
                    gate2.StartX = gate3.StartX =
                    ReferenceC.StartX = MeasureC.StartX = 
                    value; }
            get { return _StartX; }
        }
        private float _EndX;
        /// <summary>
        /// 当前图像的终止X值
        /// </summary>
        public float EndX
        {
            set { _EndX = value;
                    gate0.EndX = gate1.EndX = 
                    gate2.EndX = gate3.EndX =
                    ReferenceC.EndX = MeasureC.EndX =
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
            set { _StepX = value;
                    gate0.StepX = gate1.StepX = 
                    gate2.StepX = gate3.StepX =
                    ReferenceC.StepX = MeasureC.StepX =
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
            set { _StartY = value;
                    gate0.StartY = gate1.StartY = 
                    gate2.StartY = gate3.StartY =
                    ReferenceC.StartY = MeasureC.StartY =
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
            set { _EndY = value;
                    gate0.EndY = gate1.EndY = 
                    gate2.EndY = gate3.EndY =
                    ReferenceC.EndY = MeasureC.EndY =
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
            set { _StepY = value;
                    gate0.StepY = gate1.StepY = 
                    gate2.StepY = gate3.StepY =
                    ReferenceC.StepY = MeasureC.StepY =
                    value;
            }
            get { return _StepY; }
        }
        
        public AscanToolLayout()
        {
            InitializeComponent();
            InitialGates();
            InitialCursors();
        }

        private void InitialCursors(ref FigCursors ft)
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

        private void InitialGates()
        {
            gate0 = new Gates();
            gate0.color = Color.Red;
            gate0.Name = "gate0";

            gate1 = new Gates();
            gate1.color = Color.Blue;
            gate1.Name = "gate1";

            gate2 = new Gates();
            gate2.color = Color.Green;
            gate2.Name = "gate2";

            gate3 = new Gates();
            gate3.color = Color.Black;
            gate3.Name = "gate3";
        }

        private void InitialCursors()
        {
            Font font = new Font("宋体", 8);
            string format = "{0:0.00}";

            ReferenceC = new FigCursors();
            ReferenceC.color = Color.Red;
            ReferenceC.InfoFormat = format;
            ReferenceC.InfoFont = font;
            ReferenceC.Name = "Ref";

            MeasureC = new FigCursors();
            MeasureC.color = Color.Blue;
            MeasureC.InfoFormat = format;
            MeasureC.InfoFont = font;
            MeasureC.Name = "Mea";

        }
                        
        public void EnableGates(ref Gates gate, int threshold, int start, int end)
        {
            gate.Enable = true;
            gate.ThresholdPixel = threshold;
            gate.StartPixel = start;
            gate.EndPixel = end;
        }

        public void EnableCursors(ref FigCursors fc, int X, int Y)
        {
            fc.Enable = true;
            fc.XPixel = X;
            fc.YPixel = Y;
        }
        
        private void AscanToolLayout_KeyUp(object sender, KeyEventArgs e)
        {
            var m = ModifierKeys;
            if (m == Keys.None)
            {
                AltIsDown = false;
                ControlIsDown = false;
                ShiftIsDown = false;
                SpaceIsDown = false;
            }
        }

        private void AscanToolLayout_KeyDown(object sender, KeyEventArgs e)
        {
            var m = ModifierKeys;
            if (m == Keys.Alt)
                AltIsDown = true;
            if (m == Keys.Control)
                ControlIsDown = true;
            if (m == Keys.Shift)
                ShiftIsDown = true;
            if (m == Keys.Space)
                SpaceIsDown = true;
        }

        private void AscanToolLayout_MouseMove(object sender, MouseEventArgs e)
        {
            JudgeMouseIsInControl(e.Location);

            ///判断鼠标状态，并移动光标
            if (!MouseLeftIsDown)
            {
                StateCursor(ReferenceC, e.Location);
                if (MouseState == "+")
                    StateCursor(MeasureC, e.Location);
                if (MouseState == "+")
                    StateGate(gate0, e.Location);
                if (MouseState == "+")
                    StateGate(gate1, e.Location);
                if (MouseState == "+")
                    StateGate(gate2, e.Location);
                if (MouseState == "+")
                    StateGate(gate3, e.Location);
            }
            else
            {
                MoveCursor(e.Location);
                MoveGate(e.Location);
            }

            ///画放大区域
            if (zoomRegion != null)
            {
                zoomRegion.p1 = e.Location;
                Invalidate();
            }
        }

        private void AscanToolLayout_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MouseIsDoubleClick = true;
        }

        private void AscanToolLayout_MouseDown(object sender, MouseEventArgs e)
        {
            ///确定鼠标状态
            if (e.Button == MouseButtons.Left)
            {
                MouseLeftIsDown = true;
                switch (MouseState)
                {
                    case "RefX":
                        SetAllDisable();
                        ReferenceC.SelectedX = true;
                        break;
                    case "RefY":
                        SetAllDisable();
                        ReferenceC.SelectedY = true;
                        break;
                    case "MeaX":
                        SetAllDisable();
                        MeasureC.SelectedX = true;
                        break;
                    case "MeaY":
                        SetAllDisable();
                        MeasureC.SelectedY = true;                        
                        break;                    
                }
                if (MouseState.Contains("0"))
                {
                    SetAllDisable();
                    gate0.Selected = true;
                }
                if (MouseState.Contains("1"))
                {
                    SetAllDisable();
                    gate1.Selected = true;
                }
                if (MouseState.Contains("2"))
                {
                    SetAllDisable();
                    gate2.Selected = true;
                }
                if (MouseState.Contains("3"))
                {
                    SetAllDisable();
                    gate3.Selected = true;
                }
                LastMouseState = MouseState;
            }
            

            ///新建缩放区域，准备画图
            if (MouseIsInControl && MouseState == "+")
            {
                zoomRegion = new ZoomRegion();
                zoomRegion.p0 = e.Location;
            }
        }

        private void AscanToolLayout_MouseUp(object sender, MouseEventArgs e)
        {
            ///重置缩放区域
            if (zoomRegion != null &&
                zoomRegion.width != 0 && 
                zoomRegion.height != 0 &&
                !MouseIsDoubleClick)
            {
                FigCursors ft = new FigCursors();
                InitialCursors(ref ft);
                
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

            ///1 鼠标双击时，设置光标，并获取当前光标相对位置
            ///2 重置双击状态
            if (MouseIsDoubleClick)
            {
                if (e.Button == MouseButtons.Left)
                    EnableCursors(ref ReferenceC, e.X, e.Y);
                else
                    EnableCursors(ref MeasureC, e.X, e.Y);
                MouseIsDoubleClick = false;
            }
            Invalidate();

            ///重置光标选定信息和状态信息
            MouseLeftIsDown = false;
            SetAllDisable();
            MouseState = "+";
        }
        
        private void AscanToolLayout_SizeChanged(object sender, EventArgs e)
        {
            gate0.width =
                gate1.width =
                gate2.width =
                gate3.width =
                ReferenceC.width =
                MeasureC.width =
                Width;

            gate0.height =
                gate1.height =
                gate2.height =
                gate3.height =
                ReferenceC.height =
                MeasureC.height =
                Height;
        }

        private void RefreshAll(Graphics g)
        {
            gate0.Draw(g);
            gate1.Draw(g);
            gate2.Draw(g);
            gate3.Draw(g);
            ReferenceC.Draw(g);
            MeasureC.Draw(g);
            if (zoomRegion != null)
                zoomRegion.DrawRegion(g, Size);
        }

        private void SetAllDisable()
        {
            gate0.Selected = false;
            gate1.Selected = false;
            gate2.Selected = false;
            gate3.Selected = false;
            ReferenceC.Selected = false;
            MeasureC.Selected = false;
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

        private void StateCursor(FigCursors csor, Point p)
        {
            string tmp;
            tmp = csor.MouseState(p);
            MouseState = csor.Name + tmp;
            if (tmp == "X")
                Cursor = Cursors.VSplit;
            if (tmp == "Y")
                Cursor = Cursors.HSplit;
            if (tmp == null)
                MouseState = "+";
        }

        private void StateGate(Gates g, Point p)
        {
            string tmp;
            tmp = g.MouseState(p);
            MouseState = g.Name + tmp;
            if (tmp == "mid")
                Cursor = Cursors.NoMove2D;
            if (tmp == "start" || tmp == "end")
                Cursor = Cursors.NoMoveHoriz;
            if (tmp == null)
                MouseState = "+";

        }

        private void MoveCursor(Point p)
        {
            if (p.X > Width - 1)
                p.X = Width - 1;
            if (p.X < 0)
                p.X = 0;
            if (p.Y > Height - 1)
                p.Y = Height - 1;
            if (p.Y < 0)
                p.Y = 0;

            if (ReferenceC.SelectedX)
                ReferenceC.XPixel = p.X;
            if (ReferenceC.SelectedY)
                ReferenceC.YPixel = p.Y;

            if (MeasureC.SelectedX)
                MeasureC.XPixel = p.X;
            if (MeasureC.SelectedY)
                MeasureC.YPixel = p.Y;
            
            Invalidate();

        }

        private void MoveGate(Point p)
        {
            if (p.X > Width - 1)
                p.X = Width - 1;
            if (p.X < 0)
                p.X = 0;
            if (p.Y > Height - 1)
                p.Y = Height - 1;
            if (p.Y < 0)
                p.Y = 0;

            if (gate0.Selected)
                RefreshGate(ref gate0, p);
            if (gate1.Selected)
                RefreshGate(ref gate1, p);
            if (gate2.Selected)
                RefreshGate(ref gate2, p);
            if (gate3.Selected)
                RefreshGate(ref gate3, p);
            /*Console.WriteLine(
                gate0.Threshold.ToString() + " " + 
                gate0.Start.ToString() + " " +
                gate0.End.ToString() + " " );*/
            
            Invalidate();
        }

        private void RefreshGate(ref Gates gate, Point p)
        {
            if (MouseState.Contains("mid"))
            {
                int width = (gate.EndPixel - gate.StartPixel) / 2;
                gate.StartPixel = p.X - width;
                gate.EndPixel = p.X + width;
                gate.ThresholdPixel = p.Y;
            }
            if (MouseState.Contains("start"))
                gate.StartPixel = p.X;
            if (MouseState.Contains("end"))
                gate.EndPixel = p.X;
        }

        /// <summary>
        /// 按照起始点和步进点设置合适的数据点
        /// </summary>
        /// <param name="a"></param>
        /// <param name="step"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public float RefreshPerData(float a, float step, float c)
        {
            return (float)(Math.Round((c - a) / step) * step + a);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            RefreshAll(e.Graphics);
        }
    }
}
