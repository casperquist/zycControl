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
    public partial class Ascan_Ruler : UserControl
    {
        public Ascan_Ruler()
        {
            InitializeComponent();
        }

        private void ascan_1_SizeChanged(object sender, EventArgs e)
        {
            ascan_1.Location = new Point(30, 0);
            ascan_1.Width = ClientSize.Width - 30;
            ascan_1.Height = ClientSize.Height - 30;

            rulerBarV.Height = ClientSize.Height - 30; ;

            rulerBarH.Width = ClientSize.Width - 30;
            rulerBarH.Location = new Point(30, ascan_1.Height);

            if (rulerBarH.Width != 0 &
                rulerBarV.Height != 0)
            {
                rulerBarH.Draw();
                rulerBarV.Draw();
            }
        }

        private void ascan_1_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
