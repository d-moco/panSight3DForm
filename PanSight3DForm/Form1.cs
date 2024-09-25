using PanSight3DForm.Model;
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
        #region 变量
        private bool isConnect = false;
        CameraManger cameraManger;
        #endregion
        public Form1()
        {
            InitializeComponent();
            cameraManger = new CameraManger();
        }

        private void btn_cameraSetting_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedIndex = 0;
        }

        private void btn_algSetting_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedIndex = 1;
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            if (!isConnect) 
            {
                DetectForm detectForm = new DetectForm(cameraManger);
                detectForm.TopMost = true;
                detectForm.ShowDialog();
                if (cameraManger.isConnect) 
                {

                    btn_connect.Text = "断开";
                    isConnect = true;
                    btn_connect.BackColor = Color.Red;
                    lb_status.Text = "状态: 已连接";
                    lb_status.BackColor = Color.Green;
                    textBoxLocalIp.Text = cameraManger.localIp;
                    textBoxRemoteIp.Text = cameraManger.targetIp;
                    var mode = cameraManger.GetMode();
                    nud_expo.Value = mode._uiExpo;
                    cbb_gain.Text = mode._usGain.ToString();
                    nup_jiguang.Value = mode._ucLaserPower;
                    nud_frameData.Value = mode._usFrame;
                    nud_x.Value = (decimal)mode._fXScaling;
                    nud_y.Value = (decimal)mode._fYScaling;
                    var camMode = cameraManger.GetCamMode();
                    cb_transmode.SelectedIndex = camMode._ucTransMode;
                }
               
            }
            else
            {
                cameraManger.DisConnectCamera();
                btn_connect.Text = "连接";
                isConnect = false;
                btn_connect.BackColor = Color.OliveDrab;
                lb_status.Text = "状态: 未连接";
                lb_status.BackColor = Color.Red;
                textBoxLocalIp.Text = "";
                textBoxRemoteIp.Text = "";
            }
        }
    }
}
