using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Application.Service.BusinessService.Log;
using DDD.Domain;
using DDD.Domain.Model.Entities.Admin;

namespace DDD.Application.Service.BusinessService.Admin
{
    public interface IAdminLogService : IBaseServices<AdminLog>, ILogService
    {

        /// <summary>
        /// 登录成功前，记录日志
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="OptContent"></param>
        /// <param name="OptRemark"></param>
        void Log(SystemAdmin admin, string OptContent, string OptRemark);

        /// <summary>
        /// 查询分页列表
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="OptContent">操作内容</param>
        /// <param name="UserName">用户帐号</param>
        /// <param name="FromOptTime">操作时间起</param>
        /// <param name="ToOptTime">操作时间至</param>
        /// <returns></returns>
        PageData<AdminLog> Search(int PageIndex, int PageSize, string OptContent, string UserName, DateTime? FromOptTime, DateTime? ToOptTime);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        void DeleteSome(string[] ids);
        /// <summary>
        /// 删除时间之前的日志
        /// </summary>
        /// <param name="dt"></param>
        void DeleteBeforeDate(DateTime dt);
    }
}
