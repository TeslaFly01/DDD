using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;

namespace DDD.Domain.MainModule.Test
{
    public interface IUserInfoDomainService
    {
        List<UserInfo> GetList(ISpecification<UserInfo> specification);

        List<string> GetNotNoticeList(int num);
    }
}
