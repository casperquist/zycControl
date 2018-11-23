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
    public partial class FigureForm : System.Windows.Forms.Form
    {
        public FigureForm()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.  
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲  
            InitializeComponent();            
        }

        public void FigureInitial(InputImageData a, OutputBmp b, float tmp)
        {
            figure1.inputData = a;
            figure1.outBmp = b;
            figure1.zeroRatio = tmp;
            figure1.NewImage();
            figure1.ima.ZeroColRatio = tmp;

        }

        
    }
}
