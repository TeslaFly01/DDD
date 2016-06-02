using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain;
using DDD.Domain.MainModule.Admin;
using System.Transactions;
using DDD.Domain.Specification;
using DDD.Application.Service.BusinessService.Admin;

namespace DDD.Application.Service.BusinessService.Admin
{
    public class AdminRoleService : BaseServices<AdminRole>, IAdminRoleService
    {
        private readonly IAdminRole_ModuleService _adminR_MService;
        private readonly IAdminRoleDomainService _adminRoleDomainService;
        private readonly IAdminLogService _adminLogService;
        public AdminRoleService(IUnitOfWork unitOfWork, IAdminRoleRepository repository, IAdminRole_ModuleService adminR_MService, IAdminRoleDomainService adminROleDomainService, IAdminLogService adminLogService)
            : base(unitOfWork, repository)
        {
            _adminR_MService = adminR_MService;
            _adminRoleDomainService = adminROleDomainService;
            _adminLogService = adminLogService;
        }

        public void AddRole(AdminRole arl, string moduleids)
        {
            //角色唯一性判断
            bool isexists = base.Exists(new DirectSpecification<AdminRole>(a => a.ARName.Trim().ToLower() == arl.ARName.Trim().ToLower()));
            if (isexists)
                throw new InvalidOperationException("角色名已存在!");
            using (TransactionScope scope = new TransactionScope())
            {
                string newids = moduleids.Substring(0, moduleids.Length - 1);
                string[] ids = newids.Split('|');
                //添加角色
                base.Add(arl);
                //添加角色-功能色关联
                _adminR_MService.AddList(ids, arl.ARID);
                _adminLogService.Log("添加角色", "角色名称:" + arl.ARName + " || 角色功能ids:" + moduleids + " || 描述:" + arl.Description);

                scope.Complete();
            }
        }

        public void UpdateRole(AdminRole arl, string moduleids)
        {
            //角色唯一性判断
            string ar_name = base.GetByCondition(new DirectSpecification<AdminRole>(a => a.ARID == arl.ARID),true).ARName;
            bool isexists = base.Exists(new DirectSpecification<AdminRole>(a => a.ARName.Trim().ToLower() == arl.ARName.Trim().ToLower()));
            if(isexists&&!arl.ARName.Trim().Equals(ar_name.Trim()))
                throw new InvalidOperationException("角色名已存在!");
            using (var scope = new TransactionScope())
            {
                string newids = moduleids.Substring(0, moduleids.Length - 1);
                string[] ids = newids.Split('|');
                //修改角色
                base.Modify(arl);
                //修改角色-功能色关联
                _adminR_MService.UpdateList(ids, arl.ARID);
                _adminLogService.Log("修改角色", "角色名称:" + arl.ARName + " || 角色功能ids:" + moduleids + " || 描述:" + arl.Description);

                scope.Complete();
            }
        }


        public void DeleteRole(int arid)
        {
            AdminRole amr = base.GetByCondition(new DirectSpecification<AdminRole>(ar => ar.ARID == arid));
            using (TransactionScope scope = new TransactionScope())
            {
                _adminRoleDomainService.DeleteRole(amr);
                base.unitOfWork.Commit();
                _adminLogService.Log("删除角色", "角色名称:" + amr.ARName + " ||  描述:" + amr.Description);

                scope.Complete();
            }
        }


        public List<AdminModule> GetRoleModules(int arid)
        {
            return _adminRoleDomainService.GetRoleModules(arid);
        }


        public string GetCurrRoleSecondModule(int arid, int firstid)
        {
            return _adminRoleDomainService.GetCurrRoleSecondModule(arid, firstid);
        }

        public void UpdateWeight(int arid, string CMIDWeight)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                _adminRoleDomainService.UpdateWeight(arid, CMIDWeight);
                base.unitOfWork.Commit();
                _adminLogService.Log("修改角色权值", "功能id|权值id:" + CMIDWeight + " || 角色id:" + arid );

                scope.Complete();
            }
        }


        public string GetAllRoletoCheck(IEnumerable<AdminRole> checkids, bool disabled)
        {
            return _adminRoleDomainService.GetAllRoletoCheck(checkids, disabled);
        }


        public string GetActionCheck(IEnumerable<AdminAction> checkids, int arid)
        {
            return _adminRoleDomainService.GetActionCheck(checkids, arid);
        }
    }
}