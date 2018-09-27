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
    public partial class DataTips : UserControl
    {
        /// <summary>
        /// 当前数据点的位置
        /// </summary>
        public Point DataLocation;
        /// <summary>
        /// 当前显示窗的矩形
        /// </summary>
        public Rectangle CurrentDisplayRect;
        public Bitmap bmp;
        public Control parent;

        /// <summary>
        /// 数据点的黑色方框
        /// </summary>
        private Rectangle DataBlackPoint;
        /// <summary>
        /// 数据点黑色方框尺寸，为奇数，方便获取方框中心点位置
        /// </summary>
        public int PointSize = 5;
        private Rectangle TextBoxRect;
        public Color[,] OriDataPointData = new Color[5, 5];

        public DataTips()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 添加数据游标
        /// </summary>
        /// <param name="x">数据点在InputMatrix中的X坐标</param>
        /// <param name="y">数据点在InputMatrix中的Y坐标</param>
        /// <param name="data">数据点对应的原始数据</param>
        /// <param name="colorIndex">数据点对应的32bit颜色索引</param>
        public void Add(int x, int y, float data, uint colorIndex)
        {
            if (bmp == null)
                Console.WriteLine("请指定bmp");
            
            CurrentDisplayRect = new Rectangle();
            CurrentDisplayRect.Width = parent.Width;
            CurrentDisplayRect.Height = parent.Height;
            ///字符
            textBox1.Text = "[x,y]: [" + x.ToString() + " " + y.ToString() + "]\r\n"
                + "Data: " + data.ToString() + "\r\n"
                + "Index: " + colorIndex.ToString();
            textBox1.Enabled = false;
            TextBoxRect = textBox1.ClientRectangle;        

            ///黑点
            DataBlackPoint = new Rectangle();
            LocationEachPart();
            DataBlackPoint.Size = new Size(PointSize, PointSize);
            for (int i =0; i< PointSize; i++)
                for (int j = 0; j < PointSize; j++)
                {
                    OriDataPointData[i, j] = bmp.GetPixel(DataBlackPoint.X + i, DataBlackPoint.Y + j);
                    bmp.SetPixel(DataBlackPoint.X + i, DataBlackPoint.Y + j,Color.Black);
                }
            parent.Controls.Add(textBox1);
        }

        public void Clear()
        {
            for (int i = 0; i < PointSize; i++)
                for (int j = 0; j < PointSize; j++)
                {
                    bmp.SetPixel(DataBlackPoint.X + i, DataBlackPoint.Y + j, OriDataPointData[i, j]) ;
                }
            parent.Controls.Clear();

            Dispose(false);
        }

        /// <summary>
        /// 对各个元素进行定位,并重新定义控件大小
        /// </summary>
        private void LocationEachPart()
        {

            double CurrentDisplayRect_HalfWidth = CurrentDisplayRect.Width / 2;
            double CurrentDisplayRect_HalfHeight = CurrentDisplayRect.Height / 2;

            Width = textBox1.Width + PointSize;
            Height = textBox1.Height + PointSize;
            int dx = DataLocation.X;
            int dy = DataLocation.Y;
            int halfDataBlackSize = (PointSize - 1) / 2;
            DataBlackPoint.Location = new Point(dx - halfDataBlackSize, dy - halfDataBlackSize);
            int halfPointAndTextBoxHeight = halfDataBlackSize + textBox1.Height;
            int halfPointAndTextBoxWidth = halfDataBlackSize + textBox1.Width;
            if (dx < CurrentDisplayRect_HalfWidth)
            {
                if (dy < CurrentDisplayRect_HalfHeight)
                {
                    textBox1.Location = new Point(halfDataBlackSize + dx, halfDataBlackSize + dy);
                }
                else
                {
                    textBox1.Location = new Point(dx + halfDataBlackSize, -halfPointAndTextBoxHeight + dy);
                }
            }
            else
            {
                if (dy < CurrentDisplayRect_HalfHeight)
                {
                    textBox1.Location = new Point(dx - halfPointAndTextBoxWidth, dy + halfDataBlackSize);
                }
                else
                {
                    textBox1.Location = new Point(dx - halfPointAndTextBoxWidth, dy-halfPointAndTextBoxHeight);
                }
            }      
            
        }

        
    }
}
