using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;

namespace DDD.Application.Service.BusinessService
{
    public interface IUserInfoService : IBaseServices<UserInfo>
    {
        List<UserInfo> GetList(string name, int? sex);

        bool ResetNotNoticeScanningToWait();

        List<string> GetNotNoticeList(int topNum);

        void PostNotNotice(string strId);
    }
}
