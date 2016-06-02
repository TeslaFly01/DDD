using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DDD.Utility
{
    public class ChineseCalendarFormatter : IFormatProvider, ICustomFormatter
    {
        //实现IFormatProvider返回当前格式化对象
        public object GetFormat(Type formatType)
        {

            if (formatType == typeof(ICustomFormatter))

                return this;

            else

                return Thread.CurrentThread.CurrentCulture.GetFormat(formatType);

        }


        //实现ICustomFormatter返回农历
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {

            string s;

            IFormattable formattable = arg as IFormattable;

            if (formattable == null)

                s = arg.ToString();

            else

                s = formattable.ToString(format, formatProvider);

            if (arg.GetType() == typeof(DateTime))
            {

                DateTime time = (DateTime)arg;

                switch (format)
                {

                    case "D": //长日期格式

                        s = String.Format("{0}年{1}月{2}",

                            ChineseCalendarHelper.GetYear(time),

                            ChineseCalendarHelper.GetMonth(time),

                            ChineseCalendarHelper.GetDay(time));

                        break;

                    case "d": //短日期格式

                        s = String.Format("{0}年{1}月{2}", ChineseCalendarHelper.GetStemBranch(time),

                            ChineseCalendarHelper.GetMonth(time),

                            ChineseCalendarHelper.GetDay(time));

                        break;

                    case "M": //月日格式

                        s = String.Format("{0}月{1}", ChineseCalendarHelper.GetMonth(time),

                            ChineseCalendarHelper.GetDay(time));

                        break;

                    case "Y": //年月格式

                        s = String.Format("{0}年{1}月", ChineseCalendarHelper.GetYear(time),

                            ChineseCalendarHelper.GetMonth(time));

                        break;

                    default:

                        s = String.Format("{0}年{1}月{2}", ChineseCalendarHelper.GetYear(time),

                            ChineseCalendarHelper.GetMonth(time),

                            ChineseCalendarHelper.GetDay(time));

                        break;

                }

            }

            return s;

        }

    }
}
