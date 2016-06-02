using System;
using System.Linq;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;

namespace DDD.Domain.MainModule.Admin.Specification
{
    public class SysAdminSpecification : Specification<SystemAdmin>
    {
        #region Members

        string _SAName = default(String);
        string _SANickName = default(String);
        int _ARID = -1;

        #endregion

        #region Constructor

        public SysAdminSpecification(string SAName, string SANickName, int ARID)
        {
            this._SAName = SAName;
            this._SANickName = SANickName;
            this._ARID = ARID;
        }

        #endregion

        public override global::System.Linq.Expressions.Expression<Func<SystemAdmin, bool>> SatisfiedBy()
        {
            Specification<SystemAdmin> beginSpec = new TrueSpecification<SystemAdmin>();

            if (!string.IsNullOrEmpty(_SAName) && !string.IsNullOrEmpty(_SAName.Trim()))
                beginSpec &= new DirectSpecification<SystemAdmin>(o => o.SAName == _SAName);

            if (!string.IsNullOrEmpty(_SANickName) && !string.IsNullOrEmpty(_SANickName.Trim()))
                beginSpec &= new DirectSpecification<SystemAdmin>(o => o.SANickName.Contains(_SANickName));

            if (_ARID != -1)
                beginSpec &= new DirectSpecification<SystemAdmin>(o => o.AdminRoles.Select(r => r.ARID).Contains(_ARID));

            return beginSpec.SatisfiedBy();
        }
    }
}
