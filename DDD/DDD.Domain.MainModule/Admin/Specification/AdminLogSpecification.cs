using System;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;

namespace DDD.Domain.MainModule.Admin.Specification
{
    public class AdminLogSpecification : Specification<AdminLog>
    {
        #region Members

        string _optContent = string.Empty;
        string _userName = string.Empty;
        DateTime? _fromOptTime = null;
        DateTime? _toOptTime = null;

        #endregion

        public AdminLogSpecification(string OptContent, string UserName, DateTime? FromOptTime, DateTime? ToOptTime)
        {
            _toOptTime = ToOptTime;
            _fromOptTime = FromOptTime;
            _userName = UserName;
            _optContent = OptContent;
        }



        public override global::System.Linq.Expressions.Expression<Func<AdminLog, bool>> SatisfiedBy()
        {
            Specification<AdminLog> spec = new TrueSpecification<AdminLog>();

            if (!string.IsNullOrEmpty(_optContent) && !string.IsNullOrEmpty(_optContent.Trim()))
                spec &= new DirectSpecification<AdminLog>(o => o.OptContent.Contains(_optContent));

            if (!string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_userName.Trim()))
                spec &= new DirectSpecification<AdminLog>(o => o.UserName == _userName);

            if (_fromOptTime.HasValue)
                spec &= new DirectSpecification<AdminLog>(o => o.OptTime >= (_fromOptTime ?? DateTime.MinValue));

            if (_toOptTime.HasValue)
                spec &= new DirectSpecification<AdminLog>(o => o.OptTime <= (_toOptTime ?? DateTime.MaxValue));

            return spec.SatisfiedBy();
        }
    }
}
