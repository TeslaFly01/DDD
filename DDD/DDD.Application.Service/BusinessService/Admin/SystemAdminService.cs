using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.MainModule.Admin;
using DDD.Domain;
using DDD.Domain.Specification;
using System.Transactions;
using DDD.Domain.MainModule.Admin.Specification;
using DDD.Utility;
using DDD.Application.Service.CacheService;
using DDD.Application.Service.Common;

namespace DDD.Application.Service.BusinessService.Admin
{
    public class SystemAdminService : BaseServices<SystemAdmin>, ISystemAdminService
    {
        private readonly ISystemAdminDomainService _sysadminDomainService;
        private readonly ISystemAdminRepository _repository;
        private readonly IAdminLogService _adminLogService;
        private readonly AdminCacheService _adminCacheService;
        public SystemAdminService(IUnitOfWork unitOfWork, ISystemAdminRepository repository, ISystemAdminDomainService sysadminDomainService, IAdminLogService adminLogService, AdminCacheService adminCacheService)
            : base(unitOfWork, repository)
        {
            _sysadminDomainService = sysadminDomainService;
            _repository = repository;
            _adminLogService = adminLogService;
            _adminCacheService = adminCacheService;
        }

        public void AddSysAdmin(SystemAdmin admin, string roleids)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                string newids = roleids.Substring(0, roleids.Length - 1);
                string[] ids = newids.Split('|');

                _sysadminDomainService.AddSysAdmin(admin, ids);
                unitOfWork.Commit();
                _adminLogService.Log("添加系统管理员", "管理员姓名:" + admin.SANickName + " || 帐号:" + admin.SAName + " || 性别:" + admin.SASex + " || 注册时间:" + admin.RegTime + " || 角色id" + roleids);
                scope.Complete();
            }
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
                unitOfWork.Commit();
                string mes = "管理员";
                if (isEnable)
                {
                    mes += "启用";
                }
                else
                {
                    mes += "禁用";
                }
                _adminLogService.Log(mes, "操作ID:" + ids);
                scope.Complete();
            }
        }


        public void DeleteSysAdmin(int said)
        {
            var sysAdmin = new CurrentAdminEx();
            if (sysAdmin.SAID == said)
            {
                throw new InvalidOperationException("无法删除当前已登录管理员");
            }
            SystemAdmin admin = base.GetByCondition(new DirectSpecification<SystemAdmin>(sa => sa.SAID == said));
            using (TransactionScope scope = new TransactionScope())
            {
                Remove(admin);
                _adminLogService.Log("删除系统管理员", "管理员姓名:" + admin.SANickName + " || 帐号:" + admin.SAName + " || 性别:" + (admin.SASex ? "男" : "女") + " || 注册时间:" + admin.RegTime);
                scope.Complete();
            }
        }

        public void UpdateSysAdmin(SystemAdmin admin, string roleids)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string newids = roleids.Substring(0, roleids.Length - 1);
                string[] ids = newids.Split('|');
                _sysadminDomainService.UpdateSysAdmin(admin, ids);
                unitOfWork.Commit();
                _adminLogService.Log("修改系统管理员", "管理员姓名:" + admin.SANickName + " || 帐号:" + admin.SAName + " || 性别:" + (admin.SASex ? "男" : "女") + " || 角色id:" + roleids);
                scope.Complete();
            }
        }

        public PageData<SystemAdmin> Search(int PageIndex, int PageSize, string SAName, string SANickName, int ARID)
        {
            SysAdminSpecification sepci = new SysAdminSpecification(SAName, SANickName, ARID);
            PageData<SystemAdmin> searchList = base.FindAll<int>(PageIndex, PageSize, sepci as ISpecification<SystemAdmin>, x => (int)x.SAID, true, false);
            return searchList;
        }


        public void ChangePwd(SystemAdmin admin, string newPwd, string oldPwd)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                _sysadminDomainService.ChangePwd(admin, newPwd, oldPwd);
                unitOfWork.Commit();
                _adminLogService.Log("管理员修改个人密码", "管理员修改个人密码");
                scope.Complete();
            }
        }

        public void EditCurr(SystemAdmin admin)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                _sysadminDomainService.EditCurr(admin);
                unitOfWork.Commit();
                _adminCacheService.Remove(AdminCacheService.SysAdmin_Current_prefix + admin.SAName);
                _adminLogService.Log("管理员修改个人资料", "姓名:" + admin.SANickName + " || 性别:" + (admin.SASex ? "男" : "女") + " || email:" + admin.Email + " || 电话:" + admin.SAMobileNo);
                scope.Complete();
            }
        }


        public SystemAdmin GetByNameAndPassword(string SAName, string SAPwd)
        {
            string pwd = StrUtil.EncryptPassword(SAPwd, "MD5");
            SystemAdmin sysAdmin = _repository.GetByCondition(new DirectSpecification<SystemAdmin>(sa => sa.SAName == SAName && sa.SAPwd == pwd), true);
            return sysAdmin;
        }


        public void UpdateLogonInfo(SystemAdmin admin)
        {
            _sysadminDomainService.UpdateLogonInfo(admin);
            unitOfWork.Commit();
        }


        public IEnumerable<AdminModule> GetsysAdminModule(SystemAdmin admin)
        {
            return _sysadminDomainService.GetsysAdminModule(admin);
        }


        public bool CheckIsRepeatLogon(int said, string currIP)
        {
            return _sysadminDomainService.CheckIsRepeatLogon(said, currIP);
        }
    }
}
