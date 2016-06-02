using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Infrastructure.CrossCutting.Cache
{
    public interface ICachePolicy
    {

        void Add<T>(string key, T value);

        void Add<T>(string key, T value, DateTime dt);

        void Add<T>(string key, T value, TimeSpan dt);

        T Get<T>(string key);

        void Add(string key, object value);

        void Add(string key, object value, DateTime dt);

        void Add(string key, object value, TimeSpan dt);

        object Get(string key);

        void Delete(string key);

    }
}
