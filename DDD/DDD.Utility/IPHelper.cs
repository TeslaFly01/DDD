using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace DDD.Utility
{
    public  class IPHelper
    {
        public IPHelper()
        {
 
        }
        public static string getIPAddr()
        {
            string user_IP = string.Empty;
            try
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    user_IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    user_IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
            }
            catch
            {
                try
                {
                    user_IP = HttpContext.Current.Request.UserHostAddress;
                }
                catch (Exception)
                {
                    user_IP = "-";
                }
                
            }
            return user_IP;
        }

        /// <summary>
        /// 取得ip地址前几段(127.0.*.*)
        /// </summary>
        /// <param name="myIP"></param>
        /// <param name="myPreNum"></param>
        /// <returns></returns>
        public static string getIPPrefix(string myIP, int myPreNum)
        {
            string re = "";
            if (!string.IsNullOrEmpty(myIP))
            {
                string[] myIPs = myIP.Split('.');
                int P = myIPs.Length < myPreNum ? myIPs.Length : myPreNum;

                for (int i = 0; i < P; i++)
                {
                    re += myIPs[i] + ".";
                }
                re = re.Substring(0, re.LastIndexOf("."));
                if (myIPs.Length > myPreNum)
                {
                    int k = myIPs.Length - myPreNum;
                    string sdot = "";
                    for (int j = 0; j < k; j++)
                    {
                        sdot += ".*";
                    }
                    re = re + sdot;
                }
            }
            return re;
        }
    }
}
