using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.Service.BusinessService.Log
{
    public interface ILogService
    {
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="OptContent">操作内容</param>
        /// <param name="OptRemark">详细描述</param>
        void Log(string OptContent, string OptRemark);
    }
}
