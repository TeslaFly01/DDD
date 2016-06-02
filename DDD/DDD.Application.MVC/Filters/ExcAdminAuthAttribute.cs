using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.MVC.Filters
{
    /// <summary>
    /// 取消系统管理员权限拦截的标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ExcAdminAuthAttribute : Attribute
    {
    }
}
