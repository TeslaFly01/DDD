using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Infrastructure.CrossCutting.Common
{
    public class TreeHelper
    {
        /// <summary>
        /// 构造树型下拉菜单的符号
        /// </summary>
        /// <param name="_Deep">树的深度</param>
        /// <returns></returns>
        public static string GetDeepStr(int _Deep)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < _Deep; i++)
            {
                result.Append("　");
            }
            if (_Deep != 0)
            {
                result.Append("├");
            }

            return result.ToString();
        }
    }
}
