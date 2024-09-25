using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PanSight3DForm
{
    class Common
    {
        public struct IPINFO
        {
            public string _hostIp;
            public string[] _cameraIps;
        }

        public struct DETECTINFO
        {
            public string _adapter;
            public IPINFO[] _ips;
        }

        public static DETECTINFO[] analysisIpInfos(string ips)
        {             
            string[] sArrayByAdapter = ips.Split(new[] { "###" }, StringSplitOptions.RemoveEmptyEntries);
            DETECTINFO[] allDectInfo = new DETECTINFO[sArrayByAdapter.Length];
            for (int i = 0; i < sArrayByAdapter.Length; i++)
            {
                string sItem = sArrayByAdapter[i];
                sItem = sItem.Trim();
                if (sItem.Length > 0)
                {
                    string[] sArrayTmp = sItem.Split(new[] { "@@@" }, StringSplitOptions.RemoveEmptyEntries);
                    if (sArrayTmp.Length == 2)
                    {
                        string sItem2 = sArrayTmp[0];
                        sItem2 = sItem2.Trim();
                        if (sItem2.Length > 0)
                        {
                            string[] sArrayHosts = sItem2.Split(new[] { "..." }, StringSplitOptions.RemoveEmptyEntries);
                            allDectInfo[i]._ips = new IPINFO[sArrayHosts.Length];
                            for (int j = 0; j < sArrayHosts.Length; j++)
                            {
                                string sItem3 = sArrayHosts[j];
                                sItem3 = sItem3.Trim();
                                if (sItem3.Length > 0)
                                {
                                    string[] sArrayTmp2 = sItem3.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                                    if (sArrayTmp2.Length == 2)
                                    {
                                        string sCameraIps = sArrayTmp2[0];
                                        sCameraIps = sCameraIps.Trim();
                                        if (sCameraIps.Length > 0)
                                        {
                                            string[] sArrayTmp3 = sCameraIps.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                            allDectInfo[i]._ips[j]._cameraIps = new string[sArrayTmp3.Length];
                                            for (int m = 0; m < sArrayTmp3.Length; m++)
                                            {
                                                allDectInfo[i]._ips[j]._cameraIps[m] = sArrayTmp3[m];
                                            }
                                            allDectInfo[i]._ips[j]._hostIp = sArrayTmp2[1];

                                        }
                                    }
                                }
                            }
                        }

                        allDectInfo[i]._adapter = sArrayTmp[1];
                    }
                }
            }
            return allDectInfo;
        }

        public static void InvokeFunc(Control invoker, Action action)
        {
            if (invoker != null && invoker.InvokeRequired)
            {
                invoker.BeginInvoke(action);
                return;
            }
            action();
        }

        //读取txt文件中总行数的方法
        public static int fileLineCount(String fileName)
        {
            Stopwatch sw = new Stopwatch();           
            int lineCount = 0;

            //按行读取
            sw.Restart();
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    while (sr.Peek() >= 0)
                    {
                        sr.ReadLine();
                        lineCount++;
                    }
					sr.Close();
                }
            }
            catch (Exception)
            {

            }
            sw.Stop();
          
            return lineCount;
        }
    }
}
