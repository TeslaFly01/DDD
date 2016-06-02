using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain;
using DDD.Domain.MainModule.Admin;
using DDD.Domain.Specification;

namespace DDD.Application.Service.BusinessService.Admin
{
    public class AdminRole_ModuleService : BaseServices<AdminRole_Module>, IAdminRole_ModuleService
    {
        private readonly IAdminRole_ModuleDomainService _adminRole_ModuleService;
        public AdminRole_ModuleService(IUnitOfWork unitOfWork, IAdminRole_ModuleRepository repository, IAdminRole_ModuleDomainService adminRole_ModuleService)
            : base(unitOfWork, repository)
        {
            _adminRole_ModuleService = adminRole_ModuleService;
        }

        public void AddList(string[] moduleids, int arid)
        {
            _adminRole_ModuleService.AddList(moduleids, arid);
            this.unitOfWork.Commit();
        }


        public void UpdateList(string[] moduleids, int arid)
        {
            _adminRole_ModuleService.UpdateList(moduleids, arid);
            this.unitOfWork.Commit();
        }
    }
}
