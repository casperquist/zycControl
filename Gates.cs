using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ZYCControl
{
    public class Gates: INotifyPropertyChanged
    {
        public bool Enable;
        public Color color;
        public string Name;
        private int _ThresholdPixel;

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged<T>(Expression<Func<T>> property)
        {
            if (PropertyChanged == null)
                return;

            var memberExpression = property.Body as MemberExpression;
            if (memberExpression == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
        }
        /// <summary>
        /// 闸门阈值(像素点)
        /// </summary>
        public int ThresholdPixel
        {
            set { _ThresholdPixel = value; ReWriteThresholdPixel(value); }
            get { return _ThresholdPixel; }
        }
        private int _StartPixel;
        /// <summary>
        /// 闸门起始点(像素点)
        /// </summary>
        public int StartPixel
        {
            set { _StartPixel = value; ReWriteStartPixel(value); }
            get { return _StartPixel; }
        }
        private int _EndPixel;
        /// <summary>
        /// 闸门终止点(像素点)
        /// </summary>
        public int EndPixel
        {
            set { _EndPixel = value; ReWriteEndPixel(value); }
            get { return _EndPixel; }
        }
        private float _Threshold;
        /// <summary>
        /// 闸门阈值
        /// </summary>
        public float Threshold
        {
            set { _Threshold = value; ReWriteThreshold(value); }
            get { return _Threshold; }
        }
        private float _Start;
        /// <summary>
        /// 闸门起始值
        /// </summary>
        public float Start
        {
            set { _Start = value; ReWriteStart(value); }
            get { return _Start; }
        }
        private float _End;
        /// <summary>
        /// 闸门终止值
        /// </summary>
        public float End
        {
            set { _End = value; ReWriteEnd(value); }
            get { return _End; }
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
        /// 闸门被选中
        /// </summary>
        public bool Selected
        {
            set { _Selected = value; if (!value) { _SelectedX0 = _SelectedX1 = false; } }
            get { return _Selected; }
        }
        private bool _SelectedX0;
        /// <summary>
        /// 闸门起点被选中
        /// </summary>
        public bool SelectedX0
        {
            set { _SelectedX0 = value; if (value) _Selected = true; }
            get { return _SelectedX0; }
        }
        private bool _SelectedX1;
        /// <summary>
        /// 闸门终点被选中
        /// </summary>
        public bool SelectedX1
        {
            set { _SelectedX1 = value; if (value) _Selected = true; }
            get { return _SelectedX1; }
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

        private void ReWriteThresholdPixel(int value)
        {
            _Threshold = (_EndY - _StartY) * (_height - 1 - value) / (_height - 1) + _StartY;
            ReWriteThreshold(_Threshold);
        }

        private void ReWriteStartPixel(int value)
        {
            _Start = (_EndX - _StartX) * value / (_width - 1) + _StartX;
            ReWriteStart(_Start);
        }

        private void ReWriteEndPixel(int value)
        {            
            _End = (_EndX - _StartX) * value / (_width - 1) + _StartX;
            ReWriteEnd(_End);
        }

        private void ReWriteThreshold(float value)
        {
            _Threshold = RefreshPerData(_StartY, _StepY, value);
            _ThresholdPixel = (int)(Math.Round(
                _height - 1 - (_Threshold - _StartY) * (_height - 1) / (_EndY - _StartY)));
        }

        private void ReWriteStart(float value)
        {
            _Start = RefreshPerData(_StartX, _StepX, value);
            _StartPixel = (int)(Math.Round(
                (_Start - _StartX) * (_width - 1) / (_EndX - _StartX)));
        }

        private void ReWriteEnd(float value)
        {
            _End = RefreshPerData(_StartX, _StepX, value);
            _EndPixel = (int)(Math.Round(
                (_End - _StartX) * (_width - 1) / (_EndX - _StartX)));
        }

        private void ReWriteStartX(float value)
        {
            _StartX = RefreshPerData(_StartX, _StepX, value);
            ///start,end不变化
            ReWriteStart(_Start);
            ReWriteEnd(_End);
        }

        private void ReWriteEndX(float value)
        {
            _EndX = RefreshPerData(_StartX, _StepX, value);
            ///start,end不变化
            ReWriteStart(_Start);
            ReWriteEnd(_End);
        }

        private void ReWriteStartY(float value)
        {
            _StartY = RefreshPerData(_StartY, _StepY, value);
            ReWriteThreshold(_Threshold);
        }

        private void ReWriteEndY(float value)
        {
            _EndY = RefreshPerData(_StartY, _StepY, value);
            ReWriteThreshold(_Threshold);
        }

        private void ReWriteWidth()
        {
            ReWriteStart(_Start);
            ReWriteEnd(_End);
        }

        private void ReWriteHeight()
        {
            ReWriteThreshold(_Threshold);
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
                g.DrawLine(new Pen(color,5),
                    new Point(_StartPixel, _ThresholdPixel),
                    new Point(_EndPixel, _ThresholdPixel));
            }
        }

        public string MouseState(Point mouse)
        {
            if (!Enable)
                return null;
            if (Math.Abs(mouse.Y - _ThresholdPixel) > 5)
                return null;
            else
            {
                if (Math.Abs(mouse.X - (_StartPixel + _EndPixel) / 2) < 5)
                    return "mid";
                if (Math.Abs(mouse.X - _StartPixel) < 5)
                    return "start";
                if (Math.Abs(mouse.X - _EndPixel) < 5)
                    return "end";
                return null;
            }
        }
    }
}
