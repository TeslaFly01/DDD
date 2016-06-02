using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DDD.Infrastructure.CrossCutting.Common.Validation
{
    /// <summary>
    /// 验证输入的数值是2的阶梯次方
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class TwoSqrtAttribute : ValidationAttribute, IClientValidatable
    {
        // public string InputString { get; set; }
        public TwoSqrtAttribute()
        {
            ErrorMessage = "验证失败";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            int inputString = (int)value;
            bool re = inputString != 0 && ((inputString - 1) & inputString) == 0;
            return re;
        }

        #region IClientValidatable 成员

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "twosqrt",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            //rule.ValidationParameters["inputstring"] = InputString;
            yield return rule;
        }

        #endregion
    }
}
