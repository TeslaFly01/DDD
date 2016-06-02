using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DDD.Utility
{
    public static class LoggerHelper
    {
        const string DirectPath = "Log";
        private const string BrStr = "---------------------------------------------------------------------------";

        public static void Log(string msg)
        {
            var dt = DateTime.Now;
            DirectoryInfo di = new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath("~") + "\\" + DirectPath);
            if (di.Exists == false) di.Create();
            DirectoryInfo disub = new DirectoryInfo(di.ToString() + "\\" + dt.ToString("yyyy-MM"));
            if (disub.Exists == false) disub.Create();

            var FilePath = disub.ToString() + "\\" + dt.ToString("yyyy-MM-dd") + ".txt";

            if (!File.Exists(FilePath))
            {
                using (var fs = File.Create(FilePath))//防止产生“另一个进程访问”出错，不能直接Create
                {
                    fs.Flush();
                    //fs.Close();//using了就不用close了
                }
            }
            using (var filestream = new System.IO.FileStream(FilePath, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.ReadWrite))
            {
                using (var sw = new StreamWriter(filestream, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine("[" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "]");
                    sw.WriteLine(msg);
                    sw.WriteLine(BrStr);
                    sw.Flush();
                }
            }
        }

    }
}
