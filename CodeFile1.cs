using System.Windows.Forms;
using System;
using System.Collections.Generic;

namespace ZYCControl
{
    class Program
    {
        static void Main(string[] args)
        {
            LsTest(true);
        }
            
        static void LsTest(bool last)
        {
            int num = 36000;
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
            lsf.longStrip1.JudgeLine0Enable = true;
            lsf.longStrip1.JudgeLine0 = 0.5f;
            ///动态画图时，必须fixRange
            lsf.FigureInitial(t, true, new float[4] { x[0], x[num-1], -1, 1 });
            if (!last)
                lsf.Show();
            else
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

        static void rulerTest()
        {
            RulerTest rt = new RulerTest();
            rt.ShowDialog();
        }
        
    }
}
