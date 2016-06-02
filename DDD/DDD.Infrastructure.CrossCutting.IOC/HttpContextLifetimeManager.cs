using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Practices.Unity;

namespace DDD.Infrastructure.CrossCutting.IOC
{
    public class HttpContextLifetimeManager<T> : LifetimeManager, IDisposable
    {
        private static object _lock = new object();
        public override object GetValue()
        {
            lock (_lock)
            {
                return HttpContext.Current.Items[typeof(T).AssemblyQualifiedName];
            }
        }
        public override void RemoveValue()
        {
            lock (_lock)
            {
                HttpContext.Current.Items.Remove(typeof(T).AssemblyQualifiedName);
            }
        }
        public override void SetValue(object newValue)
        {
            lock (_lock)
            {
                HttpContext.Current.Items[typeof(T).AssemblyQualifiedName] = newValue;
            }
        }
        public void Dispose()
        {
            RemoveValue();
        }
    }
}
