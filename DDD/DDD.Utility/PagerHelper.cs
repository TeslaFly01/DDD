using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace DDD.Utility
{
    /// <summary>
    /// 分页控件帮助类
    /// </summary>
    public class PagerHelper
    {
        /// <summary>
        /// 提交的PageIndex参数大于页数时，更新数据源控件参数0－"PageIndex"
        /// </summary>
        /// <param name="itfc"></param>
        /// <param name="pageSize"></param>
        /// <param name="obds"></param>
        public static void UpdatePageIndex(IPage itfc, int pageSize, ObjectDataSource obds)
        {
            if (StrUtil.IsPosInt(HttpContext.Current.Request["PageIndex"]))
            {
                int PageCount = DataLogicalCommon.getPageCount(itfc, pageSize);
                if (int.Parse(HttpContext.Current.Request["PageIndex"]) > PageCount)
                {
                    obds.SelectParameters.RemoveAt(0);
                    obds.SelectParameters.Insert(0, new Parameter("PageIndex", TypeCode.Int32, PageCount.ToString()));
                }
            }
        }


        public static int getPageIndex(IPage itfc, int pageSize)
        {
            int myPIndex = RequestHelper.GetQueryPosInt("PageIndex", 1);
            int PageCount = DataLogicalCommon.getPageCount(itfc, pageSize);
            if (myPIndex > PageCount)
            {
                return PageCount;
            }
            return myPIndex;
        }
    }
}
