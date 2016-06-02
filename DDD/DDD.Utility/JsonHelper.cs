using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace DDD.Utility
{
    public static class JsonHelper
    {
        /// <summary>
        /// 替换JSON特殊字符
        /// (WCF restful 会自动处理特殊字符 ，不需要再调用这个方法)
        /// </summary>
        /// <param name="jsonPara"></param>
        /// <returns></returns>
        public static string Encode(string jsonPara)
        {
            string re = string.Empty;
            if (!string.IsNullOrEmpty(jsonPara))
                re = jsonPara.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\t", "\\\t").Replace("\r", "\\\r").Replace("\n", "\\\n").Replace("\f", "\\\f").Replace("\b", "\\\b");
            return re;
        }

        public static string Serialize(object o)
        {
            return new JavaScriptSerializer().Serialize(o);
        }

        public static void Serialize(object o, StringBuilder output)
        {
            new JavaScriptSerializer().Serialize(o, output);
        }

        public static T Deserialize<T>(string input)
        {
            return new JavaScriptSerializer().Deserialize<T>(input);
        }
    }
}
