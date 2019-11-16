using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace ZYCControl
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, string> a = new Dictionary<int, string>() { { 1, "1" }, { 5, "5" }, { 3, "3" } };
            Dictionary<int, string> a1 = a.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle();
            LongStripTest();
            //AscanTestt();
            /*int a = 5;
            int b = a;
            a = 10;*/
            //figureTest();
            //Tooltest();
        }
            
        static void LsTest(bool last)
        {
            int num = 360;
            float[] x = new float[num];
            float[] y = new float[num];
            float[] x1 = new float[num * 2];
            float[] z = new float[num*2];
            for (int i = 0; i < num; i++)
            {
                x[i] = (float)((i - 180) * Math.PI / 180.0);
                x1[i] = x[i];
                x1[i + num] = (float)(x[i] + 2 * Math.PI) ;
                y[i] = (float)(Math.Sin(x[i]));
                z[i] = (float)(Math.Cos(x[i]));
                z[i+num] = z[i];
            }
            series a = new series(x, y);
            a.sColor = System.Drawing.Color.Red;
            series b = new series(x1, z);

            List<series> t = new List<series>();
            t.Add(a);
            t.Add(b);

            LongStripForm lsf = new LongStripForm();
            lsf.longStrip1.JudgeLine0Enable = true;
            lsf.longStrip1.JudgeLine0 = 0.5f;
            ///动态画图时，必须fixRange
            lsf.FigureInitial(t, true, new float[4] { x[0], x1[2*num-1], -1, 1 });
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
            //RulerTest rt = new RulerTest();
            //rt.ShowDialog();
        }

        static void AscanTestt()
        {
            int num = 360;
            float[] x = new float[num];
            float[] y = new float[num];
            for (int i = 0; i < num; i++)
            {
                x[i] = (float)((i - 180) );
                y[i] = 0.0f;
            }
            series a = new series(x, y);
            List<series> list = new List<series>() { a};
           
            /*ascan_Ruler1.ascan_1.StepX = 1;
            ascan_Ruler1.ascan_1.StepY = 0.001f;
            ascan_Ruler1.ascan_1.StartX = x[0];
            ascan_Ruler1.ascan_1.EndX = x[num - 1];
            ascan_Ruler1.ascan_1.StartY = -1;
            ascan_Ruler1.ascan_1.EndY = 1;
            ascan_Ruler1.ascan_1.NewImage(list, true, new float[] { x[0], x[num - 1], -1, 1 });
            ascan_Ruler1.ascan_1.ascanToolLayout1.ReferenceC.InfoFormat = "{0:0.000}";
            ascan_Ruler1.ascan_1.ascanToolLayout1.MeasureC.InfoFormat = "{0:0.000}";
            ascan_Ruler1.ascan_1.ascanToolLayout1.ReferenceC.Enable = false;
            

            ascan_Ruler1.ReDrawRuler();

            StartPosition = FormStartPosition.CenterScreen;
            ShowDialog();*/
            
            
        }

        static void figureTest()
        {
            UltraFTest fig = new UltraFTest();
            fig.StartPosition = FormStartPosition.CenterScreen;
            float[] data = new float[1024 * 1024];
            for (int i = 0; i < 1024; i++)
                for (int j = 0; j < 1024; j++)
                    data[i * 1024 + j] = i + j;
            
            InputImageData a = new InputImageData(data, 1024, 1024);
            OutputBmp outputBmp = new OutputBmp(0,1024*2);
            fig.ultravisionFieldRB.ultravisionField1.inputData = a;
            fig.ultravisionFieldRB.ultravisionField1.outBmp = outputBmp;
            fig.ultravisionFieldRB.ultravisionField1.zeroRatio = 0;
            ///xgap,ygap需写在前面
            fig.ultravisionFieldRB.ultravisionField1.xGap = 1;
            fig.ultravisionFieldRB.ultravisionField1.yGap = 1;
            fig.ultravisionFieldRB.ultravisionField1.startX = 0;
            fig.ultravisionFieldRB.ultravisionField1.endX = 1023;
            fig.ultravisionFieldRB.ultravisionField1.startY = 0;
            fig.ultravisionFieldRB.ultravisionField1.endY = 1023;

            fig.ultravisionFieldRB.ultravisionField1.NewImage();
            fig.ultravisionFieldRB.Start();
            fig.StartPosition = FormStartPosition.CenterScreen;
            fig.ShowDialog();
        }

        static void Ascan()
        {
            /*int num = 360;
            float[] x = new float[num];
            float[] y = new float[num];
            for (int i = 0; i < num; i++)
            {
                x[i] = (float)((i - 180));
                y[i] = 0.0f;
            }
            series a = new series(x, y);
            List<series> list = new List<series>() { a };
            ToolTest ascanTest = new AscanTest(x,y);
            ascanTest.StartPosition = FormStartPosition.CenterScreen;

            ascanTest.ascanToolLayout.StepX = 1;
            ascanTest.ascanToolLayout.StartX = 0;
            ascanTest.ascanToolLayout.EndX = 1024;
            ascanTest.ascanToolLayout.StepY = 0.5f;
            ascanTest.ascanToolLayout.StartY = 0;
            ascanTest.ascanToolLayout.EndY = 512;

            ascanTest.ascanToolLayout.gate0.Enable = true;
            ascanTest.ascanToolLayout.gate0.Threshold = 512;
            ascanTest.ascanToolLayout.gate0.Start = 100;
            ascanTest.ascanToolLayout.gate0.End = 150;*/


            //ascanTest.ShowDialog();
        }

        static void RefreshData()
        {
            while (true)
            {
                Thread.Sleep(10);
                int num = 360;
                float[] x = new float[num];
                float[] y = new float[num];
                for (int i = 0; i < num; i++)
                {
                    x[i] = (float)((i - 180));
                    y[i] = 0.0f;
                }
            }
        }

        static void LongStripTest()
        {
            int num = 360;
            float[] x = new float[num];
            float[] y = new float[num];
            float[] y2 = new float[num];
            float[] y3 = new float[num];

            for (int i = 0; i < num; i++)
            {
                x[i] = (float)((i - 180));
                y[i] = (float)Math.Sin(i * Math.PI / 180);
                y2[i] = (float)Math.Sin((i+10) * Math.PI / 180);
                y3[i] = (float)Math.Sin((i + 20) * Math.PI / 180);
            }
            series a = new series(x, y);
            series b = new series(x, y2);
            series c = new series(x, y3);
            List<series> list = new List<series>() { a,b,c };
            LongStripTest ls = new LongStripTest();
            ls.startX = x[0];
            ls.endX = x[num - 1];
            ls.startY = -1;
            ls.endY = 1;
            ls.stepX = x[1] - x[0];
            ls.stepY = 0.001f;
            ls.data = list;
            ls.judge0Enable = true;
            ls.judge1Enable = true;
            ls.judge1Color = System.Drawing.Color.Red;
            ls.judge0Color = System.Drawing.Color.Red;
            ls.judge0Y = -0.5f;
            ls.judge1y = 0.5f;
            ls.InfoFormat = "{0:0.000}";
            ls.tipFont = new System.Drawing.Font("宋体", 16);
            ls.tipFormat = "0.000";
            ls.Initial(true);
            /*LongStrip_Ruler lsr = ls.longStrip_Ruler1;
            lsr.longStrip_1.StepX = x[1]-x[0];
            lsr.longStrip_1.StepY = 0.001f;
            lsr.longStrip_1.StartX = x[0];
            lsr.longStrip_1.EndX = x[num - 1];
            lsr.longStrip_1.StartY = -1;
            lsr.longStrip_1.EndY = 1;
            lsr.longStrip_1.NewImage(list, true, new float[] { x[0], x[num - 1], -1, 1 });
            lsr.longStrip_1.longStripToolLayout1.JudgeLine0.InfoFormat = "{0:0.000}";
            lsr.longStrip_1.longStripToolLayout1.JudgeLine1.InfoFormat = "{0:0.000}";
            lsr.longStrip_1.longStripToolLayout1.JudgeLine0.Y = 0.5f;
            lsr.longStrip_1.longStripToolLayout1.JudgeLine1.Y = -0.5f;
            lsr.longStrip_1.longStripToolLayout1.JudgeLine0.Enable = true;
            lsr.longStrip_1.longStripToolLayout1.infoShow.data = list;
            lsr.longStrip_1.longStripToolLayout1.infoShow.startx0 = x[0];
            lsr.ReDrawRuler();

            ls.StartPosition = FormStartPosition.CenterParent;
            ls.ShowDialog();*/
        }

        static void Tooltest()
        {
            int num = 360;
            float[] x = new float[num];
            float[] y = new float[num];
            for (int i = 0; i < num; i++)
            {
                x[i] = (float)((i - 180));
                y[i] = (float)(Math.Sin(x[i]*Math.PI/180)*100);
            }
            series a = new series(x, y);
            List<series> list = new List<series>() { a };

            ToolTest toolTest = new ToolTest();
            toolTest.ascan_Ruler1.ascan_1.NewImage(list,true,new float[] { -180,180,-100,100});
            toolTest.ascan_Ruler1.ascan_1.ascanToolLayout1.StepX = 1;
            toolTest.ascan_Ruler1.ascan_1.ascanToolLayout1.StepY = 0.1f;
            toolTest.ascan_Ruler1.ascan_1.ascanToolLayout1.StartX = 0;
            toolTest.ascan_Ruler1.ascan_1.ascanToolLayout1.EndX = 360;
            toolTest.ascan_Ruler1.ascan_1.ascanToolLayout1.StartY = -100;
            toolTest.ascan_Ruler1.ascan_1.ascanToolLayout1.EndY = 100;
            toolTest.ascan_Ruler1.ascan_1.ascanToolLayout1.MeasureC.Enable = false;
            toolTest.ascan_Ruler1.ascan_1.ascanToolLayout1.ReferenceC.Enable = false;
            toolTest.ascan_Ruler1.ascan_1.ascanToolLayout1.gate1.Enable = true;
            toolTest.ascan_Ruler1.ascan_1.ascanToolLayout1.gate1.Start = 10;
            toolTest.ascan_Ruler1.ascan_1.ascanToolLayout1.gate1.End = 50;
            toolTest.ascan_Ruler1.ascan_1.ascanToolLayout1.gate1.Threshold = 10;

            //toolTest.ascan_Ruler1.ReDrawRuler();
            //toolTest.ascan_Ruler1.Invalidate();
            toolTest.ascan_Ruler1.ascan_1.Reset();
            toolTest.ShowDialog();
        }
        
    }

    


}

