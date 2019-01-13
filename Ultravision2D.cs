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
    public partial class Ultravision2D : UserControl
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
        public float zeroRatio;
        private bool firstZoom;
        /// <summary>
        /// 图像显示区域的width和height，0~1
        /// </summary>
        private float wt, ht;
        public bool RefCursorShow, MeaCursorShow;
        /// <summary>
        /// 当前数据的起末点信息
        /// </summary>
        public double Vstart, Vend, Hstart, Hend;
        private Font CursorInfoFont;
        private string CursorInfoSFormat;
        private Graphics g;
                
        public Ultravision2D()
        {
            InitializeComponent();

            CursorInfoFont = new Font("Times New Roman", 8);
            CursorInfoSFormat = "{0:0.00}";
            g = CreateGraphics();
            CursorsInitail();
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

        private void Ultravision2D_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            JudgeMouseIsInControl(p);

            //画阴影区域
            if (zoomRegion != null)
            {
                DrawRectangle(p);
            }
        }

        private void Ultravision2D_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseIsInControl)
            {
                zoomRegion = new DrawZoomRegion();
                zoomRegion.p0 = e.Location;
            }            
        }

        private void Ultravision2D_MouseUp(object sender, MouseEventArgs e)
        {
            if (zoomRegion != null & AltIsDown == false)
            {
                Invalidate();
                CalRealZoomRect(new Rectangle(zoomRegion.minX, zoomRegion.minY, zoomRegion.width, zoomRegion.height));
                
                zoomRegion = null;

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

            if (zoomRegion.pChanged)
            {
                Invalidate();
                //zoomRegion.Draw(true);
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
        
        private void Ultravision2D_KeyDown(object sender, KeyEventArgs e)
        {
            var m = ModifierKeys;
            if (m == Keys.Alt)
                AltIsDown = true;
            if (m == Keys.Control)
                ControlIsDown = true;
            if (m == Keys.Shift)
                ShiftIsDown = true;
        }

        private void Ultravision2D_KeyUp(object sender, KeyEventArgs e)
        {
            var m = ModifierKeys;
            if (m == Keys.None)
            {
                AltIsDown = false;
                ControlIsDown = false;
                ShiftIsDown = false;
            }
        }

        private void Ultravision2D_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (AltIsDown)
            {
                ima.inputData.ReSet();
                ima.DisplayZoneMin = new float[2] { 0, 0 };
                ima.DisplayZoneMax = new float[2] { 1, 1 };
                ima.RefreshBMP();
                firstZoom = true;
            }
            else
            {                
                if (e.Button == MouseButtons.Left)
                {
                    MeaCursorShow = true;
                    CursorsInitail();
                    MeaCursorsH.p = MeaCursorsV.p = e.Location;
                    RefreshDataCursors();
                }
                else
                {
                    RefCursorShow = true;
                    CursorsInitail();
                    RefCursorsH.p = RefCursorsV.p = e.Location;
                    RefreshDataCursors();
                }

            }
        }

        private void Ultravision2D_SizeChanged(object sender, EventArgs e)
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

        #region 光标
        
        private void CursorsInitail()
        {
                RefCursorsH = new FigCursors();
                RefCursorsH.Parent = this;
                RefCursorsH.style = "H";
                RefCursorsH.color = Color.Red;
                RefCursorsH.p = new Point(0, 0);

                RefCursorsV = new FigCursors();
                RefCursorsV.Parent = this;
                RefCursorsV.style = "V";
                RefCursorsV.color = Color.Red;
                RefCursorsV.p = new Point(0, 0);

                MeaCursorsH = new FigCursors();
                MeaCursorsH.Parent = this;
                MeaCursorsH.style = "H";
                MeaCursorsH.color = Color.Blue;
                MeaCursorsH.p = new Point(0, 0);

                MeaCursorsV = new FigCursors();
                MeaCursorsV.Parent = this;
                MeaCursorsV.style = "V";
                MeaCursorsV.color = Color.Blue;
                MeaCursorsV.p = new Point(0, 0);
        }

        private void RefreshDataCursors()
        {   
            if (MeaCursorShow)
            {
                AddCursorsInfo(MeaCursorsH);
                AddCursorsInfo(MeaCursorsV);
            }
            if (RefCursorShow)
            {
                AddCursorsInfo(RefCursorsH);
                AddCursorsInfo(RefCursorsV);
            }
        }

        private void AddCursorsInfo(FigCursors figCursors)
        {
            double info = CalCursorsInfo(figCursors.p, figCursors.style);
            DrawInfo(info, figCursors.style, figCursors.p);
        }
        
        private double CalCursorsInfo(Point p, string style)
        {
            if (style == "H")
                return (1 - p.Y / Height) * (Vend - Vstart) + Vstart;
            else
                return p.X / Width * (Hend - Hstart) + Hstart;
        }

        private void DrawInfo(double info, string style, Point p)
        {
            SizeF sif = g.MeasureString(info.ToString(CursorInfoSFormat), CursorInfoFont);
            Size sii = new Size((int)(sif.Width + 1), (int)(sif.Height + 1));
            Point pinfo = new Point(0, p.Y);
            if (style == "H")
            {
                if (p.Y / Height < 0.5){
                    //不变。
                }
                else
                    pinfo = new Point(0, p.Y - sii.Height);
            }
            else
            {
                if (p.X / Width < 0.5)
                    pinfo = new Point(p.X, 0);
                else
                    pinfo = new Point(p.X - sii.Width, 0);
            }

            Rectangle rectangle = new Rectangle(pinfo, sii);
            g.DrawRectangle(new Pen(Color.White, 1), rectangle);
            g.DrawString(CursorInfoSFormat, CursorInfoFont, new SolidBrush(Color.Black), pinfo);
        }
        #endregion
    }
}
