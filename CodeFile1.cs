using System.Windows.Forms;
using System;
using System.Collections.Generic;

namespace ZYCControl
{
    class Program
    {
        static void Main(string[] args)
        {



            LsTest();
            //float[] res = new float[1024*1024];
            //int tmp = 0;
            //for (int i = 0; i < 1024; i++)
            //    for (int j = 0; j < 1024; j++)
            //        res[tmp++] = i+j;

            //InputImageData inputData = new InputImageData(res, 1024 , 1024);
            //OutputBmp outBmp = new OutputBmp(0, (uint)res[tmp-1]);
            //FigureForm f = new FigureForm();
            //f.IsMdiContainer = true;

            //f.FigureInitial(inputData, outBmp, -0.5f);

            //float[] tmp0 = new float[res.Length];
            //res.CopyTo(tmp0, 0);
            //bool same = true;
            //tmp0[0] = 8;
            //for (int i = 0; i < res.Length; i++)
            //    same &= (res[i] == tmp0[i]);
            //f.ShowDialog();
            //bool[] cc = new bool[361];
            //Point2Dim a0 = new Point2Dim(1, -1);
            //Point2Dim a1 = new Point2Dim(1, 1);
            //Point2Dim b0 = new Point2Dim(0, 0);
            //Point2Dim b1 = new Point2Dim(0, 0);
            //Point2Dim c = new Point2Dim();
            //Plot2D pd = new Plot2D();
            //for (int i = 0; i < 360; i++)
            //{
            //    b1.x = 2 * (float)Math.Cos(i * Math.PI / 180.0);
            //    b1.y = 2 * (float)Math.Sin(i * Math.PI / 180.0);

            //    //cc[i] = pd.SpecialLineCrossPoint(a0, a1, b0, b1, ref c);
            //}
            //int num = 36100;
            //float[] x = new float[num];
            //float[] y = new float[num];
            //float[] z = new float[num];
            //for (int i = 0; i < num; i++)
            //{
            //    x[i] = (float)((i-180)*Math.PI/180.0);
            //    y[i] = (float)(Math.Sin(x[i]));
            //    z[i] = (float)(Math.Cos(x[i]));
            //}
            //series a = new series(x,y);
            //series b = new series(x,z);
            //FigureForm_Plot2D p2 = new FigureForm_Plot2D ();
            //List<series> t = new List<series>();
            //t.Add(a);
            //t.Add(b);
            //p2.FigureInitial(t,false,new float[4] { -5, 500, -1, 100 });
            
            
            //p2.ShowDialog();
            }
            
        static void LsTest()
        {
            int num = 13610;
            float[] x = new float[num];
            float[] y = new float[num];
            float[] z = new float[num];
            for (int i = 0; i < num; i++)
            {
                x[i] = (float)((i - 180) * Math.PI / 180.0);
                y[i] = (float)(Math.Sin(x[i]));
                z[i] = (float)(Math.Cos(x[i]));
            }
            series a = new series(x, y);
            a.sColor = System.Drawing.Color.Red;
            series b = new series(x, z);

            List<series> t = new List<series>();
            t.Add(a);
            t.Add(b);

            LongStripForm lsf = new LongStripForm();
            lsf.longStrip1.transparentInfo1.JudgeLine0Enable = true;
            lsf.longStrip1.transparentInfo1.JudgeLine0 = 0.5f;
            lsf.FigureInitial(t, false, new float[4] { x[0], x[num-1], y[0], y[num - 1] });
            lsf.ShowDialog();
        }

        static void plot2d()
        {
            int num = 161;
            float[] x = new float[num];
            float[] y = new float[num];
            float[] z = new float[num];
            for (int i = 0; i < num; i++)
            {
                x[i] = (float)((i - 180) * Math.PI / 180.0);
                y[i] = (float)(Math.Sin(x[i]));
                z[i] = (float)(Math.Cos(x[i]));
            }
            series a = new series(x, y);
            series b = new series(x, z);
            FigureForm_Plot2D p2 = new FigureForm_Plot2D();
            List<series> t = new List<series>();
            t.Add(a);
            t.Add(b);
            p2.FigureInitial(t, false, new float[4] { -5, 500, -1, 100 });


            p2.ShowDialog();
        }
        
    }
}
