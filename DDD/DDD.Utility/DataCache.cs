using System;
using System.Web.Caching;
using System.Web;
using System.Collections;

namespace DDD.Utility
{
    /// <summary>
    /// 对系统缓存的操作 
    /// </summary>
    public sealed class DataCache
    {
        private static object _lock = new object();

        public DataCache()
        {
        }

        /// <summary>
        /// 取得某缓存的值
        /// </summary>
        /// <param name="CacheKey">缓存的键</param>
        /// <returns>缓存的值</returns>
        public static object GetCache(string CacheKey)
        {
            lock (_lock)
            {
                // HttpRuntime.Cache：获取当前应用程序的 System.Web.Caching.Cache。
                // System.Web.Caching.Cache：实现用于 Web 应用程序的缓存。
                Cache objCache = HttpRuntime.Cache;
                return objCache[CacheKey];
            }
        }

        /// <summary>
        /// 设置某缓存的值。
        /// </summary>
        /// <param name="CacheKey">缓存的键</param>
        /// <param name="objObject">缓存的值</param>
        public static void SetCache(string CacheKey, object objObject)
        {
            lock (_lock)
            {
                Cache objCache = HttpRuntime.Cache;
                objCache.Insert(CacheKey, objObject);
            }
        }

        /// <summary>
        /// 设置某缓存的值和跟踪缓存依赖项。
        /// </summary>
        /// <param name="CacheKey">缓存的键</param>
        /// <param name="objObject">缓存的值</param>
        /// <param name="objDependency">缓存依赖项</param>
        public static void SetCache(string CacheKey, object objObject, CacheDependency objDependency)
        {
            lock (_lock)
            {
                Cache objCache = HttpRuntime.Cache;
                objCache.Insert(CacheKey, objObject, objDependency);
            }
        }

        /// <summary>
        /// 设置某缓存的值和跟踪缓存依赖项。
        /// </summary>
        /// <param name="CacheKey">缓存的键</param>
        /// <param name="objObject">缓存的值</param>
        /// <param name="objDependency">缓存依赖项</param>
        /// <param name="AbsoluteExpiration">缓存移出的时间</param>
        /// <param name="SlidingExpiration">缓存的有效期时间长度</param>
        public static void SetCache(string CacheKey, object objObject, CacheDependency objDependency, DateTime AbsoluteExpiration, TimeSpan SlidingExpiration)
        {
            lock (_lock)
            {
                Cache objCache = HttpRuntime.Cache;
                objCache.Insert(CacheKey, objObject, objDependency, AbsoluteExpiration, SlidingExpiration);
            }
        }

        /// <summary>
        /// 设置某缓存的值。
        /// </summary>
        /// <param name="CacheKey">缓存的键</param>
        /// <param name="objObject">缓存的值</param>
        /// <param name="SlidingExpiration">缓存的有效期时间长度</param>
        public static void SetCache(string CacheKey, object objObject, TimeSpan SlidingExpiration)
        {
            lock (_lock)
            {
                Cache objCache = HttpRuntime.Cache;
                objCache.Insert(CacheKey, objObject, null, Cache.NoAbsoluteExpiration, SlidingExpiration);
            }
        }

        /// <summary>
        ///  设置某缓存的值。
        /// </summary>
        /// <param name="CacheKey">缓存的键</param>
        /// <param name="objObject">缓存的值</param>
        /// <param name="AbsoluteExpiration">缓存移出的时间</param>
        public static void SetCache(string CacheKey, object objObject, DateTime AbsoluteExpiration)
        {
            lock (_lock)
            {
                Cache objCache = HttpRuntime.Cache;
                objCache.Insert(CacheKey, objObject, null, AbsoluteExpiration, Cache.NoSlidingExpiration);
            }
        }

        /// <summary>
        /// 从缓存中移出某键的缓存值
        /// </summary>
        /// <param name="CacheKey">缓存的键</param>
        public static void RemoveCache(string CacheKey)
        {
            lock (_lock)
            {
                Cache objCache = HttpRuntime.Cache;
                if (objCache[CacheKey] != null)
                {
                    objCache.Remove(CacheKey);
                }
            }
        }

        //清除所有缓存
        public static void RemoveAllCache()
        {
            lock (_lock)
            {
                System.Web.Caching.Cache _cache = HttpRuntime.Cache;
                IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
                ArrayList al = new ArrayList();
                while (CacheEnum.MoveNext())
                {
                    al.Add(CacheEnum.Key);
                }
                foreach (string key in al)
                {
                    _cache.Remove(key);
                }
            }
        }


        /* 常见问题#Cache显示与清空问题


        List<string> cacheKeys = new List<string>();
        IDictionaryEnumerator cacheEnum = Cache.GetEnumerator();
        while (cacheEnum.MoveNext())
        {
            cacheKeys.Add(cacheEnum.Key.ToString());
        }
        foreach (string cacheKey in cacheKeys)
        {
            Cache.Remove(cacheKey);
        }
 

            //显示所有缓存 
            void show()
            {
                string str = "";
                IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();

                while (CacheEnum.MoveNext())
                {
                    str += "缓存名<b>[" + CacheEnum.Key + "]</b><br />";
                }
                this.Label1.Text = "当前网站总缓存数:" + HttpRuntime.Cache.Count + "<br />" + str;
            }
           */
    }
}
