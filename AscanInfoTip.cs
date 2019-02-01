using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;

namespace ZYCControl
{
    public class AscanInfoTip
    {
        private float _StartX;
        /// <summary>
        /// 当前图像的起始X值
        /// </summary>
        public float StartX
        {
            set { _StartX = value;}
            get { return _StartX; }
        }
        private float _EndX;
        /// <summary>
        /// 当前图像的终止X值
        /// </summary>
        public float EndX
        {
            set { _EndX = value;}
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
            set { _StartY = value; }
            get { return _StartY; }
        }
        private float _EndY;
        /// <summary>
        /// 当前图像的终止Y值
        /// </summary>
        public float EndY
        {
            set { _EndY = value; }
            get { return _EndY; }
        }
        public float X;
        public float Enable;
        public string InfoFormat;
        public Font InfoFont;

        public void Draw(float data)
        {

        }

        /// <summary>
        /// 按照起始点和步进点设置合适的数据点
        /// </summary>
        /// <param name="a"></param>
        /// <param name="step"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private float RefreshPerData(float a, float step, float c)
        {
            if (step == 0)
                return c;
            else
                return (float)(Math.Round((c - a) / step) * step + a);
        }
    }
}
