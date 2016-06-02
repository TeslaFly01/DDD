using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.Model.VO
{
    public class ShowResultModel
    {
        public ShowResultModel()
        {
            IsSuccess = false;
        }

        /// <summary>
        /// 提示页面名称
        /// </summary>
        public string PageTitle { get; set; }
        /// <summary>
        /// 提示信息内容
        /// </summary>
        public string TipMsg { get; set; }

        public string ReDirectTitle { get; set; }

        /// <summary>
        /// 转向的URL
        /// </summary>
        public string ReDirectUrl { get; set; }
        /// <summary>
        /// 停止时间 超过时间自动跳转
        /// </summary>
        public int Delay { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 备用的特殊标志位
        /// </summary>
        public int SFlag { get; set; }
        /// <summary>
        /// 备用额外对象属性
        /// </summary>
        public object TObj { get; set; }
    }
}
