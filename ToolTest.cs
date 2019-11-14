using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZYCControl
{
    public partial class ToolTest : Form
    {
        private int cnt = 0;
        public ToolTest()
        {
            InitializeComponent();
            ResponseGate();
            cnt = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ascan_Ruler1.ascan_1.ascanToolLayout1.ReferenceC.Enable = false;
            ascan_Ruler1.ascan_1.ascanToolLayout1.MeasureC.Enable = false;
            timer1.Enabled = true;
            cnt = 0;
            //ascan_Ruler1.ascan_1.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cnt++;
            for (int i = 0; i < ascan_Ruler1.ascan_1.ima.rawData[0].Count; i++)
                ascan_Ruler1.ascan_1.ima.rawData[0].y[i] = (float)(Math.Sin((i+ cnt-180)*Math.PI/180.0)*100f);
            //ascan_Ruler1.ascan_1.ima.Refresh(true);
            try
            {
                ascan_Ruler1.ascan_1.Invalidate();

            }
            catch
            { }
        }

        private void tt()
        {
            

            for (int i = 0; i < ascan_Ruler1.ascan_1.ima.rawData[0].Count; i++)
                ascan_Ruler1.ascan_1.ima.rawData[0].y[i] *= 1.1f;
            ascan_Ruler1.ascan_1.ascanToolLayout1.Refresh();
            ascan_Ruler1.ascan_1.ima.Refresh(false);
        }

        private void ResponseGate()
        {
            ascan_Ruler1.ascan_1.ascanToolLayout1.GateChanged += new MarkChangeHandleEvent(RefreshText);
        }

        private void RefreshText(float[] para)
        {
            textBox1.Text = para[1].ToString();
        }
    }
}
