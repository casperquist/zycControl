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
    public partial class TransparentInfo : UserControl
    {
        private Graphics g;
        /// <summary>
        /// 输出信息的字体
        /// </summary>
        public Font infoFont;
        /// <summary>
        /// 输出信息的x轴，y轴名称和单位
        /// </summary>
        public string xName, yName, xUnit, yUnit;
        /// <summary>
        /// 输出信息的数据格式
        /// </summary>
        public string xStringFormat, ystringFormat;

        private bool _JudgeLine0Enable;
        public bool JudgeLine0Enable { get { return _JudgeLine0Enable; } set { _JudgeLine0Enable = value; } }

        private bool _JudgeLine1Enable;
        public bool JudgeLine1Enable { get { return _JudgeLine1Enable; } set { _JudgeLine1Enable = value; } }

        private bool _JudgeLine2Enable;
        public bool JudgeLine2Enable { get { return _JudgeLine2Enable; } set { _JudgeLine2Enable = value; } }

        private float _JudgeLine0;
        public float JudgeLine0 { get { return _JudgeLine0; } set { _JudgeLine0 = value; } }

        private float _JudgeLine1;
        public float JudgeLine1 { get { return _JudgeLine1; } set { _JudgeLine1 = value; } }

        private float _JudgeLine2;
        public float JudgeLine2 { get { return _JudgeLine2; } set { _JudgeLine2 = value; } }

        public TransparentInfo()
        {
            InitializeComponent();
            if (g == null)
                g = CreateGraphics();
        }

        public void Initialization()
        {
            if (infoFont == null)
                infoFont = new Font("宋体", 15, FontStyle.Bold);
            if (xName == null)
                xName = "X";
            if (yName == null)
                yName = "Y";
            if (xUnit == null)
                xUnit = "";
            if (yUnit == null)
                yUnit = "";
            if (xStringFormat == null)
                xStringFormat = "{0:0.0}";
            if (ystringFormat == null)
                ystringFormat = "{0:0.00}";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            
            GC.Collect();
        }

    }
}
