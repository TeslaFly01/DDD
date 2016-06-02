using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDD.Utility
{
    public static class DomainHelper
    {
        /// <summary>
        /// 解析网址字符中的根域名
        /// </summary>
        /// <param name="domain">httpContext.Request.Url.Host</param>
        /// <returns>网站的域名，不带"Http://"，如wquan.cn</returns>
        public static string GetRootDomain(string domain)
        {
            if (string.IsNullOrEmpty(domain))
            {
                throw new ArgumentNullException("参数'domain'不能为空");
            }
            string[] arr = domain.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length <= 2)
            {
                return domain;
            }
            else
            {
                return arr[arr.Length - 2] + "." + arr[arr.Length - 1];
            }
        }
    }
}
