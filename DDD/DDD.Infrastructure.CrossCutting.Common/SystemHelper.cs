using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DDD.Infrastructure.CrossCutting.Common
{
    public static class SystemHelper
    {

        private const string PublicSite_1ktong = "http://www.1ktong.com";
        public static string GetPublicSiteURL_1ktong()
        {
            return CheckHostName(PublicSite_1ktong);
        }
        private const string PublicSite_wqdzp = "http://www.wqdzp.com";
        public static string GetPublicSiteURL_wqdzp()
        {
            return CheckHostName(PublicSite_wqdzp);
        }

        static string CheckHostName(string myURL)//泛解析url的处理
        {
            string re = myURL;
            if (!HttpContext.Current.Request.Url.Host.ToLower().Contains("www."))
            {
                re = myURL.Replace("http://www.", "http://");
            }
            return re;
        }

        /// <summary>
        /// 密码黑名单
        /// </summary>
        /// <returns></returns>
        public static string[] CanNotUsePassword()
        {
            string[] cannotuse = { "123456", "1234567", "12345678", "123456789", "1234567890" };
            return cannotuse;
        }


        /// <summary>
        /// 检查密码是否太简单
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="CUName"></param>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public static bool CheckPassword(string Password, string CUName, string LoginName)
        {
            foreach (var item in CanNotUsePassword())
            {
                if (item.Equals(Password))
                {
                    return false;
                }
            }

            if (Password.Equals(CUName))
            {
                return false;
            }
            if (Password.Equals(LoginName))
            {
                return false;
            }

            //检查密码是否是同一字符
            int y = 0;
            for (int i = 0; i < Password.Length; i++)
            {
                if (Password[i].Equals(Password[0]))
                {
                    y++;
                }
            }

            if (y == Password.Length)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public const string MailAddress = "sonctech@gmail.com";
        /// <summary>
        /// 邮箱密码
        /// </summary>
        public const string MailPassword = "xk123!@#";
        /// <summary>
        /// 邮件昵称
        /// </summary>
        public const string MailNickName = "万券 wquan.cn";

        /// <summary>
        /// 邮件服务器地址
        /// </summary>
        public const string MailSmtpServer = "smtp.gmail.com";


        public const string AntiForgeryTokenSalt = "wquan";
    }
}
