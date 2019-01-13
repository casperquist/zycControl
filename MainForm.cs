using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ZYCControl
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int num = 360;
            float[] tx = new float[num*100];
            float[] ty = new float[num*100];
            for (int i = 0; i < num; i++)
            {
                tx[i] = (float)((i - 180) * Math.PI / 180.0);
                ty[i] = (float)(Math.Sin(tx[i])) * 50 + 50;
            }
            LongStripForm ascanTest = new LongStripForm();
            ascanTest.FigureInitial(new List<series>() { new series(tx, ty) }, false, new float[] { 0, 1, 0, 1 });
            ascanTest.Show();            

            int k = 10;
            while (true)
            {
                
                for (int i = num; i < num+k; i++)
                {
                    tx[i] = (float)((i - 180) * Math.PI / 180.0);
                    ty[i] = (float)(Math.Sin(tx[i])) * 50 + 50;
                }
                num += k;
                //ascanTest.longStrip1.ima.rawData[0] = new series(tx, ty);
                ascanTest.longStrip1.ima.Refresh(true);
                ascanTest.longStrip1.Refresh();
                ascanTest.rulerBarH.Refresh();
                ascanTest.rulerBarV.Refresh();
                //ascanTest.Refresh();
                Application.DoEvents();
                Thread.Sleep(100);
            }
        }

        private void AscanTest_RegionChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
