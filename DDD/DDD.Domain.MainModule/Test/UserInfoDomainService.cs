using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;

namespace DDD.Domain.MainModule.Test
{
    public class UserInfoDomainService : IUserInfoDomainService
    {
        private IUserInfoRepository _userInfoRepository;

        public UserInfoDomainService(IUserInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        public List<UserInfo> GetList(ISpecification<UserInfo> specification)
        {
            return _userInfoRepository.GetList(specification);
        }

        public List<string> GetNotNoticeList(int num)
        {
            var list = _userInfoRepository.GetNotNoticeList(num);
            if (list.Any())
            {
                _userInfoRepository.BatchNotNoticeScaning(list);
            }
            return list;
        }
    }
}
