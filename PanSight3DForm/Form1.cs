using Camera;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PanSight3DForm
{
    public partial class Form1 : Form
    {
        #region 变量
        CameraWrapper.SGEXPORT_ROI m_roi = new CameraWrapper.SGEXPORT_ROI();
        CameraWrapper.SGEXPORT_MODE m_mode = new CameraWrapper.SGEXPORT_MODE();
        CameraWrapper.SGEXPORT_CAMCONFIG m_cfg = new CameraWrapper.SGEXPORT_CAMCONFIG();
        CameraWrapper.SGEXPORT_PRODUCTINFO m_prod = new CameraWrapper.SGEXPORT_PRODUCTINFO();
        private CameraSyn m_camera = new CameraSyn();
        private bool isConnect = false;
        public string sourceIp;
        public string targetIp;
        float m_fTemperature = 0;

        enum TEST_TYPE { TEST_TYPE_GRAB_PNTS_CLOUD, TEST_TYPE_LOOP_GRAB, TEST_TYPE_PROFILES };

        TEST_TYPE m_testType = TEST_TYPE.TEST_TYPE_GRAB_PNTS_CLOUD;
        #endregion
        public Form1()
        {
            InitializeComponent();
            m_camera.DepthEvent += onDepth;
           
            m_camera.prepare();
        }

        private void M_camera_ImageEvent(IntPtr arg1, CameraWrapper.SG_IMGDATA_PARAM arg2, IntPtr arg3)
        {
            throw new NotImplementedException();
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

                DetectForm1 detectForm = new DetectForm1();
                detectForm.TopMost = true;
                if (detectForm.ShowDialog() == DialogResult.OK)
                {
                    sourceIp = detectForm.sourceIp;
                    targetIp = detectForm.targetIp;
                    if (m_camera.Connect2Camera(sourceIp, targetIp))
                    {

                        if (m_camera.GetRoiAndMedia(ref m_roi) == false)
                        {
                            MessageBox.Show("获取ROI失败");
                            return;
                        }
                        if (m_camera.GetCamMode(ref m_mode) == false)
                        {
                            MessageBox.Show("获取模式失败");
                            return;
                        }
                        cb_transmode.SelectedIndex = m_mode._ucTransMode;

                        if (m_mode._ucDataType == (byte)CameraWrapper.DATATYPE.DATATYPE_PROFILE_GRAY)
                        {
                            checkBoxGray.Checked = true;
                        }
                        else
                        {
                            checkBoxGray.Checked = false;
                        }

                        numericUpDownLineNum.Value = m_mode._uiGrabNumber;
                        if (m_camera.GetCamConfig(ref m_cfg) == false)
                        {
                            MessageBox.Show("获取配置失败");
                            return;
                        }
                        if (m_camera.GetCamProductInfos(ref m_prod) == false)
                        {
                            MessageBox.Show("获取产品信息失败");
                            return;
                        }
                        btn_connect.Text = "断开";
                        isConnect = true;
                        btn_connect.BackColor = Color.Red;
                        lb_status.Text = "状态: 已连接";
                        lb_status.BackColor = Color.Green;
                        textBoxLocalIp.Text = sourceIp;
                        textBoxRemoteIp.Text = targetIp;

                        nud_expo.Value = m_cfg._uiExpo;
                        cbb_gain.Text = m_cfg._usGain.ToString();
                        nup_jiguang.Value = m_cfg._ucLaserPower;
                        nud_frameData.Value = m_cfg._usFrame;
                        nud_x.Value = (decimal)m_cfg._fXScaling;
                        nud_y.Value = (decimal)m_cfg._fYScaling;
                       



                    }


                }

            }
            else
            {
               
                m_camera.CloseCamera();

                btn_connect.Text = "连接";
                isConnect = false;
                btn_connect.BackColor = Color.OliveDrab;
                lb_status.Text = "状态: 未连接";
                lb_status.BackColor = Color.Red;
                textBoxLocalIp.Text = "";
                textBoxRemoteIp.Text = "";
            }

        }
        unsafe void saveDepthData(float* pBuf, uint bufSize)
        {
            string dataDir = "c:\\data";
            if (Directory.Exists(dataDir) == false)
            {
                Directory.CreateDirectory(dataDir);
            }
            string timestamp = DateTime.Now.ToString("yyMMddHHmmss_fff");
            string filename = string.Format("{0}\\Depth_t{1}.txt", dataDir, timestamp);
            StreamWriter sw = new StreamWriter(filename);
            string content = "";

            for (int i = 0; i < bufSize;)
            {
                content = string.Format("{0},{1},{2}", pBuf[i], pBuf[i + 1], pBuf[i + 2]);
                i += 3;
                sw.WriteLine(content);
            }
            sw.Close();

            Common.InvokeFunc(this, () => {
                labelTips.Text = "点云文件抓取成功，" + filename;
            });
        }
        int m_iGrabProfileNum = 0; 
        int m_iGrabCallbackCount = 0;
        unsafe void onDepth(IntPtr pBuf, IntPtr pGrayBuf, CameraWrapper.SG_DEPTHDATA_PARAM param, IntPtr pOwner)
        {
            if (pBuf == null)
            {
                return;
            }

            Common.InvokeFunc(this, () => {
                labelTips.Text = "数据全部采集完成，开始转换显示";
            });


            if (param._bProfile == 1)
            {
                return;
            }

            int w = param._iPointNumPerLine;
            int h = param._iCapturedProfileLineNum;

            if (pGrayBuf != null && param._iCapturedProfileLineNum > 0)
            {
                // 显示灰度图
                showGrayImg((byte*)pGrayBuf, w, h);
                
            }

            if (pBuf != null && param._iCapturedProfileLineNum > 0)
            {
                // 显示高度图
                //showDepthImg(pBuf, param);
                showDepthImg((float*)pBuf, w, h);
            }

            // 重点声明一下，onDepth 函数是SDK中的线程回调上来的，这里面不要有耗时等待的操作。

            m_iGrabProfileNum = param._iCapturedProfileLineNum;
            m_iGrabCallbackCount++;
            if (m_testType != TEST_TYPE.TEST_TYPE_PROFILES)
            {
                // 抓取点云
                Common.InvokeFunc(this, () => {

                    // 不要在SDK的回调线程中直接调用sdk接口,此处切换到ui线程中调用

                    m_fTemperature = 0;
                    m_camera.GetTemperature(ref m_fTemperature);

                    labelMsg.Text = DateTime.Now.ToString("HH:mm:ss.fff") + " 抓取线数:" + m_iGrabProfileNum + "  温度:" + m_fTemperature;
                });
            }


            //if (h > 0)
            //{
            //    saveDepthData((float*)pBuf, (uint)(w * h * 3));
            //}
            //Common.InvokeFunc(this, () => {
            //    BufToHImage((float*)pBuf, (byte*)pGrayBuf, w, h);
            //});
        }

        unsafe void showGrayImg( byte* pGrayBuf, int width, int height)
        {
            int iPntsCount = width * height;

            float[] y_pBuf = new float[iPntsCount];
            float[] x_pBuf = new float[iPntsCount];
            float[] z_pBuf = new float[iPntsCount];

            byte[] grayBufff = null;
            if (pGrayBuf != null)
            {
                grayBufff = new byte[iPntsCount];
            }


            for (int i = 0; i < iPntsCount; i++)
            {
              
                if (pGrayBuf != null)
                {
                    grayBufff[i] = pGrayBuf[i];
                }

            }
            //HTuple obj3DHandle;//3D句柄
            //HTuple hTupleX = new HTuple(x_pBuf);
            //HTuple hTupleY = new HTuple(y_pBuf);
            //HTuple hTupleZ = new HTuple(z_pBuf);
            //HTuple hTupleGray = new HTuple();
            //if (pGrayBuf != null)
            //{
            //    hTupleGray = new HTuple(iPntsCount);                
            //}
            //HOperatorSet.GenObjectModel3dFromPoints(hTupleX, hTupleY, hTupleZ, out obj3DHandle);
            //void* p0, p1, p2, p3;
            IntPtr ypRawData = IntPtr.Zero;// Marshal.AllocHGlobal(width * height); //分配非托管内存空间
            IntPtr xpRawData = IntPtr.Zero;// Marshal.AllocHGlobal(width * height);
            IntPtr zpRawData = IntPtr.Zero;// Marshal.AllocHGlobal(width * height);
            IntPtr gpRawData = IntPtr.Zero;// Marshal.AllocHGlobal(width * height);
            fixed (void* py = y_pBuf)
            {
                ypRawData = new IntPtr(py);
            }
            fixed (void* px = x_pBuf)
            {
                xpRawData = new IntPtr(px);
            }
            fixed (void* pz = z_pBuf)
            {
                zpRawData = new IntPtr(pz);
            }
            if (pGrayBuf != null)
            {
                fixed (void* pg = grayBufff)
                {
                    gpRawData = new IntPtr(pg);
                }
            }
            HObject img = null, x = null, y = null, z = null;
            img?.Dispose();
            HOperatorSet.GenImage3(out img, "real", width, height, xpRawData, ypRawData, zpRawData);
            x?.Dispose(); y?.Dispose(); z?.Dispose();
            HOperatorSet.Decompose3(img, out x, out y, out z);
            HTuple w, h;

            HOperatorSet.GetImageSize(z, out w, out h);
            HOperatorSet.SetPart(this.hWindowControlHeight.HalconWindow, 0, 0, h - 1, w - 1);
            HOperatorSet.DispObj(z, this.hWindowControlHeight.HalconWindow);
            
            if (pGrayBuf != null)
            {
                HObject gray = null;
                gray?.Dispose();
                HOperatorSet.GenImage1(out gray, "byte", width, height, gpRawData);
                HOperatorSet.GetImageSize(gray, out w, out h);
                HOperatorSet.SetPart(this.hWindowControlGray.HalconWindow, 0, 0, h - 1, w - 1);
                HOperatorSet.DispObj(gray, this.hWindowControlGray.HalconWindow);
            }
        }
      
        unsafe void showDepthImg(float* pBuf, int width, int height) 
        {
            int iPntsCount = width * height;

            float[] y_pBuf = new float[iPntsCount];
            float[] x_pBuf = new float[iPntsCount];
            float[] z_pBuf = new float[iPntsCount];



            //float/*[*/] a_pBuf = new float[iPntsCount * 3];
            for (int i = 0; i < iPntsCount; i++)
            {
                y_pBuf[i] = pBuf[i * 3];
                x_pBuf[i] = pBuf[i * 3 + 1];
                z_pBuf[i] = pBuf[i * 3 + 2];
                
                //a_pBuf[i*3] = pBuf[i * 3];
                //a_pBuf[i*3+1] = pBuf[i * 3 + 1];
                //a_pBuf[i*3+2] = pBuf[i * 3 + 2];

            }
            //// 定义txt文件的路径
            //string filePath = "all_buf.txt";

            //// 使用StreamWriter写入文件
            //using (StreamWriter writer = new StreamWriter(filePath))
            //{
            //    // 遍历数组并写入每个元素到txt文件
            //    foreach (float value in a_pBuf)
            //    {
            //        writer.WriteLine(value);
            //    }
            //}
            //// 定义txt文件的路径
            //filePath = "x1.txt";

            //// 使用StreamWriter写入文件
            //using (StreamWriter writer = new StreamWriter(filePath))
            //{
            //    // 遍历数组并写入每个元素到txt文件
            //    foreach (float value in x_pBuf)
            //    {
            //        writer.WriteLine(value);
            //    }
            //}
            //// 定义txt文件的路径
            //filePath = "y1.txt";

            //// 使用StreamWriter写入文件
            //using (StreamWriter writer = new StreamWriter(filePath))
            //{
            //    // 遍历数组并写入每个元素到txt文件
            //    foreach (float value in y_pBuf)
            //    {
            //        writer.WriteLine(value);
            //    }
            //}
            //HTuple obj3DHandle;//3D句柄
            //HTuple hTupleX = new HTuple(x_pBuf);
            //HTuple hTupleY = new HTuple(y_pBuf);
            //HTuple hTupleZ = new HTuple(z_pBuf);
            //HTuple hTupleGray = new HTuple();
            //if (pGrayBuf != null)
            //{
            //    hTupleGray = new HTuple(iPntsCount);                
            //}
            //HOperatorSet.GenObjectModel3dFromPoints(hTupleX, hTupleY, hTupleZ, out obj3DHandle);
            //void* p0, p1, p2, p3;
            IntPtr ypRawData = IntPtr.Zero;// Marshal.AllocHGlobal(width * height); //分配非托管内存空间
            IntPtr xpRawData = IntPtr.Zero;// Marshal.AllocHGlobal(width * height);
            IntPtr zpRawData = IntPtr.Zero;// Marshal.AllocHGlobal(width * height);
            IntPtr gpRawData = IntPtr.Zero;// Marshal.AllocHGlobal(width * height);
            fixed (void* py = y_pBuf)
            {
                ypRawData = new IntPtr(py);
            }
            fixed (void* px = x_pBuf)
            {
                xpRawData = new IntPtr(px);
            }
            fixed (void* pz = z_pBuf)
            {
                zpRawData = new IntPtr(pz);
            }
           
            HObject img = null, x = null, y = null, z = null;
            img?.Dispose();
            HOperatorSet.GenImage3(out img, "real", width, height, xpRawData, ypRawData, zpRawData);
            x?.Dispose(); y?.Dispose(); z?.Dispose();
            HOperatorSet.Decompose3(img, out x, out y, out z);
            HTuple w, h;

            HOperatorSet.GetImageSize(z, out w, out h);
            HOperatorSet.SetPart(this.hWindowControlHeight.HalconWindow, 0, 0, h - 1, w - 1);
            HOperatorSet.DispObj(z, this.hWindowControlHeight.HalconWindow);

           
        }
    

        public byte Rescale(float value, float max, float min)
        {
            if (value < min)
            {
                return 0;
            }
            else if (value > max)
            {
                return 255;
            }
            else if (Math.Abs(max - min) < 0.00001)
            {
                // 同一个平面
                return 0;
            }
            return (byte)(((value - min) / (max - min)) * 255);
        }
        unsafe void BufToHImage(float* pBuf, byte* pGrayBuf, int width, int height)
        {
            int iPntsCount = width * height;

            float[] y_pBuf = new float[iPntsCount];
            float[] x_pBuf = new float[iPntsCount];
            float[] z_pBuf = new float[iPntsCount];

            byte[] grayBufff = null;
            if (pGrayBuf != null)
            {
                grayBufff = new byte[iPntsCount];
            }


            for (int i = 0; i < iPntsCount; i++)
            {
                y_pBuf[i] = pBuf[i * 3];
                x_pBuf[i] = pBuf[i * 3 + 1];
                z_pBuf[i] = pBuf[i * 3 + 2];
                if (pGrayBuf != null)
                {
                    grayBufff[i] = pGrayBuf[i];
                }

            }
            //HTuple obj3DHandle;//3D句柄
            //HTuple hTupleX = new HTuple(x_pBuf);
            //HTuple hTupleY = new HTuple(y_pBuf);
            //HTuple hTupleZ = new HTuple(z_pBuf);
            //HTuple hTupleGray = new HTuple();
            //if (pGrayBuf != null)
            //{
            //    hTupleGray = new HTuple(iPntsCount);                
            //}
            //HOperatorSet.GenObjectModel3dFromPoints(hTupleX, hTupleY, hTupleZ, out obj3DHandle);
            //void* p0, p1, p2, p3;
            IntPtr ypRawData = IntPtr.Zero;// Marshal.AllocHGlobal(width * height); //分配非托管内存空间
            IntPtr xpRawData = IntPtr.Zero;// Marshal.AllocHGlobal(width * height);
            IntPtr zpRawData = IntPtr.Zero;// Marshal.AllocHGlobal(width * height);
            IntPtr gpRawData = IntPtr.Zero;// Marshal.AllocHGlobal(width * height);
            fixed (void* py = y_pBuf)
            {
                ypRawData = new IntPtr(py);
            }
            fixed (void* px = x_pBuf)
            {
                xpRawData = new IntPtr(px);
            }
            fixed (void* pz = z_pBuf)
            {
                zpRawData = new IntPtr(pz);
            }
            if (pGrayBuf != null)
            {
                fixed (void* pg = grayBufff)
                {
                    gpRawData = new IntPtr(pg);
                }
            }
            HObject img = null, x = null, y = null, z = null;
            img?.Dispose();
            HOperatorSet.GenImage3(out img, "real", width, height, xpRawData, ypRawData, zpRawData);
            x?.Dispose(); y?.Dispose(); z?.Dispose();
            HOperatorSet.Decompose3(img, out x, out y, out z);
            HTuple w, h;

            HOperatorSet.GetImageSize(z, out w, out h);
            HOperatorSet.SetPart(this.hWindowControlHeight.HalconWindow, 0, 0, h - 1, w - 1);
            HOperatorSet.DispObj(z, this.hWindowControlHeight.HalconWindow);

            if (pGrayBuf != null)
            {
                HObject gray = null;
                gray?.Dispose();
                HOperatorSet.GenImage1(out gray, "byte", width, height, gpRawData);
                HOperatorSet.GetImageSize(gray, out w, out h);
                HOperatorSet.SetPart(this.hWindowControlGray.HalconWindow, 0, 0, h - 1, w - 1);
                HOperatorSet.DispObj(gray, this.hWindowControlGray.HalconWindow);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CameraWrapper.SGEXPORT_MODE mode = m_mode;
            mode._ucCaptureMode = (byte)CameraWrapper.CAPMODE.CAPMODE_AUTO;
            mode._uiGrabNumber = (uint)numericUpDownLineNum.Value;
            if (checkBoxGray.Checked)
            {
                mode._ucDataType = (byte)CameraWrapper.DATATYPE.DATATYPE_PROFILE_GRAY;
            }
            else
            {
                mode._ucDataType = (byte)CameraWrapper.DATATYPE.DATATYPE_PROFILE;
            }
            mode._ucUserOperatorMode = (byte)CameraWrapper.DATAMODE.DATAMODE_GRAB;
            mode._ucTransMode = (byte)cb_transmode.SelectedIndex; // 对应到 CameraWrapper.TRANSMODE
            m_camera.SetCamMode(mode);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CameraWrapper.SGEXPORT_CAMCONFIG cfg = m_cfg;
            m_cfg._usFrame = (ushort)nud_frameData.Value;
            m_cfg._fYScaling = (ushort)nud_y.Value;
            m_cfg._fXScaling = (ushort)nud_x.Value;
            var ret = m_camera.SetCamConfig(m_cfg);
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            m_camera.GetCamConfig(ref m_cfg);
            m_cfg._uiExpo = (uint)nud_expo.Value;
            m_cfg._usGain = ushort.Parse(cbb_gain.Text);
            m_cfg._ucLaserPower = (byte)nup_jiguang.Value;
            m_camera.SetCamConfig(m_cfg);
           
        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {
            labelTips.Text = "开始抓取...";
            bool ret = m_camera.SendGrabSignalToCameraEx(false);
            if (ret == false)
            {
                labelMsg.Text = "抓取失败";
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (m_mode._ucUserOperatorMode != (byte)CameraWrapper.DATAMODE.DATAMODE_GRAB)
            {
                MessageBox.Show("相机模式不对（非抓取模式）");
                return;
            }

            bool bStart = m_camera.StartCapture();
            if (bStart)
            {
                labelMsg.Text = "准备抓取成功，请使用定时抓取或单步触发功能";
            }
            else
            {
                labelMsg.Text = "准备抓起失败";
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            bool bStop = m_camera.StopCapture();
            if (bStop)
            {
                labelMsg.Text = "结束抓取成功";
            }
            else
            {
                labelMsg.Text = "结束抓起失败";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            CameraSyn.LibRelease();
        }
    }
}
