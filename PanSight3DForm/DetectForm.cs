using Camera;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PanSight3DForm.Model;

namespace PanSight3DForm
{
    public partial class DetectForm : Form
    {

        CameraManger cameraManger;
        public DetectForm(CameraManger _cameraManger)
        {
            InitializeComponent();
            cameraManger = _cameraManger;

            btn_scan_Click(null, null);
        }
      
      
        private void button1_Click(object sender, EventArgs e)
        {
         
        }

        private void btn_scan_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            cameraManger.ScannCamera();
            var _camera = cameraManger.GetCameras;
            foreach (var item in _camera)
            {
                dataGridView1.Rows.Add(item.sourceIp,item.targetIp,item.cameraType,item.cameraStatus);
            }
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            if (selectedRow.Cells[0].Value != null) 
            {
                string sourceIp = selectedRow.Cells[0].Value.ToString();
                string targetIp = selectedRow.Cells[1].Value.ToString();

                var ret = cameraManger.ConnectCamera(sourceIp,targetIp);
                if (ret == true) 
                {
                 
                    selectedRow.Cells[3].Value = "已连接";
                }
                else
                {
                    selectedRow.Cells[3].Value = "未连接";
                }
            }
        }

        private void btn_disConnect_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            if (selectedRow.Cells[0].Value != null)
            {
                string sourceIp = selectedRow.Cells[0].Value.ToString();
                string targetIp = selectedRow.Cells[1].Value.ToString();

                var ret = cameraManger.DisConnectCamera();
                if (!ret)
                {
                    selectedRow.Cells[3].Value = "已连接";
                }
                else
                {
                    selectedRow.Cells[3].Value = "未连接";
                }
            }
        }
    }
}
