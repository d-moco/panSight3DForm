using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camera;
using Sunny.UI.Win32;
using static AntdUI.Math3D;
namespace PanSight3DForm.Model
{
    public class CameraManger
    {
        #region 变量
        CameraWrapper.SGEXPORT_ROI m_roi = new CameraWrapper.SGEXPORT_ROI();
        CameraWrapper.SGEXPORT_MODE m_mode = new CameraWrapper.SGEXPORT_MODE();
        CameraWrapper.SGEXPORT_CAMCONFIG m_cfg = new CameraWrapper.SGEXPORT_CAMCONFIG();
        CameraWrapper.SGEXPORT_PRODUCTINFO m_prod = new CameraWrapper.SGEXPORT_PRODUCTINFO();
        public struct cameras
        {
            public string sourceIp;
            public string targetIp;
            public string cameraType;
            public string cameraStatus;
        }

        private CameraSyn m_camera = new CameraSyn();
        private List<cameras>  camerasList =new List<cameras>();
        public bool isConnect = false;
        public string localIp;
        public string targetIp;
        #endregion 

        public CameraManger() 
        {
            m_camera = new CameraSyn();
            m_camera.ImageEvent += M_camera_ImageEvent;
        }

        private void M_camera_ImageEvent(IntPtr arg1, CameraWrapper.SG_IMGDATA_PARAM arg2, IntPtr arg3)
        {
            
        }

        public void ScannCamera() 
        {
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
        }

        public bool ConnectCamera(string sourceip,string targetIp) 
        {
            
            m_camera.LocalIp = sourceip;
            m_camera.CameraIp = targetIp;

            var ret = m_camera.Connect();
            string v = "已连接";
            if (!ret) v = "未连接";
            else 
            {
                localIp = m_camera.LocalIp;
                this.targetIp = m_camera.CameraIp;
            }
            for (int i = 0; i < camerasList.Count; i++)
            {
                if (m_camera.CameraIp == camerasList[i].targetIp)
                {
                    cameras _cameras = new cameras();
                    _cameras.sourceIp = camerasList[i].sourceIp;
                    _cameras.targetIp = camerasList[i].targetIp;
                    _cameras.cameraStatus = v;
                    camerasList[i] = _cameras;
                }
            }
            isConnect = ret;
            return ret;
        }
        public bool DisConnectCamera() 
        {
            m_camera.CloseCamera();
            for (int i = 0; i < camerasList.Count; i++)
            {
                if (m_camera.CameraIp == camerasList[i].targetIp)
                {
                    cameras _cameras = new cameras();
                    _cameras.sourceIp = camerasList[i].sourceIp;
                    _cameras.targetIp = camerasList[i].targetIp;
                    _cameras.cameraStatus = "未连接";
                    camerasList[i] = _cameras;
                }
            }
            isConnect = false;
            return true;
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
        public bool SetExpor(uint expo)
        {
            m_cfg._uiExpo = expo;

            if (m_camera.SetCamConfig(m_cfg)) 
            {
                return true;
            }
            return false;
        }
        public bool SetGain(ushort gain) 
        {
            m_cfg._usGain = gain;
            if (m_camera.SetCamConfig(m_cfg))
            {
                return true;
            }
            return false;
        }
       public List<cameras> GetCameras 
        
        {
            get { return camerasList; }
        }

        public CameraWrapper.SGEXPORT_CAMCONFIG  GetMode() 
        {
            m_camera.GetCamConfig(ref m_cfg);
            return m_cfg;
            
        }
        public CameraWrapper.SGEXPORT_MODE GetCamMode() 
        {
            m_camera.GetCamMode(ref m_mode);
            return m_mode;

        }
    }
}
