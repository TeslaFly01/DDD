using DDD.Application.Service.BusinessService;
using DDD.Application.Service.BusinessService.Admin;
using DDD.Application.Service.Common;
using DDD.Domain;
using DDD.Domain.MainModule.Admin;
using DDD.Domain.MainModule.Test;
using DDD.Infrastructur.Repositories.Sql;
using DDD.Infrastructur.Repositories.Sql.MainModule.Admin;
using DDD.Infrastructur.Repositories.Sql.MainModule.Test;
using DDD.Infrastructure.CrossCutting.Cache;
using Microsoft.Practices.Unity;

namespace DDD.Infrastructure.CrossCutting.IOC
{
    public sealed class UnityContainerFactory
    {
        #region Singleton

        static readonly UnityContainerFactory instance = new UnityContainerFactory();

        /// <summary>
        /// Get singleton instance of UnityContainerFactory
        /// </summary>
        public static UnityContainerFactory Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region Members

        IUnityContainer _CurrentContainer;

        /// <summary>
        /// Get current configured IContainer
        /// <remarks>
        /// At this moment only IoCUnityContainer existss
        /// </remarks>
        /// </summary>
        public IUnityContainer CurrentContainer
        {
            get
            {
                return _CurrentContainer;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Only for singleton pattern, remove before field init IL annotation
        /// </summary>
        static UnityContainerFactory() { }
        UnityContainerFactory()
        {
            _CurrentContainer = GetUnityContainer();
        }

        #endregion

        private IUnityContainer GetUnityContainer()
        {
            //Create UnityContainer          
            IUnityContainer container = new UnityContainer()

                //用户信息
                .RegisterType<IUserInfoRepository, UserInfoRepository>(new HttpContextLifetimeManager<IUserInfoRepository>())
                .RegisterType<IUserInfoDomainService, UserInfoDomainService>(new HttpContextLifetimeManager<IUserInfoDomainService>())
                .RegisterType<IUserInfoService, UserInfoService>(new HttpContextLifetimeManager<IUserInfoService>())
                //系统管理员
                .RegisterType<ISystemAdminService, SystemAdminService>(new HttpContextLifetimeManager<ISystemAdminService>())
                .RegisterType<ISystemAdminDomainService, SystemAdminDomainService>(new HttpContextLifetimeManager<ISystemAdminDomainService>())
                .RegisterType<ISystemAdminRepository, SystemAdminRepository>(new HttpContextLifetimeManager<ISystemAdminRepository>())

                //管理员功能模块
                .RegisterType<IAdminActionService, AdminActionService>(new HttpContextLifetimeManager<IAdminActionService>())
                .RegisterType<IAdminActionDomainService, AdminActionDomainService>(new HttpContextLifetimeManager<IAdminActionDomainService>())
                .RegisterType<IAdminActionRepository, AdminActionRepository>(new HttpContextLifetimeManager<IAdminActionRepository>())

                //管理员操作日志
                .RegisterType<IAdminLogService, AdminLogService>(new HttpContextLifetimeManager<IAdminLogService>())
                .RegisterType<IAdminLogRepository, AdminLogRepository>(new HttpContextLifetimeManager<IAdminLogRepository>())//Cache

                //管理员角色功能模块
                .RegisterType<IAdminRole_ModuleService, AdminRole_ModuleService>(new HttpContextLifetimeManager<IAdminRole_ModuleService>())
                .RegisterType<IAdminRole_ModuleDomainService, AdminRole_ModuleDomainService>(new HttpContextLifetimeManager<IAdminRole_ModuleDomainService>())
                .RegisterType<IAdminRole_ModuleRepository, AdminRole_ModuleRepository>(new HttpContextLifetimeManager<IAdminRole_ModuleRepository>())

                //管理员角色
                .RegisterType<IAdminRoleService, AdminRoleService>(new HttpContextLifetimeManager<IAdminRoleService>())
                .RegisterType<IAdminRoleDomainService, AdminRoleDomainService>(new HttpContextLifetimeManager<IAdminRoleService>())
                .RegisterType<IAdminRoleRepository, AdminRoleRepository>(new HttpContextLifetimeManager<IAdminRoleRepository>())

                //管理员功能模块
                .RegisterType<IAdminModuleService, AdminModuleService>(new HttpContextLifetimeManager<IAdminModuleService>())
                .RegisterType<IAdminModuleDomainService, AdminModuleDomainService>(new HttpContextLifetimeManager<IAdminModuleService>())
                .RegisterType<IAdminModuleRepository, AdminModuleRepository>(new HttpContextLifetimeManager<IAdminModuleRepository>())


                .RegisterType<CurrentAdmin, CurrentAdmin>(new HttpContextLifetimeManager<CurrentAdmin>())
                .RegisterType<ICachePolicy, HttpRunTimeCachePolicy>(new HttpContextLifetimeManager<ICachePolicy>())

                //基础数据库工厂
                .RegisterType<IDatabaseFactory, DatabaseFactory>(new HttpContextLifetimeManager<IDatabaseFactory>())
                //基础工作单元
                .RegisterType<IUnitOfWork, UnitOfWork>(new HttpContextLifetimeManager<IUnitOfWork>());

            return container;
        }
    }
}
