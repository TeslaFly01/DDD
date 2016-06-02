using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain
{
    /// <summary>
    /// 分页数据集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageData<T>
    {
        public PageData()
        {
            this.TotalCount = 0;
            this.DataList = null;
            this.CurrentPageIndex = 0;
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 当前页数据集合
        /// </summary>
        public IList<T> DataList { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int CurrentPageIndex { get; set; }
    }
}
