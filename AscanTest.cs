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
    public partial class AscanTest : Form
    {
        public AscanTest(float[] x, float[] y)
        {
            InitializeComponent();
            int num = x.Length;
            ultraAscan1.data = new series(x, y);
            ultraAscan1.envelopActived = true;
            ultraAscan1.xStart = x[0];
            ultraAscan1.xEnd = x[num - 1];
            ultraAscan1.FigureInitial();
            //ultraAscan.Height += 50;
        }
    }
}
