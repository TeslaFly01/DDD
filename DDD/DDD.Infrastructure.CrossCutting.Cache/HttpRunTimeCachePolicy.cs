using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DDD.Utility;

namespace DDD.Infrastructure.CrossCutting.Cache
{
    public class HttpRunTimeCachePolicy : ICachePolicy
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger("LogError");

        #region ICachePolicy 成员

        public void Add<T>(string key, T value)
        {
            DataCache.SetCache(key, value);
        }

        public void Add<T>(string key, T value, DateTime dt)
        {
            DataCache.SetCache(key, value, dt);
        }

        public void Add<T>(string key, T value, TimeSpan dt)
        {
            DataCache.SetCache(key, value, dt);
        }

        public T Get<T>(string key)
        {
            try
            {
                return (T)DataCache.GetCache(key);
            }
            catch (Exception e)
            {
                var httpbc = HttpContext.Current.Request.Browser;
                _log.Error("\r\n请求地址：" + HttpContext.Current.Request.Url + " || 错误信息：Get _cache for key failed, key[" + key + "] || 客户端IP：" + HttpContext.Current.Request.UserHostAddress + " || 客户端信息：" + "[" + httpbc.Browser + "][" + httpbc.Version + "][" + httpbc.Platform + "]", e);
                DataCache.RemoveCache(key);
                return default(T);
            }

        }

        public void Add(string key, object value)
        {
            DataCache.SetCache(key, value);
        }

        public void Add(string key, object value, DateTime dt)
        {
            DataCache.SetCache(key, value, dt);
        }

        public void Add(string key, object value, TimeSpan dt)
        {
            DataCache.SetCache(key, value, dt);
        }

        public object Get(string key)
        {
            return DataCache.GetCache(key);
        }

        public void Delete(string key)
        {
            DataCache.RemoveCache(key);
        }

        #endregion
    }
}
