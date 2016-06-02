using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Application.Service.Common;
using DDD.Domain;
using DDD.Domain.MainModule.Admin;
using DDD.Domain.MainModule.Admin.Specification;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;

namespace DDD.Application.Service.BusinessService.Admin
{
    public class AdminLogService : BaseServices<AdminLog>, IAdminLogService
    {
        public AdminLogService(IUnitOfWork unitOfWork, IAdminLogRepository repository)
            : base(unitOfWork, repository)
        {
        }

        /// <summary>
        /// 登录成功后，记录日志
        /// </summary>
        public void Log(string OptContent, string OptRemark)
        {
            var currentAdminEx = new CurrentAdminEx();
            AdminLog entity = new AdminLog() { OptContent = OptContent, OptRemark = OptRemark };
            entity.IP = currentAdminEx.LoginedIP;
            entity.UserID = currentAdminEx.SAID;
            entity.UserName = currentAdminEx.SAName;
            entity.UserNickName = currentAdminEx.SANickName;
            base.Add(entity);
        }

        /// <summary>
        /// 登录成功前，记录日志
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="OptContent"></param>
        /// <param name="OptRemark"></param>
        public void Log(SystemAdmin admin, string OptContent, string OptRemark)
        {
            var entity = new AdminLog
            {
                OptContent = OptContent,
                OptRemark = OptRemark,
                IP = admin.CurrentIP,
                UserID = admin.SAID,
                UserName = admin.SAName,
                UserNickName = admin.SANickName
            };
            base.Add(entity);
        }

        public PageData<AdminLog> Search(int PageIndex, int PageSize, string OptContent, string UserName, DateTime? FromOptTime, DateTime? ToOptTime)
        {
            AdminLogSpecification sepci = new AdminLogSpecification(OptContent, UserName, FromOptTime, ToOptTime);
            PageData<AdminLog> searchList = base.FindAll<DateTime>(PageIndex, PageSize, sepci as ISpecification<AdminLog>, x => (DateTime)x.OptTime, true);
            return searchList;
        }

        public void DeleteSome(string[] ids)
        {
            List<int> result = new List<string>(ids).ConvertAll(id => int.Parse(id));
            base.Remove(new DirectSpecification<AdminLog>(x => result.Contains(x.LogID)));
        }

        public void DeleteBeforeDate(DateTime dt)
        {
            base.Remove(new DirectSpecification<AdminLog>(x => x.OptTime < dt));
        }

    }
}
