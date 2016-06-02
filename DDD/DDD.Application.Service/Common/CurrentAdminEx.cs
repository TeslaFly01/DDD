using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace DDD.Application.Service.Common
{
    /// <summary>
    /// 当前登录系统管理员简单属性版
    /// </summary>
    public class CurrentAdminEx
    {
        public CurrentAdminEx()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated) //WCF use OperationContext.Current 
                throw new InvalidOperationException("账户未登录");//必须要用户Forms身份验证后才能使用CurrentAdmin
            FormsAuthenticationTicket ticket = (HttpContext.Current.User.Identity as FormsIdentity).Ticket;
            string[] userInfo = ticket.UserData.Split(';');
            SAName = ticket.Name;
            LoginedIP = userInfo[1];
            SAID = int.Parse(userInfo[2]);
            SANickName = userInfo[3];
            LoginedTime = ticket.IssueDate;
        }

        public string LoginedIP { get; set; }
        /// <summary>
        /// 帐号
        /// </summary>
        public string SAName { get; set; }
        public int SAID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string SANickName { get; set; }

        public DateTime LoginedTime { get; set; }
    }
}
