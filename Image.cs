using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZYCControl
{
    
    
    /// <summary>
    /// 输入数据参数
    /// </summary>
    public class InputImageData
    {
        public float[] data;
        public int row;
        public int col;
        public uint[,] matrix;
        public float stepBmp;
        public float minBmp;
        public bool different;
        public float ratio;

        public InputImageData()
        {
            data = new float[0];
            row = 0;
            col = 0;
            matrix = new uint[0, 0];
            stepBmp = 1.0f;
            minBmp = 0;
            ratio = 0;
        }

        public InputImageData(float[] a, int r, int c)
        {
            data = a;
            row = r;
            col = c;
            matrix = Array2Matrix(a);
            stepBmp = 1.0f;
            minBmp = 0;
        }        

        public InputImageData(float[] a, int r, int c, OutputBmp bmp)
        {
            data = a;
            row = r;
            col = c;
            
            stepBmp = bmp.stepBmp;
            minBmp = bmp.minBmp;
            matrix = Array2Matrix(a);
        }

        public InputImageData(InputImageData a, OutputBmp bmp)
        {
            data = a.data;
            row = a.row;
            col = a.col;
            
            stepBmp = bmp.stepBmp;
            minBmp = bmp.minBmp;
            matrix = Array2Matrix(data);
        }

        public bool Changed(float[] a, int r, int c)
        {
            if (data != a | row != r | col != c)
                return true;
            else
                return false;
        }

        public bool Changed(InputImageData nd)
        {
            if (data != nd.data | row != nd.row | col != nd.col | ratio != nd.ratio)
                return true;
            else
                return false;

        }         
        

        /// <summary>
        /// 将1维矩阵转成2维矩阵
        /// </summary>
        /// <param name="x">数据源</param>
        /// <param name="row">行数</param>
        /// <param name="col">列数</param>
        /// <returns>二维矩阵</returns>
        private uint[,] Array2Matrix(float[] x)
        {
            if (x.Length != row * col)
                return null;
            int index = 0;
            uint[,] y = new uint[row, col];
            for (int i = 0; i < row; i++)
                for (int j = 0; j < col; j++)
                    y[i, j] = Float2Index(x[index++]);
            return y;
        }
        
        /// <summary>
        /// 将原始数据转成256色的索引值
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private uint Float2Index(float x)
        {
            int y = (int)((x - minBmp) / stepBmp + 0.5);
            if (y > 255)
                return 255;
            else
            {
                if (y < 0)
                    return 0;
                else
                    return (uint)y;
            }
        }

        public void ReSet()
        {
            matrix = null;
            matrix = Array2Matrix(data);
        }
    };

    /// <summary>
    /// 输出图像参数
    /// </summary>
    public class OutputBmp
    {
        public int height;
        public int width;
        public Bitmap bmp;
        public float minBmp;
        public float maxBmp;
        public float stepBmp;
        public bool different;

        public OutputBmp()
        {

            minBmp = 0;
            maxBmp = 255;
            height = 1;
            width = 1;
            ReSet();
        }

        public OutputBmp(float min, float max)
        {
            height = 1;
            width = 1;
            minBmp = min;
            maxBmp = max;
            ReSet();
        }

        public OutputBmp(float min, float max, int wd, int ht)
        {
            minBmp = min;
            maxBmp = max;
            width = wd;
            height = ht;
            ReSet();
        }

        public bool Changed(float min, float max)
        {
            if (minBmp != min | maxBmp != max)
                return true;
            else
                return false;
        }

        public bool Changed(int wd, int ht)
        {
            if (width != wd | height != ht)
                return true;
            else
                return false;
        }

        public bool Changed(OutputBmp nob)
        {
            if (height != nob.height | width != nob.width | minBmp != nob.minBmp | maxBmp != nob.maxBmp)
                return true;
            else
                return false;
        }

        public void ReSet()
        {
            if (bmp == null)
            {
                bmp = new Bitmap(width, height);
                stepBmp = (float)((maxBmp - minBmp) / 255.0);
            }
            else
            {
                bmp.Dispose();
                bmp = new Bitmap(width, height);
                stepBmp = (float)((maxBmp - minBmp) / 255.0);
            }
        }
    };    

    /// <summary>
    /// 指针法set，get pixel
    /// </summary>
    public class PointBitmap
    {
        Bitmap source = null;
        IntPtr Iptr = IntPtr.Zero;
        BitmapData bitmapData = null;

        public int Depth { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public PointBitmap(Bitmap source)
        {
            this.source = source;
        }

        public void LockBits()
        {
            try
            {
                // Get width and height of bitmap
                Width = source.Width;
                Height = source.Height;

                // get total locked pixels count
                int PixelCount = Width * Height;

                // Create rectangle to lock
                Rectangle rect = new Rectangle(0, 0, Width, Height);

                // get source bitmap pixel format size
                Depth = System.Drawing.Bitmap.GetPixelFormatSize(source.PixelFormat);

                // Check if bpp (Bits Per Pixel) is 8, 24, or 32
                if (Depth != 8 && Depth != 24 && Depth != 32)
                {
                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                }

                // Lock bitmap and return bitmap data
                bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
                                             source.PixelFormat);

                //得到首地址
                unsafe
                {
                    Iptr = bitmapData.Scan0;
                    //二维图像循环

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UnlockBits()
        {
            try
            {
                source.UnlockBits(bitmapData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Color GetPixel(int x, int y)
        {
            unsafe
            {
                byte* ptr = (byte*)Iptr;
                ptr = ptr + bitmapData.Stride * y;
                ptr += Depth * x / 8;
                Color c = Color.Empty;
                if (Depth == 32)
                {
                    int a = ptr[3];
                    int r = ptr[2];
                    int g = ptr[1];
                    int b = ptr[0];
                    c = Color.FromArgb(a, r, g, b);
                }
                else if (Depth == 24)
                {
                    int r = ptr[2];
                    int g = ptr[1];
                    int b = ptr[0];
                    c = Color.FromArgb(r, g, b);
                }
                else if (Depth == 8)
                {
                    int r = ptr[0];
                    c = Color.FromArgb(r, r, r);
                }
                return c;
            }
        }

        public void SetPixel(int x, int y, Color c)
        {
            unsafe
            {
                byte* ptr = (byte*)Iptr;
                ptr = ptr + bitmapData.Stride * y;
                ptr += Depth * x / 8;
                if (Depth == 32)
                {
                    ptr[3] = c.A;
                    ptr[2] = c.R;
                    ptr[1] = c.G;
                    ptr[0] = c.B;
                }
                else if (Depth == 24)
                {
                    ptr[2] = c.R;
                    ptr[1] = c.G;
                    ptr[0] = c.B;
                }
                else if (Depth == 8)
                {
                    ptr[2] = c.R;
                    ptr[1] = c.G;
                    ptr[0] = c.B;
                }
            }
        }
    }

    /// <summary>
    /// 二维图成像
    /// </summary>
    public class Image
    {
        /*[DllImport("kernel32")]
        static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);*/
        /// <summary>
        /// 用C++加速矩阵运算
        /// </summary>
        /// <param name="a">当前显示的矩阵_inputData.matrix[0,0]</param>
        /// <param name="b">输出的index</param>
        /// <param name="matc">_inputData.matrix的列数</param>
        /// <param name="matr">_inputData.matrix的行数</param>
        /// <param name="width">_outBmp的宽度</param>
        /// <param name="height">_outBmp的高度</param>
        [DllImport(@"D:\code\VC++\ImageProcess\Release\ImageProcess.dll", EntryPoint = "GetNewIndexMatrix")]
        public extern static void GetNewIndexMatrix(ref uint a, ref Int16 b, Int32 matc, Int32 matr, Int32 width, Int32 height);
        
        /// <summary>
        /// 刷新位图
        /// </summary>
        public void RefreshBMP()
        {

            _outBmp.ReSet();
            pb = new PointBitmap(_outBmp.bmp);
            pb.LockBits();
            uint[,] index = new uint[_outBmp.height, _outBmp.width];
            long t0 = 0;

            t0 = System.Environment.TickCount;
            for (int i = 0; i < _outBmp.height; i++)
                for (int j = 0; j < _outBmp.width; j++)
                {
                    index[i,j] = GetColorIndex(j,i);
                }

            for (int i = 0; i < _outBmp.height; i++)
                for (int j = 0; j < _outBmp.width; j++)
                {
                    pb.SetPixel(j, i, Color.FromArgb(Transparency, colormap[index[i, j]]));
                }
            
            pb.UnlockBits();
            bmp = _outBmp.bmp;
        }

        /// <summary>
        /// 计算像素点位置处的颜色索引
        /// </summary>
        /// <param name="px">像素点坐标x</param>
        /// <param name="py">像素点坐标y</param>
        /// <param name="row">bmpData行数</param>
        /// <param name="col">bmpData列数</param>
        /// <returns>素点位置处的颜色索引</returns>
        private uint GetColorIndex(int px, int py)
        {
            float stepx, stepy;
            int matc = _inputData.matrix.GetLength(1);
            int matr = _inputData.matrix.GetLength(0);
            stepx = matc / ((_outBmp.width - 1) * 1.0f);
            stepy = matr / ((_outBmp.height - 1) * 1.0f);

            float tpy = py * stepy + 1;
            float tpx = px * stepx + 1;

            int subMatrixRow0 = (int)((py > 1 ? py - 1 : 0) * stepy);
            int subMatrixRow1 = (int)(tpy < matr ? tpy : matr);
            int subMatrixCol0 = (int)((px > 1 ? px - 1 : 0) * stepx);
            int subMatrixCol1 = (int)(tpx < matc ? tpx : matc);

            int sr = subMatrixRow1 - subMatrixRow0;
            int sc = subMatrixCol1 - subMatrixCol0;
            uint maxMatrix = 0;
            uint tmp;
            for (int i = 0; i < sr; i++)
                for (int j = 0; j < sc; j++)
                {
                    tmp = _inputData.matrix[i + subMatrixRow0, j + subMatrixCol0];
                    if (tmp > maxMatrix)
                    {
                        maxMatrix = tmp;
                    }
                }
            return maxMatrix;
        }

        /// <summary>
        /// 计算像素点位置处的颜色索引
        /// </summary>
        /// <param name="px">像素点坐标x</param>
        /// <param name="py">像素点坐标y</param>
        /// <param name="matrixR">在matrix中的行坐标</param>
        /// <param name="matrixC">在matrix中的行坐标</param>
        /// <param name="DataIndex">在InputImageData.data中的索引</param>
        /// <returns>素点位置处的颜色索引</returns>
        public uint GetColorIndex(int px, int py, ref int matrixR, ref int matrixC, ref int DataIndex)
        {

            float stepx, stepy;
            int matc = _inputData.matrix.GetLength(1);
            int matr = _inputData.matrix.GetLength(0);
            stepx = matc / ((_outBmp.width - 1) * 1.0f);
            stepy = matr / ((_outBmp.height - 1) * 1.0f);

            float tpy = py * stepy + 1;
            float tpx = px * stepx + 1;

            int subMatrixRow0 = (int)((py > 1 ? py - 1 : 0) * stepy);
            int subMatrixRow1 = (int)(tpy < matr ? tpy : matr);
            int subMatrixCol0 = (int)((px > 1 ? px - 1 : 0) * stepx);
            int subMatrixCol1 = (int)(tpx < matc ? tpx : matc);

            int sr = subMatrixRow1 - subMatrixRow0;
            int sc = subMatrixCol1 - subMatrixCol0;
            uint maxMatrix = 0;
            uint tmp;
            matrixR = subMatrixRow0;
            matrixC = subMatrixCol0;
            for (int i = 0; i < sr; i++)
                for (int j = 0; j < sc; j++)
                {
                    tmp = _inputData.matrix[i + subMatrixRow0, j + subMatrixCol0];
                    if (tmp > maxMatrix)
                    {
                        maxMatrix = tmp;
                        matrixR = i + subMatrixRow0;
                        matrixC = j + subMatrixCol0;
                        DataIndex = matrixR * matc + matrixC;
                    }
                }
            return maxMatrix;
        }

        /// <summary>
        /// 图像放大
        /// </summary>
        /// <param name="row0"></param>
        /// <param name="row1"></param>
        /// <param name="col0"></param>
        /// <param name="col1"></param>
        public void ZoomBmp(int row0, int row1, int col0, int col1)
        {
            int minr, maxr, minc, maxc;
            minr = row0 < row1 ? row0 : row1;
            maxr = row0 > row1 ? row0 : row1;
            minc = col0 < col1 ? col0 : col1;
            maxc = col0 > col1 ? col0 : col1;
            _inputData.matrix.AsParallel();
            int row = maxr - minr + 1;
            int col = maxc - minc + 1;
            uint[,] tmp = new uint[row, col];
            for (int i = 0; i < row; i++)
                for (int j = 0; j < col; j++)
                {
                    tmp[i, j] = _inputData.matrix[minr + i, minc + j];
                }
            
            _inputData.matrix = null;
            _inputData.matrix = tmp;
            
            tmp = null;

        }

        #region 全局量

        /// <summary>
        /// 256色色谱，ULTRAVISION
        /// </summary>
        private Color[] colormap = new Color[256] {
                    Color.FromArgb(255, 255, 255),
                    Color.FromArgb(251, 253, 255),
                    Color.FromArgb(247, 250, 254),
                    Color.FromArgb(243, 248, 254),
                    Color.FromArgb(239, 245, 253),
                    Color.FromArgb(235, 243, 252),
                    Color.FromArgb(231, 240, 252),
                    Color.FromArgb(226, 238, 251),
                    Color.FromArgb(222, 235, 250),
                    Color.FromArgb(218, 233, 250),
                    Color.FromArgb(214, 230, 249),
                    Color.FromArgb(210, 228, 248),
                    Color.FromArgb(206, 225, 248),
                    Color.FromArgb(201, 223, 247),
                    Color.FromArgb(197, 220, 246),
                    Color.FromArgb(193, 218, 246),
                    Color.FromArgb(189, 215, 245),
                    Color.FromArgb(184, 212, 244),
                    Color.FromArgb(180, 210, 244),
                    Color.FromArgb(176, 208, 243),
                    Color.FromArgb(172, 205, 243),
                    Color.FromArgb(168, 203, 242),
                    Color.FromArgb(164, 200, 241),
                    Color.FromArgb(160, 198, 241),
                    Color.FromArgb(155, 195, 240),
                    Color.FromArgb(151, 193, 239),
                    Color.FromArgb(147, 190, 239),
                    Color.FromArgb(143, 188, 238),
                    Color.FromArgb(139, 185, 237),
                    Color.FromArgb(135, 183, 237),
                    Color.FromArgb(130, 181, 236),
                    Color.FromArgb(126, 178, 235),
                    Color.FromArgb(122, 176, 235),
                    Color.FromArgb(118, 173, 234),
                    Color.FromArgb(113, 170, 233),
                    Color.FromArgb(111, 167, 231),
                    Color.FromArgb(108, 163, 228),
                    Color.FromArgb(105, 159, 226),
                    Color.FromArgb(102, 155, 223),
                    Color.FromArgb(99, 151, 221),
                    Color.FromArgb(96, 148, 218),
                    Color.FromArgb(93, 144, 216),
                    Color.FromArgb(90, 140, 213),
                    Color.FromArgb(87, 136, 211),
                    Color.FromArgb(84, 132, 208),
                    Color.FromArgb(81, 129, 206),
                    Color.FromArgb(78, 125, 203),
                    Color.FromArgb(75, 121, 201),
                    Color.FromArgb(72, 117, 198),
                    Color.FromArgb(69, 113, 196),
                    Color.FromArgb(66, 110, 193),
                    Color.FromArgb(62, 105, 190),
                    Color.FromArgb(60, 102, 188),
                    Color.FromArgb(57, 98, 185),
                    Color.FromArgb(54, 94, 182),
                    Color.FromArgb(51, 90, 179),
                    Color.FromArgb(48, 86, 177),
                    Color.FromArgb(46, 82, 174),
                    Color.FromArgb(43, 78, 171),
                    Color.FromArgb(40, 74, 168),
                    Color.FromArgb(37, 70, 166),
                    Color.FromArgb(34, 66, 163),
                    Color.FromArgb(32, 62, 160),
                    Color.FromArgb(29, 58, 157),
                    Color.FromArgb(26, 54, 155),
                    Color.FromArgb(23, 50, 152),
                    Color.FromArgb(20, 46, 149),
                    Color.FromArgb(18, 42, 146),
                    Color.FromArgb(14, 37, 143),
                    Color.FromArgb(14, 39, 143),
                    Color.FromArgb(15, 41, 142),
                    Color.FromArgb(16, 43, 141),
                    Color.FromArgb(17, 45, 140),
                    Color.FromArgb(17, 47, 139),
                    Color.FromArgb(18, 49, 139),
                    Color.FromArgb(19, 51, 138),
                    Color.FromArgb(20, 53, 137),
                    Color.FromArgb(20, 55, 136),
                    Color.FromArgb(21, 57, 135),
                    Color.FromArgb(22, 59, 134),
                    Color.FromArgb(23, 61, 134),
                    Color.FromArgb(23, 63, 133),
                    Color.FromArgb(24, 65, 132),
                    Color.FromArgb(25, 67, 131),
                    Color.FromArgb(26, 69, 130),
                    Color.FromArgb(27, 72, 129),
                    Color.FromArgb(28, 75, 129),
                    Color.FromArgb(30, 79, 129),
                    Color.FromArgb(32, 83, 129),
                    Color.FromArgb(34, 87, 129),
                    Color.FromArgb(36, 91, 129),
                    Color.FromArgb(38, 95, 129),
                    Color.FromArgb(40, 99, 129),
                    Color.FromArgb(41, 103, 129),
                    Color.FromArgb(43, 107, 128),
                    Color.FromArgb(45, 111, 128),
                    Color.FromArgb(47, 115, 128),
                    Color.FromArgb(49, 119, 128),
                    Color.FromArgb(51, 123, 128),
                    Color.FromArgb(53, 127, 128),
                    Color.FromArgb(55, 131, 128),
                    Color.FromArgb(56, 135, 128),
                    Color.FromArgb(59, 140, 127),
                    Color.FromArgb(62, 142, 126),
                    Color.FromArgb(66, 145, 124),
                    Color.FromArgb(70, 148, 122),
                    Color.FromArgb(74, 151, 120),
                    Color.FromArgb(78, 153, 118),
                    Color.FromArgb(82, 156, 116),
                    Color.FromArgb(86, 159, 114),
                    Color.FromArgb(90, 162, 112),
                    Color.FromArgb(94, 164, 110),
                    Color.FromArgb(98, 167, 108),
                    Color.FromArgb(102, 170, 106),
                    Color.FromArgb(106, 173, 104),
                    Color.FromArgb(110, 175, 102),
                    Color.FromArgb(113, 178, 100),
                    Color.FromArgb(117, 181, 98),
                    Color.FromArgb(121, 184, 97),
                    Color.FromArgb(126, 187, 94),
                    Color.FromArgb(130, 189, 92),
                    Color.FromArgb(135, 191, 89),
                    Color.FromArgb(140, 193, 86),
                    Color.FromArgb(145, 195, 83),
                    Color.FromArgb(150, 197, 80),
                    Color.FromArgb(155, 199, 77),
                    Color.FromArgb(160, 201, 74),
                    Color.FromArgb(165, 203, 72),
                    Color.FromArgb(170, 205, 69),
                    Color.FromArgb(175, 208, 66),
                    Color.FromArgb(180, 210, 63),
                    Color.FromArgb(185, 212, 60),
                    Color.FromArgb(190, 214, 57),
                    Color.FromArgb(195, 216, 54),
                    Color.FromArgb(200, 218, 51),
                    Color.FromArgb(205, 220, 49),
                    Color.FromArgb(211, 223, 45),
                    Color.FromArgb(212, 223, 45),
                    Color.FromArgb(214, 222, 45),
                    Color.FromArgb(216, 221, 45),
                    Color.FromArgb(218, 221, 45),
                    Color.FromArgb(219, 220, 45),
                    Color.FromArgb(221, 219, 45),
                    Color.FromArgb(223, 219, 45),
                    Color.FromArgb(225, 218, 45),
                    Color.FromArgb(226, 217, 44),
                    Color.FromArgb(228, 216, 44),
                    Color.FromArgb(230, 216, 44),
                    Color.FromArgb(232, 215, 44),
                    Color.FromArgb(233, 214, 44),
                    Color.FromArgb(235, 214, 44),
                    Color.FromArgb(237, 213, 44),
                    Color.FromArgb(239, 212, 44),
                    Color.FromArgb(241, 211, 43),
                    Color.FromArgb(240, 208, 45),
                    Color.FromArgb(239, 205, 47),
                    Color.FromArgb(238, 202, 49),
                    Color.FromArgb(237, 199, 51),
                    Color.FromArgb(236, 195, 53),
                    Color.FromArgb(235, 192, 56),
                    Color.FromArgb(234, 189, 58),
                    Color.FromArgb(233, 186, 60),
                    Color.FromArgb(231, 182, 62),
                    Color.FromArgb(230, 179, 64),
                    Color.FromArgb(229, 176, 66),
                    Color.FromArgb(228, 173, 69),
                    Color.FromArgb(227, 170, 71),
                    Color.FromArgb(226, 166, 73),
                    Color.FromArgb(225, 163, 75),
                    Color.FromArgb(224, 160, 77),
                    Color.FromArgb(222, 156, 80),
                    Color.FromArgb(222, 154, 80),
                    Color.FromArgb(221, 152, 80),
                    Color.FromArgb(220, 150, 81),
                    Color.FromArgb(219, 148, 81),
                    Color.FromArgb(219, 146, 82),
                    Color.FromArgb(218, 144, 82),
                    Color.FromArgb(217, 142, 82),
                    Color.FromArgb(216, 140, 83),
                    Color.FromArgb(216, 138, 83),
                    Color.FromArgb(215, 136, 84),
                    Color.FromArgb(214, 134, 84),
                    Color.FromArgb(213, 132, 84),
                    Color.FromArgb(213, 130, 85),
                    Color.FromArgb(212, 128, 85),
                    Color.FromArgb(211, 126, 86),
                    Color.FromArgb(210, 124, 86),
                    Color.FromArgb(209, 121, 87),
                    Color.FromArgb(209, 121, 85),
                    Color.FromArgb(209, 121, 83),
                    Color.FromArgb(209, 121, 81),
                    Color.FromArgb(209, 120, 79),
                    Color.FromArgb(208, 120, 76),
                    Color.FromArgb(208, 120, 74),
                    Color.FromArgb(208, 119, 72),
                    Color.FromArgb(208, 119, 70),
                    Color.FromArgb(207, 119, 67),
                    Color.FromArgb(207, 119, 65),
                    Color.FromArgb(207, 118, 63),
                    Color.FromArgb(207, 118, 61),
                    Color.FromArgb(206, 118, 59),
                    Color.FromArgb(206, 117, 56),
                    Color.FromArgb(206, 117, 54),
                    Color.FromArgb(206, 117, 52),
                    Color.FromArgb(205, 116, 49),
                    Color.FromArgb(205, 115, 48),
                    Color.FromArgb(204, 114, 46),
                    Color.FromArgb(204, 113, 45),
                    Color.FromArgb(203, 112, 43),
                    Color.FromArgb(202, 111, 42),
                    Color.FromArgb(202, 110, 40),
                    Color.FromArgb(201, 109, 39),
                    Color.FromArgb(200, 108, 37),
                    Color.FromArgb(200, 107, 36),
                    Color.FromArgb(199, 106, 34),
                    Color.FromArgb(198, 105, 33),
                    Color.FromArgb(198, 104, 31),
                    Color.FromArgb(197, 103, 30),
                    Color.FromArgb(196, 102, 28),
                    Color.FromArgb(196, 101, 27),
                    Color.FromArgb(195, 100, 25),
                    Color.FromArgb(194, 98, 23),
                    Color.FromArgb(193, 96, 23),
                    Color.FromArgb(191, 93, 23),
                    Color.FromArgb(190, 90, 23),
                    Color.FromArgb(188, 87, 23),
                    Color.FromArgb(187, 84, 23),
                    Color.FromArgb(185, 82, 24),
                    Color.FromArgb(183, 79, 24),
                    Color.FromArgb(182, 76, 24),
                    Color.FromArgb(180, 73, 24),
                    Color.FromArgb(179, 70, 24),
                    Color.FromArgb(177, 68, 24),
                    Color.FromArgb(176, 65, 25),
                    Color.FromArgb(174, 62, 25),
                    Color.FromArgb(172, 59, 25),
                    Color.FromArgb(171, 56, 25),
                    Color.FromArgb(169, 53, 25),
                    Color.FromArgb(167, 50, 26),
                    Color.FromArgb(166, 48, 26),
                    Color.FromArgb(165, 46, 26),
                    Color.FromArgb(164, 44, 26),
                    Color.FromArgb(162, 42, 26),
                    Color.FromArgb(161, 39, 26),
                    Color.FromArgb(160, 37, 27),
                    Color.FromArgb(158, 35, 27),
                    Color.FromArgb(157, 33, 27),
                    Color.FromArgb(156, 30, 27),
                    Color.FromArgb(155, 28, 27),
                    Color.FromArgb(153, 26, 27),
                    Color.FromArgb(152, 24, 28),
                    Color.FromArgb(151, 22, 28),
                    Color.FromArgb(149, 19, 28),
                    Color.FromArgb(148, 17, 28),
                    Color.FromArgb(147, 15, 28),
                    Color.FromArgb(146, 13, 28),

        };
        /// <summary>
        /// 图片透明度
        /// </summary>
        private Int32 _Transparency = 255;
        /// <summary>
        /// 图片透明度
        /// </summary>
        public Int32 Transparency
        {
            get { return _Transparency; }
            set { _Transparency = value; }
        }

        private InputImageData _inputData;
        public InputImageData inputData
        {
            get { return _inputData; }
            set
            {
                _inputData = value;
                _inputData.ReSet();
                /*if (_inputData == null)
                {
                    _inputData = value;
                    _inputData.ReSet();
                    _inputData.different = true;

                }
                else if (_inputData.Changed(value))
                {
                    _inputData = value;
                    _inputData.ReSet();
                    _inputData.different = true;

                }
                _inputData.different = false;*/

            }
        }

        private OutputBmp _outBmp;
        public OutputBmp outBmp
        {
            get { return _outBmp; }
            set
            {
                _outBmp = value;
                _outBmp.ReSet();
                /* if (_outBmp == null)
                 {
                     _outBmp = value;
                     _outBmp.ReSet();
                     _inputData.stepBmp = _outBmp.stepBmp;
                     _inputData.minBmp = _outBmp.minBmp;
                     _inputData.ReSet();
                     _outBmp.different = true;
                 }
                 else if (_outBmp.Changed(value))
                 {
                     _outBmp = value;
                     _outBmp.ReSet();

                     if (_outBmp.stepBmp != _inputData.stepBmp |
                         _inputData.minBmp != _outBmp.minBmp)
                     {
                         _inputData.stepBmp = _outBmp.stepBmp;
                         _inputData.minBmp = _outBmp.minBmp;
                         _inputData.ReSet();
                     }
                     _outBmp.different = true;
                 }
                 else
                     _outBmp.different = false;*/


            }
        }

        public Bitmap bmp;
        PointBitmap pb;
        #endregion
    }

    public class ImageCommentUT
    {
        #region 全局量

        /// <summary>
        /// 256色色谱，ULTRAVISION
        /// </summary>
        private Color[] colormap = new Color[256] {
                    Color.FromArgb(255, 255, 255),
                    Color.FromArgb(251, 253, 255),
                    Color.FromArgb(247, 250, 254),
                    Color.FromArgb(243, 248, 254),
                    Color.FromArgb(239, 245, 253),
                    Color.FromArgb(235, 243, 252),
                    Color.FromArgb(231, 240, 252),
                    Color.FromArgb(226, 238, 251),
                    Color.FromArgb(222, 235, 250),
                    Color.FromArgb(218, 233, 250),
                    Color.FromArgb(214, 230, 249),
                    Color.FromArgb(210, 228, 248),
                    Color.FromArgb(206, 225, 248),
                    Color.FromArgb(201, 223, 247),
                    Color.FromArgb(197, 220, 246),
                    Color.FromArgb(193, 218, 246),
                    Color.FromArgb(189, 215, 245),
                    Color.FromArgb(184, 212, 244),
                    Color.FromArgb(180, 210, 244),
                    Color.FromArgb(176, 208, 243),
                    Color.FromArgb(172, 205, 243),
                    Color.FromArgb(168, 203, 242),
                    Color.FromArgb(164, 200, 241),
                    Color.FromArgb(160, 198, 241),
                    Color.FromArgb(155, 195, 240),
                    Color.FromArgb(151, 193, 239),
                    Color.FromArgb(147, 190, 239),
                    Color.FromArgb(143, 188, 238),
                    Color.FromArgb(139, 185, 237),
                    Color.FromArgb(135, 183, 237),
                    Color.FromArgb(130, 181, 236),
                    Color.FromArgb(126, 178, 235),
                    Color.FromArgb(122, 176, 235),
                    Color.FromArgb(118, 173, 234),
                    Color.FromArgb(113, 170, 233),
                    Color.FromArgb(111, 167, 231),
                    Color.FromArgb(108, 163, 228),
                    Color.FromArgb(105, 159, 226),
                    Color.FromArgb(102, 155, 223),
                    Color.FromArgb(99, 151, 221),
                    Color.FromArgb(96, 148, 218),
                    Color.FromArgb(93, 144, 216),
                    Color.FromArgb(90, 140, 213),
                    Color.FromArgb(87, 136, 211),
                    Color.FromArgb(84, 132, 208),
                    Color.FromArgb(81, 129, 206),
                    Color.FromArgb(78, 125, 203),
                    Color.FromArgb(75, 121, 201),
                    Color.FromArgb(72, 117, 198),
                    Color.FromArgb(69, 113, 196),
                    Color.FromArgb(66, 110, 193),
                    Color.FromArgb(62, 105, 190),
                    Color.FromArgb(60, 102, 188),
                    Color.FromArgb(57, 98, 185),
                    Color.FromArgb(54, 94, 182),
                    Color.FromArgb(51, 90, 179),
                    Color.FromArgb(48, 86, 177),
                    Color.FromArgb(46, 82, 174),
                    Color.FromArgb(43, 78, 171),
                    Color.FromArgb(40, 74, 168),
                    Color.FromArgb(37, 70, 166),
                    Color.FromArgb(34, 66, 163),
                    Color.FromArgb(32, 62, 160),
                    Color.FromArgb(29, 58, 157),
                    Color.FromArgb(26, 54, 155),
                    Color.FromArgb(23, 50, 152),
                    Color.FromArgb(20, 46, 149),
                    Color.FromArgb(18, 42, 146),
                    Color.FromArgb(14, 37, 143),
                    Color.FromArgb(14, 39, 143),
                    Color.FromArgb(15, 41, 142),
                    Color.FromArgb(16, 43, 141),
                    Color.FromArgb(17, 45, 140),
                    Color.FromArgb(17, 47, 139),
                    Color.FromArgb(18, 49, 139),
                    Color.FromArgb(19, 51, 138),
                    Color.FromArgb(20, 53, 137),
                    Color.FromArgb(20, 55, 136),
                    Color.FromArgb(21, 57, 135),
                    Color.FromArgb(22, 59, 134),
                    Color.FromArgb(23, 61, 134),
                    Color.FromArgb(23, 63, 133),
                    Color.FromArgb(24, 65, 132),
                    Color.FromArgb(25, 67, 131),
                    Color.FromArgb(26, 69, 130),
                    Color.FromArgb(27, 72, 129),
                    Color.FromArgb(28, 75, 129),
                    Color.FromArgb(30, 79, 129),
                    Color.FromArgb(32, 83, 129),
                    Color.FromArgb(34, 87, 129),
                    Color.FromArgb(36, 91, 129),
                    Color.FromArgb(38, 95, 129),
                    Color.FromArgb(40, 99, 129),
                    Color.FromArgb(41, 103, 129),
                    Color.FromArgb(43, 107, 128),
                    Color.FromArgb(45, 111, 128),
                    Color.FromArgb(47, 115, 128),
                    Color.FromArgb(49, 119, 128),
                    Color.FromArgb(51, 123, 128),
                    Color.FromArgb(53, 127, 128),
                    Color.FromArgb(55, 131, 128),
                    Color.FromArgb(56, 135, 128),
                    Color.FromArgb(59, 140, 127),
                    Color.FromArgb(62, 142, 126),
                    Color.FromArgb(66, 145, 124),
                    Color.FromArgb(70, 148, 122),
                    Color.FromArgb(74, 151, 120),
                    Color.FromArgb(78, 153, 118),
                    Color.FromArgb(82, 156, 116),
                    Color.FromArgb(86, 159, 114),
                    Color.FromArgb(90, 162, 112),
                    Color.FromArgb(94, 164, 110),
                    Color.FromArgb(98, 167, 108),
                    Color.FromArgb(102, 170, 106),
                    Color.FromArgb(106, 173, 104),
                    Color.FromArgb(110, 175, 102),
                    Color.FromArgb(113, 178, 100),
                    Color.FromArgb(117, 181, 98),
                    Color.FromArgb(121, 184, 97),
                    Color.FromArgb(126, 187, 94),
                    Color.FromArgb(130, 189, 92),
                    Color.FromArgb(135, 191, 89),
                    Color.FromArgb(140, 193, 86),
                    Color.FromArgb(145, 195, 83),
                    Color.FromArgb(150, 197, 80),
                    Color.FromArgb(155, 199, 77),
                    Color.FromArgb(160, 201, 74),
                    Color.FromArgb(165, 203, 72),
                    Color.FromArgb(170, 205, 69),
                    Color.FromArgb(175, 208, 66),
                    Color.FromArgb(180, 210, 63),
                    Color.FromArgb(185, 212, 60),
                    Color.FromArgb(190, 214, 57),
                    Color.FromArgb(195, 216, 54),
                    Color.FromArgb(200, 218, 51),
                    Color.FromArgb(205, 220, 49),
                    Color.FromArgb(211, 223, 45),
                    Color.FromArgb(212, 223, 45),
                    Color.FromArgb(214, 222, 45),
                    Color.FromArgb(216, 221, 45),
                    Color.FromArgb(218, 221, 45),
                    Color.FromArgb(219, 220, 45),
                    Color.FromArgb(221, 219, 45),
                    Color.FromArgb(223, 219, 45),
                    Color.FromArgb(225, 218, 45),
                    Color.FromArgb(226, 217, 44),
                    Color.FromArgb(228, 216, 44),
                    Color.FromArgb(230, 216, 44),
                    Color.FromArgb(232, 215, 44),
                    Color.FromArgb(233, 214, 44),
                    Color.FromArgb(235, 214, 44),
                    Color.FromArgb(237, 213, 44),
                    Color.FromArgb(239, 212, 44),
                    Color.FromArgb(241, 211, 43),
                    Color.FromArgb(240, 208, 45),
                    Color.FromArgb(239, 205, 47),
                    Color.FromArgb(238, 202, 49),
                    Color.FromArgb(237, 199, 51),
                    Color.FromArgb(236, 195, 53),
                    Color.FromArgb(235, 192, 56),
                    Color.FromArgb(234, 189, 58),
                    Color.FromArgb(233, 186, 60),
                    Color.FromArgb(231, 182, 62),
                    Color.FromArgb(230, 179, 64),
                    Color.FromArgb(229, 176, 66),
                    Color.FromArgb(228, 173, 69),
                    Color.FromArgb(227, 170, 71),
                    Color.FromArgb(226, 166, 73),
                    Color.FromArgb(225, 163, 75),
                    Color.FromArgb(224, 160, 77),
                    Color.FromArgb(222, 156, 80),
                    Color.FromArgb(222, 154, 80),
                    Color.FromArgb(221, 152, 80),
                    Color.FromArgb(220, 150, 81),
                    Color.FromArgb(219, 148, 81),
                    Color.FromArgb(219, 146, 82),
                    Color.FromArgb(218, 144, 82),
                    Color.FromArgb(217, 142, 82),
                    Color.FromArgb(216, 140, 83),
                    Color.FromArgb(216, 138, 83),
                    Color.FromArgb(215, 136, 84),
                    Color.FromArgb(214, 134, 84),
                    Color.FromArgb(213, 132, 84),
                    Color.FromArgb(213, 130, 85),
                    Color.FromArgb(212, 128, 85),
                    Color.FromArgb(211, 126, 86),
                    Color.FromArgb(210, 124, 86),
                    Color.FromArgb(209, 121, 87),
                    Color.FromArgb(209, 121, 85),
                    Color.FromArgb(209, 121, 83),
                    Color.FromArgb(209, 121, 81),
                    Color.FromArgb(209, 120, 79),
                    Color.FromArgb(208, 120, 76),
                    Color.FromArgb(208, 120, 74),
                    Color.FromArgb(208, 119, 72),
                    Color.FromArgb(208, 119, 70),
                    Color.FromArgb(207, 119, 67),
                    Color.FromArgb(207, 119, 65),
                    Color.FromArgb(207, 118, 63),
                    Color.FromArgb(207, 118, 61),
                    Color.FromArgb(206, 118, 59),
                    Color.FromArgb(206, 117, 56),
                    Color.FromArgb(206, 117, 54),
                    Color.FromArgb(206, 117, 52),
                    Color.FromArgb(205, 116, 49),
                    Color.FromArgb(205, 115, 48),
                    Color.FromArgb(204, 114, 46),
                    Color.FromArgb(204, 113, 45),
                    Color.FromArgb(203, 112, 43),
                    Color.FromArgb(202, 111, 42),
                    Color.FromArgb(202, 110, 40),
                    Color.FromArgb(201, 109, 39),
                    Color.FromArgb(200, 108, 37),
                    Color.FromArgb(200, 107, 36),
                    Color.FromArgb(199, 106, 34),
                    Color.FromArgb(198, 105, 33),
                    Color.FromArgb(198, 104, 31),
                    Color.FromArgb(197, 103, 30),
                    Color.FromArgb(196, 102, 28),
                    Color.FromArgb(196, 101, 27),
                    Color.FromArgb(195, 100, 25),
                    Color.FromArgb(194, 98, 23),
                    Color.FromArgb(193, 96, 23),
                    Color.FromArgb(191, 93, 23),
                    Color.FromArgb(190, 90, 23),
                    Color.FromArgb(188, 87, 23),
                    Color.FromArgb(187, 84, 23),
                    Color.FromArgb(185, 82, 24),
                    Color.FromArgb(183, 79, 24),
                    Color.FromArgb(182, 76, 24),
                    Color.FromArgb(180, 73, 24),
                    Color.FromArgb(179, 70, 24),
                    Color.FromArgb(177, 68, 24),
                    Color.FromArgb(176, 65, 25),
                    Color.FromArgb(174, 62, 25),
                    Color.FromArgb(172, 59, 25),
                    Color.FromArgb(171, 56, 25),
                    Color.FromArgb(169, 53, 25),
                    Color.FromArgb(167, 50, 26),
                    Color.FromArgb(166, 48, 26),
                    Color.FromArgb(165, 46, 26),
                    Color.FromArgb(164, 44, 26),
                    Color.FromArgb(162, 42, 26),
                    Color.FromArgb(161, 39, 26),
                    Color.FromArgb(160, 37, 27),
                    Color.FromArgb(158, 35, 27),
                    Color.FromArgb(157, 33, 27),
                    Color.FromArgb(156, 30, 27),
                    Color.FromArgb(155, 28, 27),
                    Color.FromArgb(153, 26, 27),
                    Color.FromArgb(152, 24, 28),
                    Color.FromArgb(151, 22, 28),
                    Color.FromArgb(149, 19, 28),
                    Color.FromArgb(148, 17, 28),
                    Color.FromArgb(147, 15, 28),
                    Color.FromArgb(146, 13, 28),

        };
        /// <summary>
        /// 图片透明度
        /// </summary>
        private Int32 _Transparency = 255;
        /// <summary>
        /// 图片透明度
        /// </summary>
        public Int32 Transparency
        {
            get { return _Transparency; }
            set { _Transparency = value; }
        }
        private float _ZeroColRatio = 0;
        public float ZeroColRatio
        {
            get { return _ZeroColRatio; }
            set { _ZeroColRatio = value;
                if (_outBmp != null)
                    ResetPara();
            }
        }
        public float[] _DisplayZoneMin = new float[2]{0,0};
        public float[] _DisplayZoneMax = new float[2] { 1, 1};
        private float width0;
        private float height0;

        private InputImageData _inputData;
        public InputImageData inputData
        {
            get { return _inputData; }
            set
            {
                _inputData = value;
                _inputData.ReSet();
            }
        }

        private OutputBmp _outBmp;
        public OutputBmp outBmp
        {
            get { return _outBmp; }
            set
            {
                _outBmp = value;
                _outBmp.ReSet();
                ResetPara();
            }
        }

        public Bitmap bmp;
        PointBitmap pb;
        #endregion
              

        /// <summary>
        /// 刷新位图
        /// </summary>
        public void RefreshBMP()
        {
            ResetPara();
            _outBmp.ReSet();
            pb = new PointBitmap(_outBmp.bmp);
            pb.LockBits();
            uint[,] index = new uint[_outBmp.height, _outBmp.width];
            long t0 = 0;

            t0 = System.Environment.TickCount;
            //for (int i = 0; i < _outBmp.width; i++)
            Parallel.For(0, _outBmp.width, i =>
              {
                  for (int j = 0; j < _outBmp.height; j++)
                  {
                      index[j, i] = GetColorIndex(i, j);
                  }
              });
            //Console.WriteLine((Environment.TickCount-t0).ToString());
            for (int i = 0; i < _outBmp.width; i++)
                for (int j = 0; j < _outBmp.height; j++)
                {
                    pb.SetPixel(i, j, Color.FromArgb(Transparency, colormap[index[j, i]]));
                }

            pb.UnlockBits();
            bmp = _outBmp.bmp;
        }

        /// <summary>
        /// 计算像素点位置处的颜色索引
        /// </summary>
        /// <param name="px">像素点坐标x</param>
        /// <param name="py">像素点坐标y</param>
        /// <param name="row">bmpData行数</param>
        /// <param name="col">bmpData列数</param>
        /// <returns>素点位置处的颜色索引</returns>
        public uint GetColorIndex(int px, int py)
        {
            float stepx, stepy;
            float matc = _inputData.col / (1- Math.Abs(_ZeroColRatio));
            float matr = _inputData.row;

            stepx = matc / (width0);
            stepy = matr / (height0);

            //像素起点
            float x0 = _DisplayZoneMin[0] * width0;
            float y0 = _DisplayZoneMin[1] * height0;
            //数据矩阵点
            float tpy = (py+ y0) * stepy  ;
            float tpy0 = Math.Max(tpy - stepy,0);
            float tpx = (px+ x0) * stepx  ;
            float tpx0 = Math.Max(tpx - stepx, 0);
            float tmpc = matc * Math.Abs(_ZeroColRatio);                        

            int subMatrixRow0 = (int)(tpy0);
            int subMatrixRow1 = (int)(tpy);            
            int subMatrixCol0 = (int)(tpx0);
            int subMatrixCol1 = (int)(tpx);

            int sr = subMatrixRow1 - subMatrixRow0;
            int sc = subMatrixCol1 - subMatrixCol0;
            uint maxMatrix = 0;
            uint tmp;
            int rr,rc;
            float minc;
            for (int i = 0; i <= sr; i++)
            {
                rr = i + subMatrixRow0;

                if (_ZeroColRatio > 0)
                    minc = tmpc * (matr - rr) / matr;
                else
                    minc = tmpc * rr / matr;

                for (int j = 0; j <= sc; j++)
                {
                    rc = (int)Math.Round(j + subMatrixCol0 - minc);
                    if (rc > 0 & rc < _inputData.col)
                        tmp = _inputData.matrix[rr, rc];     
                    else
                        tmp = 0;

                    if (tmp > maxMatrix)
                    {
                        maxMatrix = tmp;
                    }
                }
            }
            return maxMatrix;
        }

        /// <summary>
        /// 计算像素点位置处的颜色索引
        /// </summary>
        /// <param name="px">像素点坐标x</param>
        /// <param name="py">像素点坐标y</param>
        /// <param name="row">bmpData行数</param>
        /// <param name="col">bmpData列数</param>
        /// <returns>素点位置处的颜色索引</returns>
        public uint GetColorIndex(int px, int py, ref int matrixR, ref int matrixC, ref int DataIndex)
        {
            float stepx, stepy;
            float matc = _inputData.col / (1 - Math.Abs(_ZeroColRatio));
            float matr = _inputData.row;

            stepx = matc / (width0);
            stepy = matr / (height0);

            //像素起点
            float x0 = _DisplayZoneMin[0] * width0;
            float y0 = _DisplayZoneMin[1] * height0;
            //数据矩阵点
            float tpy = (py + y0) * stepy;
            float tpy0 = Math.Max(tpy - stepy, 0);
            float tpx = (px + x0) * stepx;
            float tpx0 = Math.Max(tpx - stepx, 0);
            float tmpc = matc * Math.Abs(_ZeroColRatio);

            int subMatrixRow0 = (int)(tpy0);
            int subMatrixRow1 = (int)(tpy);
            int subMatrixCol0 = (int)(tpx0);
            int subMatrixCol1 = (int)(tpx);

            int sr = subMatrixRow1 - subMatrixRow0;
            int sc = subMatrixCol1 - subMatrixCol0;
            uint maxMatrix = 0;
            uint tmp;
            int rr, rc;
            float minc;
            for (int i = 0; i <= sr; i++)
            {
                rr = i + subMatrixRow0;

                if (_ZeroColRatio > 0)
                    minc = tmpc * (matr - rr) / matr;
                else
                    minc = tmpc * rr / matr;

                for (int j = 0; j <= sc; j++)
                {
                    rc = (int)(j + subMatrixCol0 - minc);
                    if (rc > 0 & rc < _inputData.col)
                    {
                        tmp = _inputData.matrix[rr, rc];
                        matrixR = rr;
                        matrixC = rc;
                    }

                    else
                        tmp = 0;

                    if (tmp > maxMatrix)
                    {
                        maxMatrix = tmp;
                        matrixR = rr;
                        matrixC = rc;
                    }
                }
                DataIndex = matrixR * _inputData.col + matrixC;
            }
            return maxMatrix;
        }

        /// <summary>
        /// 重计算公共参数
        /// </summary>
        public void ResetPara()
        {
            width0 = _outBmp.width / (_DisplayZoneMax[0] - _DisplayZoneMin[0]);
            height0 = _outBmp.height / (_DisplayZoneMax[1] - _DisplayZoneMin[1]);
            
        }
    }

    public class series
    {
        public float[] x;
        public float[] y;
        public Point2Dim[] s;
        public Color sColor = Color.Blue;
        public int Count;

        public series()
        {

        }

        public void CopyTo(series a)
        {
            a.Count = Count;
            a.x = new float[Count];
            a.y = new float[Count];
            a.s = new Point2Dim[Count];
            x.CopyTo(a.x, 0);
            y.CopyTo(a.y, 0);
            s.CopyTo(a.s, 0);
            a.sColor = sColor;
            
        }

        public series(int length)
        {
            Count = length;
            x = new float[length];
            y = new float[length];
            s = new Point2Dim[length];
        }

        public series(float[] a, float[] b)
        {
            x = a;
            y = b;
            Count = a.Count();
            s = new Point2Dim[Count];
            for (int i = 0; i < Count; i++)
                s[i] = new Point2Dim(x[i], y[i]);
        }

        public series(float[] a)
        {
            int num = a.Count();
            y = a;
            x = new float[num];
            for (int i = 0; i < num; i++)
            {
                x[i] = i;
            }
            Count = a.Count();
            s = new Point2Dim[Count];
            for (int i = 0; i < Count; i++)
                s[i] = new Point2Dim(x[i], y[i]);
        }

        public series(float[] a, Color c)
        {
            series t = new series(a);
            x = t.x;
            y = t.y;
            sColor = c;
        }

        public series(float[] a, float[] b, Color c)
        {
            series t = new series(a, b);
            x = t.x;
            y = t.y;
            sColor = c;
        }
    }

    public class Plot2D
    {        
        private List<series> _rawData;
        public List<series> rawData
        {
            get { return _rawData; }
            set { _rawData = value; }
        }
           
        public Bitmap bmp;
        /// <summary>
        /// 显示的最小最大数据值
        /// </summary>
        public float x0, x1, y0, y1;
        public bool FixRange = false;

        private Graphics g;
        /// <summary>
        /// 全局的缩放起始点（比例值）
        /// </summary>
        public float[] DisplayZoneMin = new float[2] { 0, 0 };
        /// <summary>
        /// 全局的缩放结束点（比例值）
        /// </summary>
        public float[] DisplayZoneMax = new float[2] { 1, 1 };
        public int ControlWidth;
        public int ControlHeight;
        private List<bool[]> inG;
        /// <summary>
        /// 全局的显示起点(像素)
        /// </summary>
        private Point2Dim DisPlayZoneMinP = new Point2Dim(0,0);
        /// <summary>
        /// 全局的显示终点
        /// </summary>
        private Point2Dim DisPlayZoneMaxP = new Point2Dim(1, 1);
        /// <summary>
        /// 换算的显示宽度和高度(像素)
        /// ，大于等于ControlWidth，ControlHeight
        /// </summary>
        private float width0,height0;
        /// <summary>
        /// x1-x0;y1-y0
        /// </summary>
        public float xw,yh;
        public int seriesNum;
        private List<series> pixelData;

        public void Refresh()
        {
            ResetPara();
            GetDataRange();
            ResetPixelData();

            for (int i = 0; i < seriesNum; i++)            //Parallel.For(0,seriesNum,i=>
                DrawSeries(pixelData[i], inG[i]);
            


        }

        /// <summary>
        /// 以下操作需ResetPara:
        /// 缩放；
        /// 控件尺寸变化；
        /// 原始数据变化
        /// </summary>
        public void ResetPara()
        {
            if (bmp != null)
                bmp.Dispose();
            bmp = new Bitmap(ControlWidth, ControlHeight);
            g = Graphics.FromImage(bmp);
            width0 = ControlWidth / (DisplayZoneMax[0] - DisplayZoneMin[0]);
            height0 = ControlHeight / (DisplayZoneMax[1] - DisplayZoneMin[1]);
            seriesNum = rawData.Count();
            DisPlayZoneMinP.x = width0 * DisplayZoneMin[0];
            DisPlayZoneMinP.y = height0 * DisplayZoneMin[1];
            DisPlayZoneMaxP.x = width0 * DisplayZoneMax[0];
            DisPlayZoneMaxP.y = height0 * DisplayZoneMax[1];

            if (inG != null)
                inG.Clear();
            else
                inG = new List<bool[]>(seriesNum);
        }        

        /// <summary>
        /// 当前数据范围，原始数据变化是需进行该操作
        /// </summary>
        public void GetDataRange()
        {
            if (!FixRange)
            {
                x0 = 0; x1 = 0; y0 = 0; y1 = 0;
                float xmin, xmax, ymin, ymax;
                for (int i = 0; i < seriesNum; i++)
                {
                    xmin = _rawData[i].x.Min();
                    xmax = _rawData[i].x.Max();
                    ymin = _rawData[i].y.Min();
                    ymax = _rawData[i].y.Max();
                    x0 = (float)(x0 > xmin ? xmin : x0);
                    x1 = (float)(x1 < xmax ? xmax : x1);
                    y0 = (float)(y0 > ymin ? ymin : y0);
                    y1 = (float)(y1 < ymax ? ymax : y1);
                }
            }
            xw = x1 - x0;
            yh = y1 - y0;
        }

        /// <summary>
        /// 画线
        /// </summary>
        /// <param name="c"></param>
        /// <param name="p"></param>
        public void DrawLine(Color c, Point2Dim p0, Point2Dim p1)
        {
            Pen newPen = new Pen(c);//定义一个画笔
            
            g.DrawLine(newPen, Point2ToPoint(p0), Point2ToPoint(p1));//绘制直线
        }

        /// <summary>
        /// 画线
        /// </summary>
        /// <param name="c"></param>
        /// <param name="p"></param>
        public void DrawLine(Pen c, Point p0, Point p1)
        {
            if (p0 == p1)
                return;           

            g.DrawLine(c, p0, p1);//绘制直线
        }

        /// <summary>
        /// 在图框内画线段
        /// </summary>
        /// <param name="c">线段颜色</param>
        /// <param name="p">线段端点坐标</param>
        /// <param name="isin">线段端点是否在图框内</param>
        private bool GetPointInGraphic(Color c, Point2Dim[] p, bool[] isin, 
            ref Point2Dim p0, ref Point2Dim p1)
        {
            p0 = new Point2Dim(0, 0);
            p1 = new Point2Dim(0, 0);
            bool existCP = isin[0] & isin[1];
            if (existCP)
            {
                p0 = p[0] - DisPlayZoneMinP;
                p1 = p[1] - DisPlayZoneMinP;                
            }
            else
            {
                bool justOneCross = isin[0]|isin[1];
                Point2Dim[] cp;
                existCP = LineCrossRectanle(DisPlayZoneMinP, DisPlayZoneMaxP, 
                    p[0], p[1], justOneCross, out cp);
                if (justOneCross)
                {
                    if (isin[0])
                    {
                        p0 = p[0] - DisPlayZoneMinP;
                        p1 = cp[0] - DisPlayZoneMinP;
                    }
                    else
                    {
                        p0 = cp[0] - DisPlayZoneMinP;
                        p1 = p[1] - DisPlayZoneMinP;
                    }
                }
                else if (existCP)
                {
                    p0 = cp[0] - DisPlayZoneMinP;
                    p1 = cp[1] - DisPlayZoneMinP;
                }                
            }
            return existCP;
        }

        /// <summary>
        /// 画一条曲线
        /// </summary>
        /// <param name="p"></param>
        private void DrawSeries(series s, bool[] isin)
        {
            int num = s.Count-1;            

            Point2Dim[] CurrentSeriersP = new Point2Dim[num*2];
            bool[] tmpb = new bool[num];
            Point2Dim p0, p1;
            //Parallel.For(0, num, i =>
            for(int i = 0; i < num; i++)
            {
                
                  p0 = new Point2Dim(s.x[i], s.y[i]);
                  p1 = new Point2Dim(s.x[i + 1], s.y[i + 1]);
                  tmpb[i] = GetPointInGraphic(s.sColor,
                      new Point2Dim[2] { p0, p1 },
                      new bool[2] { isin[i], isin[i + 1] },
                      ref CurrentSeriersP[i * 2],
                      ref CurrentSeriersP[i * 2 + 1]);

              }

            DrawMultiLine(s.sColor, CurrentSeriersP, tmpb);
        }

        /// <summary>
        /// 计算临近点及相应数据点
        /// </summary>
        /// <param name="x">鼠标当前x像素坐标</param>
        /// <param name="pointX">临近点x像素坐标</param>
        /// <param name="pointY">临近点y像素坐标,超出显示范围取值-1</param>
        /// <returns></returns>
        public List<float[]> PixelToData(int x, out int pointX, out List<int> pointY)
        {
            List<float[]> result = new List<float[]>(seriesNum);
            List<int> tmp = new List<int>(seriesNum);
            float px = (x + DisPlayZoneMinP.x) / width0 * xw + x0;
            List<float> allX = new List<float>(seriesNum * 100000);
            for (int i = 0; i < seriesNum; i++)
            {
                for (int j = 0; j < _rawData[i].Count; j++)
                    allX.Add(_rawData[i].x[j]);
            }
            int ox = FindNearPointIndex(allX.ToArray(), px);
            float odatax = allX[ox];
            pointX = (int)Math.Round( width0 * (odatax - x0) / xw - DisPlayZoneMinP.x);
            pointY = new List<int>(seriesNum);
            for (int i = 0; i < seriesNum; i++)
            {
                int m = _rawData[i].x.ToList().FindIndex(s => s == odatax);
                if (m == -1)
                {
                    result.Add(null);
                    pointY.Add(-1);
                }
                else
                {
                    float odatay = _rawData[i].y[m];
                    result.Add(new float[] { odatax, odatay });
                    int pixelY = (int)Math.Round(ControlHeight - (odatay - y0) / yh * ControlHeight);
                    if (pixelY > ControlHeight || pixelY < 0)
                        pixelY = -1;
                    pointY.Add(pixelY);
                }
            }
            return result;
        }

        /// <summary>
        /// 数组中当前位置的临近点
        /// </summary>
        /// <param name="x">数组</param>
        /// <param name="x0">当前位置</param>
        /// <returns></returns>
        private int FindNearPointIndex(float[] x , float x0)
        {
            List<float> y = x.ToList();
            y.Sort();
            int m = y.FindIndex(s => s >= x0);
            int tx = 0;
            if (m > 0)            
                tx = (y[m] - x0 >= x0 - y[m - 1]) ? m - 1 : m;
            return x.ToList().FindIndex(s => s == y[tx]);
        }

        /// <summary>
        /// 数组中当前位置的临近点
        /// </summary>
        /// <param name="x">数组</param>
        /// <param name="x0">当前位置</param>
        /// <returns></returns>
        private int FindNearPointIndex(int[] x, int x0)
        {
            List<int> y = x.ToList();
            y.Sort();
            int m = y.FindIndex(s => s >= x0);
            int tx = 0;
            if (m != 0)
                tx = (y[m] - x0 >= x0 - y[m - 1]) ? m - 1 : m;
            return x.ToList().FindIndex(s => s == y[tx]);
        }

        private void DrawMultiLine(Color c, Point2Dim[] sp, bool[] draw)
        {
            int m = draw.Count();
            int n = 0;
            for (int i = 0; i < m; i++)
                if (draw[i])
                    n += 2;

            Point2Dim[] nesp = new Point2Dim[n];
            if (n == m*2)
                nesp = sp;
            else
            {
                int tk = 0;
                for (int i = 0; i < m; i++)
                    if (draw[i])
                    {
                        nesp[tk * 2] = sp[i * 2];
                        nesp[tk * 2 + 1] = sp[i * 2 + 1];
                        tk++;
                    }
            }

            Point[] nsp = new Point[n];
            Parallel.For(0,n,i=>
                nsp[i] = Point2ToPoint(nesp[i]));
            /*for (int i = 0; i < n; i++)
                nsp[i] = Point2ToPoint(nesp[i]);*/

            Pen newPen = new Pen(c);//定义一个画笔
            /*g.DrawLines(newPen, nsp);*/
            List<int> tmp = new List<int>(10000);        
            tmp.Add(nsp[0].Y);            
            for (int i = 1; i < n; i++)
            {
                if (nsp[i - 1].X == nsp[i].X)
                    tmp.Add(nsp[i].Y);
                else
                {
                    DrawLine(newPen, new Point(nsp[i - 1].X, tmp.Min()), new Point(nsp[i - 1].X, tmp.Max()));
                    DrawLine(newPen, nsp[i - 1], nsp[i]);
                    tmp.Clear();
                    tmp.Add(nsp[i].Y);
                }
                
            }


        }

        /// <summary>
        /// 重新计算所有的数据点
        /// </summary>
        private void ResetPixelData()
        {
            if (pixelData == null)
                pixelData = new List<series>(seriesNum);
            else
                pixelData.Clear();

            Point2Dim tmp = new Point2Dim();
            series ds;
            series ts ;
            for (int i = 0; i < seriesNum; i++)
            {
                ds = _rawData[i];
                int num = ds.x.Length;
                ts = new series(num);
                inG.Add( new bool[num]);
                for (int j = 0; j < num; j++)
                {
                    inG[i][j] =
                    DataInCurrentDisArea(new float[] { ds.x[j], ds.y[j] }, ref tmp);
                    ts.x[j] = tmp.x;
                    ts.y[j] = tmp.y;
                }
                ts.sColor = ds.sColor;
                pixelData.Add(ts);
            }
        }

        /// <summary>
        /// 判断数据点是否在当前显示范围
        /// </summary>
        /// <param name="Data">原始数据点</param>
        /// <param name="pixel">当前坐标点</param>
        /// <returns></returns>
        private bool DataInCurrentDisArea(float[] Data, ref Point2Dim pixel)
        {
            Point2Dim tmp = new Point2Dim();
            tmp.x = width0 * (Data[0] - x0) / xw;
            tmp.y = height0 - height0 * (Data[1] - y0) / yh;

            pixel = tmp;

            if (pixel.x >= DisPlayZoneMinP.x &
                pixel.x <= DisPlayZoneMaxP.x &
                pixel.y >= DisPlayZoneMinP.y &
                pixel.y <= DisPlayZoneMaxP.y
            )
            {
                return true;
            }
            else
                return false;

        }

        /// <summary>
        /// 线段与矩形框的焦点计算
        /// </summary>
        /// <param name="minp">矩形框最小坐标点</param>
        /// <param name="maxp">矩形框最大坐标点</param>
        /// <param name="b0">线段起点</param>
        /// <param name="b1">线段终点</param>
        /// <param name="JustOneCross">是否肯定只有一个交点</param>
        /// <param name="CrossPoint">反馈的交点值</param>
        /// <returns>是否存在交点</returns>
        private bool LineCrossRectanle(Point2Dim minp, Point2Dim maxp, Point2Dim b0, Point2Dim b1, bool JustOneCross,out Point2Dim[] CrossPoint)
        {
            bool result = false;
            CrossPoint = new Point2Dim[2];
            int m = 2;
            if (JustOneCross)
            {
                result = true;
                m = 1;
            }
            Point2Dim cp = new Point2Dim();
            minp.Clone(out Point2Dim a0);
            maxp.Clone(out Point2Dim a2);
            Point2Dim a1 = new Point2Dim(maxp.x, minp.y);
            Point2Dim a3 = new Point2Dim(minp.x, maxp.y);
            
            int k = 0;
            if (SpecialLineCrossPoint(a0, a1, b0, b1, ref cp) & k < m)
            {
                result = true;
                cp.Clone(out CrossPoint[k++]);
            }
            if (SpecialLineCrossPoint(a1, a2, b0, b1, ref cp) & k < m)
            {
                result = true;
                cp.Clone(out CrossPoint[k++]);
            }
            if (SpecialLineCrossPoint(a2, a3, b0, b1, ref cp) & k < m)
            {
                result = true;
                cp.Clone(out CrossPoint[k++]);
            }
            if (SpecialLineCrossPoint(a3, a0, b0, b1, ref cp) & k < m)
            {
                result = true;
                cp.Clone(out CrossPoint[k++]);
            }

            return result;
                
        }

        /// <summary>
        /// 目标线段与水平或竖直线段的交点计算
        /// </summary>
        /// <param name="a0">水平或竖直线段的起点</param>
        /// <param name="a1">水平或竖直线段的终点</param>
        /// <param name="b0">目标线段的起点</param>
        /// <param name="b1">目标线段的终点</param>
        /// <param name="CrossPoint">交点</param>
        /// <returns>是否存在交点</returns>
        private bool SpecialLineCrossPoint(Point2Dim a0, Point2Dim a1, Point2Dim b0, Point2Dim b1, ref Point2Dim CrossPoint)
        {
            string style = null;
            if (a0.x == a1.x)
                style = "V";
            else
                style = "H";

            if (style == "V")
            {
                float c0 = b0.x - a0.x;
                float c1 = b1.x - a0.x;
                float min = a0.y >= a1.y ? a1.y : a0.y;
                float max = a0.y <= a1.y ? a1.y : a0.y;
                if (c0*c1<=0)
                {
                    float tmp = c0 / (c0 - c1) * (b1.y-b0.y) + b0.y;
                    if (tmp >= min & tmp <= max)
                    {
                        CrossPoint = new Point2Dim(a0.x, tmp);
                        return true;
                    }
                }
            }
            else
            {
                float c0 = b0.y - a0.y;
                float c1 = b1.y - a0.y;
                float min = a0.x >= a1.x ? a1.x : a0.x;
                float max = a0.x <= a1.x ? a1.x : a0.x;
                if (c0 * c1 <= 0)
                {
                    float tmp = c0 / (c0 - c1) * (b1.x - b0.x) + b0.x;
                    if (tmp >= min & tmp <= max)
                    {
                        CrossPoint = new Point2Dim(tmp, a0.y);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 线段与线段的交点
        /// </summary>
        /// <param name="a0">线段a的起点</param>
        /// <param name="a1">线段a的终点</param>
        /// <param name="b0">线段b的起点</param>
        /// <param name="b1">线段b的终点</param>
        /// <param name="CrossPoint">反馈的交点值</param>
        /// <returns>是否存在交点</returns>
        private bool LineCrossPoint(Point2Dim a0, Point2Dim a1, Point2Dim b0, Point2Dim b1, ref Point2Dim CrossPoint)
        {
            bool result = false;
            Point2Dim NormVA = (a1 - a0).unitVec();
            Point2Dim NormVB = (b1 - b0).unitVec();
            float cos = NormVA.dotMulti(NormVB).sum();
            if (cos != 1)                
            {
                Point2Dim tmp = pointDiv(NormVA, NormVB, b0 - a0);
                CrossPoint = a0 + NormVA * tmp.x;
                if (tmp.x <= (a1 - a0).normVec() &
                    tmp.x >= 0 &
                    tmp.y >= 0 &
                    tmp.y <= (b1 - b0).normVec())
                    result = true;
            }

            return result;
        }
        
        private Point2Dim pointDiv(Point2Dim a, Point2Dim b, Point2Dim c)
        {
            Point2Dim result;
            float mo = a.x * b.y - a.y * b.x;
            Point2Dim inv0 = new Point2Dim(b.y, -b.x);
            Point2Dim inv1 = new Point2Dim(-a.y, a.x);
            result = new Point2Dim(inv0.dotMulti(c).sum() / mo, -inv1.dotMulti(c).sum() / mo);
            return result;
        }

        private Point Point2ToPoint(Point2Dim a)
        {
            return (new Point((int)Math.Round(a.x),
                (int)Math.Round(a.y)));
        }        


        
    }
    

    
}
