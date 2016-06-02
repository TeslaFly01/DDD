using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Application.MVC.Core.Controllers;
using DDD.Application.MVC.Filters;

namespace DDD.Application.MVC.Controllers
{
    /// <summary>
    /// 系统管理后台Controller基类
    /// </summary>
    [AdminAuthorize]
    public abstract class BaseAdminControllers : BaseControllers
    {

    }
}
