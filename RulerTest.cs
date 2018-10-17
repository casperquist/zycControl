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
    public partial class RulerTest : Form
    {
        public RulerTest()
        {
            
            InitializeComponent();
            rulerBar1.HoriBar = true;
            rulerBar1.startValue = 43727f;
            rulerBar1.endValue = 437292f;
            rulerBar1.minScalePN = 1;
            rulerBar1.maxScalePN = 10;
            rulerBar1.Draw();

            /*rulerBar2.HoriBar = true;
            rulerBar2.startValue = 0.727f;
            rulerBar2.endValue = 43.7292f;
            rulerBar2.minScalePN = 1;
            rulerBar2.maxScalePN = 10;

            
            rulerBar2.Draw();*/
        }

        private void RulerTest_SizeChanged(object sender, EventArgs e)
        {
        }

        private void rulerBar1_SizeChanged(object sender, EventArgs e)
        {
            rulerBar1.Draw();
        }
    }
}
