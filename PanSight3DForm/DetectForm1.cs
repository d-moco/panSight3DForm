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


namespace PanSight3DForm
{
    public partial class DetectForm1 : Form
    {
        public struct cameras
        {
            public string sourceIp;
            public string targetIp;
            public string cameraType;
            public string cameraStatus;
        }
        public string sourceIp;
        public string targetIp;

        private List<cameras> camerasList = new List<cameras>();
        public DetectForm1()
        {
            InitializeComponent();
            

            btn_scan_Click(null, null);
        }
      
      
        private void button1_Click(object sender, EventArgs e)
        {
         
        }
     
        private void btn_scan_Click(object sender, EventArgs e)
        {
            
            dataGridView1.Rows.Clear();
            camerasList.Clear();
            IntPtr ptr = CameraWrapper.SgDetectNetCameras();
            string sip = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ptr);
            Common.DETECTINFO[] devices = Common.analysisIpInfos(sip);


            if (devices != null && devices.Length > 0)
            {

                GetDeviceIPPair(devices).ToList().ForEach((item) =>
                {
                    cameras _camera = new cameras();
                    _camera.sourceIp = item.Item1;
                    _camera.targetIp = item.Item2.Split('(')[0];
                    _camera.cameraType = item.Item2.Split('(')[1].Split(')')[0].Split('=')[0];
                    _camera.cameraStatus = "未连接";
                    foreach (var v in camerasList)
                    {
                        if (v.cameraType == _camera.cameraType)
                        {
                            return;
                        }
                    }


                    camerasList.Add(_camera);
                });
            }
            foreach (var item in camerasList)
            {
                dataGridView1.Rows.Add(item.sourceIp,item.targetIp,item.cameraType,item.cameraStatus);
            }
        }
        IEnumerable<Tuple<string, string>> GetDeviceIPPair(Common.DETECTINFO[] infos)
        {
            foreach (var i in infos)
            {
                foreach (var j in i._ips)
                {
                    foreach (var k in j._cameraIps)
                    {
                        yield return Tuple.Create(j._hostIp, k);
                    }
                }
            }
        }
        private void btn_connect_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            if (selectedRow.Cells[0].Value != null) 
            {
                sourceIp = selectedRow.Cells[0].Value.ToString();
                targetIp = selectedRow.Cells[1].Value.ToString();

                this.DialogResult = DialogResult.OK;
            }
        }

        private void DetectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
