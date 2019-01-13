using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZYCControl
{
    public partial class UltraToolLayout : UserControl
    {
        public ZoomRegion zoomRegion;
        public FigCursor RefCursor, MeaCursor;
        public bool DrawBeamLine;
        public double startX { set { _sx = value; ResetCursorData(); } get { return _sx; } }
        public double endX { set { _ex = value; ResetCursorData(); } get { return _ex; } }
        public double startY { set { _sy = value; ResetCursorData(); } get { return _sy; } }
        public double endY { set { _ey = value; ResetCursorData(); } get { return _ey; } }
        public int xNum { set {_xNum = value; xGap = (_ex-_sx)/(_xNum - 1); } get { return _xNum; } }
        public int yNum { set { _yNum = value; yGap = (_sy - _ey) / (_yNum - 1); } get { return _yNum; } }

        private bool MouseIsInControl;
        private bool AltIsDown, ControlIsDown, ShiftIsDown, SpaceIsDown;
        private bool MouseIsDoubleClick, MouseLeftIsDown;  
        /// <summary>
        /// 对应startX，endX, StartY,endY
        /// </summary>
        private double _sx, _ex, _sy, _ey;      
        /// <summary>
        /// 
        /// </summary>
        private string MouseState;
        /// <summary>
        /// 当前光标对应的数据
        /// </summary>
        private double[] CurrentRefP, CurrentMeaP;
        /// <summary>
        /// 最后一次选择的光标
        /// </summary>
        private string LastSelectedCursor;
        /// <summary>
        /// 像素点间距对应的数据差（像素点K值）
        /// </summary>
        private double kx, ky;
        /// <summary>
        /// 轴上的数据数量（端点数）
        /// </summary>
        private int _xNum,_yNum;
        /// <summary>
        /// 数据点对应的间距
        /// </summary>
        private double xGap, yGap;
        
        public UltraToolLayout()
        {
            InitializeComponent();
        }

        private void InitalCursor(string CursorName)
        {
            Font font = new Font("宋体", 8);
            string format = "{0:0.00}";
            double ang = 45;
            if (CursorName == "Ref")
            {                
                RefCursor = new FigCursor();                
                RefCursor.color = Color.Red;
                RefCursor.InfoFont = font;
                RefCursor.InfoFormat = format;
                RefCursor.p = new Point(0, 0);
                RefCursor.angle = ang;
                RefCursor.beamPx = 45;
                RefCursor.Name = "Ref";
            }
            if (CursorName == "Mea")
            {
                MeaCursor = new FigCursor();
                MeaCursor.color = Color.Blue;
                MeaCursor.InfoFont = font;
                MeaCursor.InfoFormat = format;
                MeaCursor.p = new Point(0, 0);
                MeaCursor.angle = ang;
                MeaCursor.beamPx = 90;
                MeaCursor.Name = "Mea";
            }
            MouseState = "+";
        }

        private void InitalCursor(string CursorName, Point p)
        {
            InitalCursor(CursorName);
            if (CursorName == "Ref")
                RefCursor.p = p;
            if (CursorName == "Mea")
                MeaCursor.p = p;
        }

        private void UltraToolLayout_KeyDown(object sender, KeyEventArgs e)
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
        
        private void UltraToolLayout_KeyUp(object sender, KeyEventArgs e)
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

        private void UltraToolLayout_MouseDown(object sender, MouseEventArgs e)
        {
            ///确定鼠标状态
            if (e.Button == MouseButtons.Left)
            {
                MouseLeftIsDown = true;
                switch (MouseState)
                {
                    case "RefX":
                        RefCursor.xSelected = true;
                        if (MeaCursor != null) MeaCursor.Selected = false;
                        break;
                    case "RefY":                        
                        RefCursor.ySelected = true;
                        if (MeaCursor != null) MeaCursor.Selected = false;
                        break;
                    case "MeaX":
                        MeaCursor.xSelected = true;
                        if (RefCursor != null) RefCursor.Selected = false;
                        break;
                    case "MeaY":
                        MeaCursor.ySelected = true;
                        if (RefCursor != null) RefCursor.Selected = false;
                        break;
                }
                LastSelectedCursor = MouseState;
            }

            ///新建缩放区域，准备画图
            if (MouseIsInControl && MouseState == "+")
            {
                zoomRegion = new ZoomRegion();
                zoomRegion.p0 = e.Location;
            }
            
        }
        
        private void UltraToolLayout_MouseUp(object sender, MouseEventArgs e)
        {
            ///重置缩放区域
            zoomRegion = null;

            ///1 鼠标双击时，设置光标，并获取当前光标相对位置
            ///2 重置双击状态
            if (MouseIsDoubleClick)
            {
                if (e.Button == MouseButtons.Left)
                {
                    InitalCursor("Ref", e.Location);
                    CurrentRefP = new double[2] { e.X * kx, e.Y * ky };
                }
                else
                {
                    InitalCursor("Mea", e.Location);
                    CurrentMeaP = new double[2] { e.X * kx, e.Y * ky};
                }
                ResetCursorData();
                MouseIsDoubleClick = false;
            }
            Invalidate();

            ///重置光标选定信息和状态信息
            MouseLeftIsDown = false;
            if (RefCursor != null)
                RefCursor.Selected = false;
            if (MeaCursor != null)
                MeaCursor.Selected = false;
            MouseState = "+";
        }

        private void UltraToolLayout_MouseMove(object sender, MouseEventArgs e)
        {
            JudgeMouseIsInControl(e.Location);

            ///判断鼠标状态，并移动光标
            if (!MouseLeftIsDown)
            {
                StateCursor(RefCursor, e.Location);
                if (MouseState == "+")
                    StateCursor(MeaCursor, e.Location);
            }
            else
                MoveCursor(e.Location);

            ///画放大区域
            if (zoomRegion != null)
            {
                zoomRegion.p1 = e.Location;
                Invalidate();
                
            }
        }

        private void UltraToolLayout_SizeChanged(object sender, EventArgs e)
        {
            ResetCursorPos(RefCursor, CurrentRefP);
            ResetCursorPos(MeaCursor, CurrentMeaP);
        }

        private void UltraToolLayout_DoubleClick(object sender, EventArgs e)
        {
            MouseIsDoubleClick = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Refresh(e.Graphics);
        }

        public void Refresh(Graphics g)
        {
            if (zoomRegion != null)
                zoomRegion.DrawRegion(g, Size);
            if (RefCursor != null)
                RefCursor.DrawCursor(g, Size, DrawBeamLine);
            if (MeaCursor != null)
                MeaCursor.DrawCursor(g, Size, DrawBeamLine);
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

        private void StateCursor (FigCursor csor, Point p)
        {
            string tmp;
            if (csor != null)
            {
                tmp = csor.MouseState(p);
                
                if (tmp == "X")
                {
                    Cursor = Cursors.VSplit;
                    MouseState = csor.Name + "X";
                }
                if (tmp == "Y")
                {
                    Cursor = Cursors.HSplit;
                    MouseState = csor.Name + "Y";
                }
                if (tmp == null)
                    MouseState = "+";
            }
            
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
            if (RefCursor != null)
            {
                if (RefCursor.xSelected)
                    RefCursor.p.X = p.X;
                if (RefCursor.ySelected)
                    RefCursor.p.Y = p.Y;
                Invalidate();
                CurrentRefP = new double[2] { p.X * kx, p.Y * ky };
            }
            if (MeaCursor != null)
            {
                if (MeaCursor.xSelected)
                    MeaCursor.p.X = p.X;
                if (MeaCursor.ySelected)
                    MeaCursor.p.Y = p.Y;
                Invalidate();
                CurrentMeaP = new double[2] { p.X * kx, p.Y * ky };
            }
            ResetCursorData();
                
        }

        /// <summary>
        /// 重置光标数据值
        /// </summary>
        private void ResetCursorData()
        {
            kx = (endX - startX) / (Width - 1.0);
            ky = (startY - endY) / (Height - 1.0);
            CalCursorData(RefCursor);
            CalCursorData(MeaCursor);
            
    }

        /// <summary>
        /// 根据光标位置计算光标对应的数据值
        /// </summary>
        /// <param name="csor"></param>
        private void CalCursorData(FigCursor csor)
        {
            if (csor != null)
            {
                int x = csor.p.X;
                int y = csor.p.Y;

                csor.pdata.X = (float)(_sx + Math.Round(x * kx / xGap)*xGap);
                csor.pdata.Y = (float)(_ey + Math.Round(y * ky / yGap) * yGap);

                ResetCursorPos(csor, new double[2]{ csor.pdata.X, csor.pdata.Y});

            }
        }

        /// <summary>
        /// 控件缩放时重置光标位置
        /// </summary>
        /// <param name="csor"></param>
        /// <param name="CurrentPf"></param>
        private void ResetCursorPos(FigCursor csor, double[] CurrentPf)
        {
            if (csor != null)
            {
                kx = (endX - startX) / (Width - 1.0);
                ky = -(endY - startY) / (Height - 1.0);
                csor.p = new Point((int)(Math.Round((CurrentPf[0]-_sx)/kx)),
                                (int)(Math.Round(CurrentPf[1]-_ey)/ky));
            }
        }
    }

    
}
