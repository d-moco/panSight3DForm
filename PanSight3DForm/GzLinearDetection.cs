using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanSight3DForm
{
    public static class GzLinearDetection
    {
        //public static float[][] bufferC; // x y z 重新
    
       
        public static int[][] CheckAlg(float[] buffer, int WLength, int channels, int HLength)
        {
            int length = WLength * channels;
            //bufferC = new float[HLength][];
            float[][]  bufferZ3 = new float[HLength][];
            for (int j = 0; j < HLength; j++)
            {
                //bufferC[j] = new float[length];
                bufferZ3[j] = new float[WLength];  

                //for (int i = 0, k = 2; i < length; i++, k += 3)
                //{
                //    bufferC[j][i] = buffer[i + j * length];
                //}
                for (int i = 0, k = 2; k < length; i++, k += 3)
                {
                    if (k + j * length < buffer.Length)  
                    {
                        bufferZ3[j][i] = buffer[k + j * length];
                    }
                }
            }

            return DefectDetectMX(bufferZ3,WLength, HLength);
        }


        private static int[][] DefectDetectMX(float[][] bufferZ3, int WLength, int HLength)
        {
            int[][] defLocZ;
            defLocZ = new int[HLength][];
            for (int j = 0; j < HLength; j++)
            {
                defLocZ[j] = new int[WLength + 2];
                int nr = 1;

                for (int i = 1; i < WLength; i++)
                {
                    if (bufferZ3[j][i] != 0)
                    {
                        defLocZ[j][nr] = i;
                        nr++;
                    }
                }

                defLocZ[j][0] = nr - 1; 
                defLocZ[j][nr + 1] = -1; 
            }

            return defLocZ;
        }
    }
}
