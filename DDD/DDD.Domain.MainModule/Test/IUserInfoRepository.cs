using System.Collections.Generic;
using DDD.Domain.Model.Entities;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;
using DDD.Infrastructur.Repositories;

namespace DDD.Domain.MainModule.Test
{
    public interface IUserInfoRepository : IRepository<UserInfo, PageData<UserInfo>>
    {
        List<UserInfo> GetList(ISpecification<UserInfo> specification);

        void ChangeScanningToWait();

        List<string> GetNotNoticeList(int topNum);

        void BatchNotNoticeScaning(List<string> idList);
    }
}
