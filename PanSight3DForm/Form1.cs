using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PanSight3DForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_cameraSetting_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedIndex = 0;
        }

        private void btn_algSetting_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedIndex = 1;
        }
    }
}
