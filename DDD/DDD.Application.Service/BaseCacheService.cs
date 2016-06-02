using System;
using DDD.Infrastructure.CrossCutting.Cache;

namespace DDD.Application.Service
{
    public abstract class BaseCacheService
    {
        protected readonly ICachePolicy _cachePolicy;

        public BaseCacheService(ICachePolicy cachePolicy)
        {
            _cachePolicy = cachePolicy;
        }

        public T GetCache<T>(string key) where T : class
        {
            return _cachePolicy.Get<T>(key);
        }

        public void Add<T>(string key, T items)
        {
            _cachePolicy.Add(key, items);
        }

        public void Add<T>(string key, T items, DateTime dt)
        {
            _cachePolicy.Add(key, items, dt);
        }

        public void Add<T>(string key, T items, TimeSpan dt)
        {
            _cachePolicy.Add(key, items, dt);
        }

        public T GetOrAdd<T>(string key, T loadedItems) where T : class
        {
            var items = _cachePolicy.Get<T>(key);

            if (items == null)
            {
                _cachePolicy.Add(key, loadedItems);
                return loadedItems;
            }

            return items;
        }

        public T GetOrAdd<T>(string key, Func<T> howToGet) where T : class
        {
            var items = _cachePolicy.Get<T>(key);


            if (items == null)
            {
                var loadedItems = howToGet();
                _cachePolicy.Add(key, loadedItems);
                return loadedItems;
            }

            var type = items.GetType();
            if (type == typeof(int) && items.Equals(0))
            {
                var loadedItems = howToGet();
                _cachePolicy.Add(key, loadedItems);
                return loadedItems;
            }

            return items;
        }

        public T GetOrAdd<T>(string key, Func<T> howToGet, DateTime dt) where T : class
        {
            var items = _cachePolicy.Get<T>(key);
            if (items == null)
            {
                var loadedItems = howToGet();
                _cachePolicy.Add(key, loadedItems, dt);
                return loadedItems;
            }

            var type = items.GetType();
            if (type == typeof(int) && items.Equals(0))
            {
                var loadedItems = howToGet();
                _cachePolicy.Add(key, loadedItems, dt);
                return loadedItems;
            }
            return items;
        }

        public T GetOrAdd<T>(string key, Func<T> howToGet, TimeSpan dt) where T : class
        {
            var items = _cachePolicy.Get<T>(key);
            if (items == null)
            {
                var loadedItems = howToGet();
                _cachePolicy.Add(key, loadedItems, dt);
                return loadedItems;
            }

            var type = items.GetType();
            if (type == typeof(int) && items.Equals(0))
            {
                var loadedItems = howToGet();
                _cachePolicy.Add(key, loadedItems, dt);
                return loadedItems;
            }
            return items;
        }

        public void Remove(string key)
        {
            _cachePolicy.Delete(key);
        }

    }
}
