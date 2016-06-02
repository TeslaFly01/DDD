using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DDD.BusinessServiceEngine.Common
{
    public class LogFileHelper
    {
        const string DirectPath = "Log";

        static object _fileLockObj = new object();

        public static void InsertMsg(string msg)
        {
            //锁住写日文对象，防止多线程同时写文件，造成并发。
            //Update by Chen De Jun 2013-10-8
            lock (_fileLockObj)
            {
                var dt = DateTime.Now;
                var di = new DirectoryInfo(Application.StartupPath + "\\" + DirectPath);
                if (di.Exists == false) di.Create();
                var disub = new DirectoryInfo(di.ToString() + "\\" + dt.ToString("yyyy-MM"));
                if (disub.Exists == false) disub.Create();

                //每天生成一个文件，会造成文件容易增加，减慢效率。故意每天每小时生成一个文件。 
                //Update by Chen De Jun 2013-10-8
                var filePath = disub.ToString() + "\\" + dt.ToString("yyyy-MM-dd HH") + ".txt";

                if (!File.Exists(filePath))
                {
                    FileStream fs = File.Create(filePath);//防止产生“另一个进程访问”出错，不能直接Create
                    fs.Flush();
                    fs.Close();
                }
                var filestream = new System.IO.FileStream(filePath, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.ReadWrite);
                var sw = new StreamWriter(filestream, Encoding.GetEncoding("gb2312"));
                //File.AppendText(FilePath);
                sw.WriteLine(msg);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
