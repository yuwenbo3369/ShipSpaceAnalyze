using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace ProConfiguration_IntelShipSpaceAnalys
{
    public class LogAPI
    {
        static string sFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "CustomPro\\Log.txt";

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="ep"></param>
        public static void WriteErrorLog(Exception ep)
        {
            DateTime pNowTime = DateTime.Now;

            using (FileStream pFileStream = new FileStream(sFilePath, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter pStreamWrite = new StreamWriter(pFileStream))
                {
                    pStreamWrite.WriteLine();
                    pStreamWrite.WriteLine("错误发生时间 ：" + pNowTime);
                    pStreamWrite.WriteLine("Message :" + ep.Message);
                    pStreamWrite.WriteLine("Source :" + ep.Source);
                    pStreamWrite.WriteLine("StackTrace :" + ep.StackTrace);
                    pStreamWrite.WriteLine("TargetSite :" + ep.TargetSite);
                    if (ep.InnerException != null)
                    {
                        pStreamWrite.WriteLine("TargetSite :" + ep.InnerException.Message);
                    }
                    pStreamWrite.Flush();
                }
            }
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="ep"></param>
        public static void WriteErrorLog(string sLog)
        {
            DateTime pNowTime = DateTime.Now;
            using (FileStream pFileStream = new FileStream(sFilePath, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter pStreamWrite = new StreamWriter(pFileStream))
                {
                    pStreamWrite.WriteLine();
                    pStreamWrite.WriteLine(sLog);
                    pStreamWrite.Flush();
                }
            }
        }
    }
}
