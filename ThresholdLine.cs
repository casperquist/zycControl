using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZYCControl
{
    public class ThresholdLine
    {
        private float _StartY;
        /// <summary>
        /// 当前图像的起始Y值
        /// </summary>
        public float StartY
        {
            set { _StartY = value;}
            get { return _StartY; }
        }        
        private float _EndY;
        /// <summary>
        /// 当前图像的终止Y值
        /// </summary>
        public float EndY
        {
            set { _EndY = value;}
            get { return _EndY; }
        }
        public Color color;
        public float Threshold;
        public bool Enable;

        public void Draw(Graphics g, Size size)
        {
            if (Enable)
            {
                int py = (int)Math.Round((size.Height - 1) - 
                    (Threshold - _StartY) / (_EndY - _StartY) * (size.Height - 1));
                g.DrawLine(new Pen(color),
                    new Point(0, py),
                    new Point(size.Width - 1, py));
            }
        }

        
    }
}
