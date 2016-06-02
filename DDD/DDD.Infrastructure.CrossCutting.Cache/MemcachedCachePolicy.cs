using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BeIT.MemCached;

namespace DDD.Infrastructure.CrossCutting.Cache
{
    public class MemcachedCachePolicy : ICachePolicy
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger("LogError");

        private readonly MemcachedClient _cache;

        public MemcachedCachePolicy()
        {
            _cache = MemcachedClient.GetInstance("MemcachedConfig");

            _cache.MaxPoolSize = 10000;
        }

        public void Add<T>(string key, T value)
        {
            if (_cache.Set(key, value))
            {
                //Logger.Debug("Set _cache for key successed, key[" + key + "]");
            }
            else
            {
                //Logger.Debug("Set _cache for key failed");
                var httpbc = HttpContext.Current.Request.Browser;
                _log.Error("\r\n请求地址：" + HttpContext.Current.Request.Url + " || 错误信息：Set _cache for key failed, key[" + key + "] || 客户端IP：" + HttpContext.Current.Request.UserHostAddress + " || 客户端信息：" + "[" + httpbc.Browser + "][" + httpbc.Version + "][" + httpbc.Platform + "]");
            }
        }

        public void Add<T>(string key, T value, DateTime dt)
        {
            _cache.Set(key, value, dt);
        }

        public void Add<T>(string key, T value, TimeSpan dt)
        {
            _cache.Set(key, value, dt);
        }


        public T Get<T>(string key)
        {
            try
            {
                return (T)_cache.Get(key);
            }
            catch (Exception e)
            {
                var httpbc = HttpContext.Current.Request.Browser;
                _log.Error("\r\n请求地址：" + HttpContext.Current.Request.Url + " || 错误信息：Get _cache for key failed, key[" + key + "] || 客户端IP：" + HttpContext.Current.Request.UserHostAddress + " || 客户端信息：" + "[" + httpbc.Browser + "][" + httpbc.Version + "][" + httpbc.Platform + "]", e);
                _cache.Delete(key);
                return default(T);
            }
        }

        public void Add(string key, object value)
        {
            _cache.Set(key, value);
        }

        public void Add(string key, object value, DateTime dt)
        {
            _cache.Set(key, value, dt);
        }

        public void Add(string key, object value, TimeSpan dt)
        {
            _cache.Set(key, value, dt);
        }

        public object Get(string key)
        {
            return _cache.Get(key);
        }

        public void Delete(string key)
        {
            _cache.Delete(key);
        }
    }
}
