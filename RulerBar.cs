using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZYCControl
{
    public partial class RulerBar : UserControl
    {
        public RulerBar()
        {
            InitializeComponent();
            GapPixl = minGapPixl;
        }

        public bool HoriBar;
        public float startValue, endValue;
        public int startP, endP;
        private int minGapPixl = 10;
        private int maxGapPixl = 20;
        private int GapPixl;
        private int scalePN;
        private int k;
        /// <summary>
        /// 标尺线间隔点
        /// </summary>
        private List<int> scale;
        private List<bool> isLong;

        private void Draw()
        {

        }

        private void CalculateScale()
        {
            scalePN = (endP - startP) / GapPixl;
            
        }

    }
}
