using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Infrastructure.CrossCutting.Cache;

namespace DDD.Application.Service.CacheService
{
    public class AdminCacheService : BaseCacheService
    {
        //Key:

        //当前登录管理员
        public const string SysAdmin_Current_prefix = "SysAdmin_Current_";

        public AdminCacheService(ICachePolicy cachePolicy)
            : base(cachePolicy)
        {
            //TO DO:可以设置一些Key命名规则 等等
        }

    }
}
