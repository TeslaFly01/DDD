using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain;
using DDD.Domain.MainModule.Admin;
using DDD.Application.Service.CacheService;
using DDD.Domain.Specification;
using System.Transactions;

namespace DDD.Application.Service.BusinessService.Admin
{
    public class AdminModuleService : BaseServices<AdminModule>, IAdminModuleService
    {
        // private readonly AdminCacheService _adminCacheService; 暂时不考虑缓存
        private readonly IAdminModuleDomainService _adminModuleDomainService;
        private readonly IAdminModuleRepository _repository;
        private readonly IAdminLogService _adminLogService;
        public AdminModuleService(IUnitOfWork unitOfWork, IAdminModuleRepository repository, IAdminModuleDomainService adminModuleDomainService, IAdminLogService adminLogService)
            : base(unitOfWork, repository)
        {
            _adminModuleDomainService = adminModuleDomainService;
            _repository = repository;
            _adminLogService = adminLogService;
        }

        public void Move(int amid, bool flag)
        {
            _adminModuleDomainService.Move(amid, flag);
            base.unitOfWork.Commit();
        }

        public void Enable(string ids, bool isEnable)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                string newids = ids.Substring(0, ids.Length - 1);
                string[] moduleids = newids.Split('|');
                foreach (var id in moduleids)
                {
                    _repository.Enable(int.Parse(id), isEnable);
                }
                base.unitOfWork.Commit();
                string mes = "功能模块";
                if (isEnable)
                    mes += "启用";
                else
                    mes += "禁用";
                _adminLogService.Log(mes, "操作ID:" + ids);
                scope.Complete();
            }
        }

        public void DeleteList(string ids)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                string newids = ids.Substring(0, ids.Length - 1);
                string[] moduleids = newids.Split('|');
                _adminModuleDomainService.DeleteList(moduleids);
                base.unitOfWork.Commit();
                _adminLogService.Log("批量删除功能模块", "删除ID:" + ids);
                scope.Complete();
            }
        }

        public string GetCheckStr(IEnumerable<AdminRole_Module> checkids)
        {
            return _adminModuleDomainService.GetCheckStr(checkids);
        }
       

        public AdminModule GetOneByPageUrl(string PageUrl)
        {
            return base.GetByCondition(new DirectSpecification<AdminModule>(am => am.PageUrl == PageUrl),true);
        }


        public void DeleteModule(int amid)
        {
            AdminModule adminM = _repository.GetByCondition(new DirectSpecification<AdminModule>(adm => adm.AMID == amid));
            using (TransactionScope scope = new TransactionScope())
            {

                _adminModuleDomainService.Delete(adminM);
                base.unitOfWork.Commit();
                _adminLogService.Log("删除功能模块", "功能模块名:" + adminM.ModuleName + " || Form身份名称:" + adminM.FormRoleName + " || CSS样式:" + adminM.CSSStyle + " || 页面地址:" + adminM.PageUrl);
                scope.Complete();
            }
        }

        public void AddModule(AdminModule am)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                base.Add(am);
                _adminLogService.Log("新增功能模块", "功能模块名:" + am.ModuleName + " || Form身份名称:" + am.FormRoleName + " || CSS样式:" + am.CSSStyle + " || 页面地址:" + am.PageUrl);

                scope.Complete();
            }
        }

        public void EditModule(AdminModule am)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                base.Modify(am);
                _adminLogService.Log("修改功能模块", "功能模块名:" + am.ModuleName + " || Form身份名称:" + am.FormRoleName + " || CSS样式:" + am.CSSStyle + " || 页面地址:" + am.PageUrl);
                scope.Complete();
            }
        }
    }
}