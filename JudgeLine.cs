using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace ZYCControl
{
    public class JudgeLine
    {
        public bool Enable;
        public Color color;
        private int _YPixel;
        /// <summary>
        /// 光标Y值(像素点)
        /// </summary>
        public int YPixel
        {
            set { _YPixel = value; ReWriteYPixel(value); }
            get { return _YPixel; }
        }
        private int _XPixel;
        /// <summary>
        /// 光标X值(像素点)
        /// </summary>
        public int XPixel
        {
            set { _XPixel = value; ReWriteXPixel(value); }
            get { return _XPixel; }
        }
        private float _Y;
        /// <summary>
        /// 光标Y值
        /// </summary>
        public float Y
        {
            set { _Y = value; ReWriteY(value); }
            get { return _Y; }
        }
        private float _X;
        /// <summary>
        /// 光标X值
        /// </summary>
        public float X
        {
            set { _X = value; ReWriteX(value); }
            get { return _X; }
        }
        private float _StartX;
        /// <summary>
        /// 当前图像的起始X值
        /// </summary>
        public float StartX
        {
            set { _StartX = value; ReWriteStartX(value); }
            get { return _StartX; }
        }
        private float _EndX;
        /// <summary>
        /// 当前图像的终止X值
        /// </summary>
        public float EndX
        {
            set { _EndX = value; ReWriteEndX(value); }
            get { return _EndX; }
        }
        private float _StepX;
        /// <summary>
        /// 当前图像的X轴间隔值
        /// </summary>
        public float StepX
        {
            set { _StepX = value; }
            get { return _StepX; }
        }
        private float _StartY;
        /// <summary>
        /// 当前图像的起始Y值
        /// </summary>
        public float StartY
        {
            set { _StartY = value; ReWriteStartY(value); }
            get { return _StartY; }
        }
        private float _EndY;
        /// <summary>
        /// 当前图像的终止Y值
        /// </summary>
        public float EndY
        {
            set { _EndY = value; ReWriteEndY(value); }
            get { return _EndY; }
        }
        private float _StepY;
        /// <summary>
        /// 当前图像的Y轴间隔值
        /// </summary>
        public float StepY
        {
            set { _StepY = value; }
            get { return _StepY; }
        }
        private bool _Selected;
        /// <summary>
        /// 光标被选中
        /// </summary>
        public bool Selected
        {
            set { _Selected = value; if (!value) { _SelectedX = _SelectedY = false; } }
            get { return _Selected; }
        }
        private bool _SelectedX;
        /// <summary>
        /// 光标X被选中
        /// </summary>
        public bool SelectedX
        {
            set { _SelectedX = value; if (value) _Selected = true; }
            get { return _SelectedX; }
        }
        private bool _SelectedY;
        /// <summary>
        /// 光标Y被选中
        /// </summary>
        public bool SelectedY
        {
            set { _SelectedY = value; if (value) _Selected = true; }
            get { return _SelectedY; }
        }
        private int _width;
        /// <summary>
        /// 当前图像宽度
        /// </summary>
        public int width
        {
            set { _width = value; ReWriteWidth(); }
            get { return _width; }
        }
        private int _height;
        /// <summary>
        /// 当前图像高度
        /// </summary>
        public int height
        {
            set { _height = value; ReWriteHeight(); }
            get { return _height; }
        }
        public string Name;
        public string InfoFormat;
        public Font InfoFont;

        private void ReWriteYPixel(int value)
        {
            _Y = (_EndY - _StartY) * (_height - 1 - value) / (_height - 1) + _StartY;
            ReWriteY(_Y);
        }

        private void ReWriteXPixel(int value)
        {
            _X = (_EndX - _StartX) * value / (_width - 1) + _StartX;
            ReWriteX(_X);
        }

        private void ReWriteY(float value)
        {
            _Y = RefreshPerData(_StartY, _StepY, value);
            _YPixel = (int)(Math.Round(
                _height - 1 - (_Y - _StartY) * (_height - 1) / (_EndY - _StartY)));
        }

        private void ReWriteX(float value)
        {
            _X = RefreshPerData(_StartX, _StepX, value);
            _XPixel = (int)(Math.Round(
                (_X - _StartX) * (_width - 1) / (_EndX - _StartX)));
        }

        private void ReWriteStartX(float value)
        {
            _StartX = RefreshPerData(_StartX, _StepX, value);
            ReWriteX(_X);
        }

        private void ReWriteEndX(float value)
        {
            _EndX = RefreshPerData(_StartX, _StepX, value);
            ReWriteX(_X);
        }

        private void ReWriteStartY(float value)
        {
            _StartY = RefreshPerData(_StartY, _StepY, value);
            ReWriteY(_Y);
        }

        private void ReWriteEndY(float value)
        {
            _EndY = RefreshPerData(_StartY, _StepY, value);
            ReWriteY(_Y);
        }

        private void ReWriteWidth()
        {
            ReWriteX(_X);
        }

        private void ReWriteHeight()
        {
            ReWriteY(_Y);
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
            if (step == 0)
                return c;
            else
                return (float)(Math.Round((c - a) / step) * step + a);
        }

        public void Draw(Graphics g)
        {
            if (Enable)
            {
                Pen pen = new Pen(color);
                Pen blank = new Pen(Color.White);
                try
                {
                    g.DrawLine(pen, 0, _YPixel, _width - 1, _YPixel);
                    //g.DrawLine(blank, 0, _YPixel + 1, _width - 1, _YPixel + 1);
                    //g.DrawLine(pen, _XPixel, 0, _XPixel, _height - 1);
                    //g.DrawLine(blank, _XPixel + 1, 0, _XPixel + 1, _height - 1);

                    string strx = string.Format(InfoFormat, _X);
                    string stry = string.Format(InfoFormat, _Y);

                    /*SizeF sizex = g.MeasureString(strx, InfoFont);
                    Size sizesx = new Size((int)sizex.Width + 1, (int)sizex.Height + 1);*/
                    SizeF sizey = g.MeasureString(stry, InfoFont);
                    Size sizesy = new Size((int)sizey.Width + 1, (int)sizey.Height + 1);

                    Point ps = new Point();
                    //if (_XPixel < _width / 2)
                    //    ps = new Point(_XPixel, 0);
                    //else
                    //    ps = new Point(_XPixel - sizesx.Width, 0);
                    //g.FillRectangle(new SolidBrush(Color.White), ps.X, ps.Y, sizesx.Width, sizesx.Height);
                    //g.DrawString(strx, InfoFont, new SolidBrush(Color.Black), ps);

                    if (_YPixel < _height / 2)
                        ps = new Point(0, _YPixel);
                    else
                        ps = new Point(0, _YPixel - sizesy.Height);
                    g.FillRectangle(new SolidBrush(Color.White), ps.X, ps.Y, sizesy.Width, sizesy.Height);
                    g.DrawString(stry, InfoFont, new SolidBrush(Color.Black), ps);
                }
                catch
                {

                }
            }
        }

        public string MouseState(Point mouse)
        {
            if (!Enable)
                return null;
            Point dp = new Point(mouse.X - _XPixel, mouse.Y - _YPixel);
            double dpx = Math.Abs(dp.X);
            double dpy = Math.Abs(dp.Y);
            if (dpx < 5)
                return "X";
            if (dpy < 5)
                return "Y";
            return null;
        }
    }
}
