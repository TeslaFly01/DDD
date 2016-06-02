using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;

namespace DDD.Domain.MainModule.Test.Specification
{
    public class UserInfoSpecification : Specification<UserInfo>
    {
        private string _name = string.Empty;
        private int? _sex = null;

        public UserInfoSpecification(string name, int? sex)
        {
            _name = name;
            _sex = sex;
        }

        public override Expression<Func<UserInfo, bool>> SatisfiedBy()
        {
            Specification<UserInfo> beginspec = new TrueSpecification<UserInfo>();
            if (!string.IsNullOrEmpty(_name))
            {
                beginspec &= new DirectSpecification<UserInfo>(x => x.Name.Contains(_name));
            }
            if (_sex.HasValue)
            {
                beginspec &= new DirectSpecification<UserInfo>(x => x.Sex == _sex.Value);
            }
            return beginspec.SatisfiedBy();
        }
    }
}
