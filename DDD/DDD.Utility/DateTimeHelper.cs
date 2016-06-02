using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Utility
{
    public static class DateTimeHelper
    {
        public static DateTime BaseTime = new DateTime(1970, 1, 1);//Unix起始时间

        /// <summary>
        /// 转换微信DateTime时间到C#时间
        /// </summary>
        /// <param name="dateTimeFromXml">微信DateTime</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromXml(long dateTimeFromXml)
        {
            return BaseTime.AddTicks((dateTimeFromXml + 8 * 60 * 60) * 10000000);
        }
        /// <summary>
        /// 转换微信DateTime时间到C#时间
        /// </summary>
        /// <param name="dateTimeFromXml">微信DateTime</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromXml(string dateTimeFromXml)
        {
            return GetDateTimeFromXml(long.Parse(dateTimeFromXml));
        }

        /// <summary>
        /// 获取微信DateTime（UNIX时间戳）
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static long GetWeixinDateTime(DateTime dateTime)
        {
            return (dateTime.Ticks - BaseTime.Ticks) / 10000000 - 8 * 60 * 60;
        }

        public static string DateStringFromNow(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays>365)
            {
                return dt.ToString("yyyy-MM-dd");
            }
            if (span.TotalDays > 7)
            {
                return dt.ToString("MM-dd HH:mm");
            }
            if (span.TotalDays > 1)
            {
                return string.Format(" {0}天前", (int)Math.Floor(span.TotalDays));
            }
            if (span.TotalHours > 1)
            {
                return string.Format(" {0}小时前", (int)Math.Floor(span.TotalHours));
            }
            if (span.TotalMinutes > 1)
            {
                return string.Format(" {0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            if (span.TotalSeconds >= 10)
            {
                return string.Format(" {0}秒前", (int)Math.Floor(span.TotalSeconds));
            }
            return "刚刚";
        }

    }
}
