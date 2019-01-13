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
    public partial class FigCursors : UserControl
    {
        public string style;
        public int length;
        public double angle;
        public Color color;
        private int linewidth;
        public Graphics g;
        public bool CursorIn;
        public Point p;

        public FigCursors()
        {
            InitializeComponent();
            linewidth = 6;
            g = CreateGraphics();
            
        }
               
        
        private void AddCursor()
        {
            switch (style)
            {
                case "H":
                    GenerateHCursor();
                    break;

                case "V":
                    GenerateVCursor();
                    break;
            }
        }

        private void GenerateHCursor()
        {
            Width = length;
            Height = linewidth;
            int y = linewidth / 2 - 1;

            g.DrawLine(new Pen(color, 1), 0, y, Width-1, y);
            g.DrawLine(new Pen(Color.White, 1), 0, y+1, Width - 1, y+1);
        }

        private void GenerateVCursor()
        {
            Height = length;
            Width = linewidth;
            int x = linewidth / 2 - 1;

            g.DrawLine(new Pen(color, 1), x, 0, x, Height-1);
            g.DrawLine(new Pen(Color.White, 1), x+1, 0, x+1, Height - 1);
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            /*Graphics graphics = e.Graphics;
            if (ima != null)
                graphics.DrawImage(ima.bmp, 0, 0);*/
            //DrawJudgeLines(graphics);
            //GC.Collect();
        }

        private void Cursors_MouseMove(object sender, MouseEventArgs e)
        {
            if (RectangleToScreen(ClientRectangle).Contains(PointToScreen(e.Location)))
            {
                CursorIn = true;
                if (style == "H")
                    Cursor = Cursors.HSplit;
                else
                    Cursor = Cursors.VSplit;
            }
        }
    }
}
