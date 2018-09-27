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
    public partial class FigureForm_Plot2D : Form
    {
        public FigureForm_Plot2D()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.  
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲  
            InitializeComponent();
        }

        public void FigureInitial(List<series> a, bool fixRange, float[] range)
        {
            
            
            figurePlot2D1.NewImage(a,fixRange,range);

        }

    }
}
