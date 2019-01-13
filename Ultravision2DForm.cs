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
    public partial class Ultravision2DForm : Form
    {
        public Ultravision2DForm()
        {
            InitializeComponent();
        }

        private void InitialForm(InputImageData a, OutputBmp b, float tmp)
        {
            u2d.inputData = a;
            u2d.outBmp = b;
            u2d.zeroRatio = tmp;
            u2d.NewImage();
            u2d.ima.ZeroColRatio = tmp;
        }
                
    }
}
