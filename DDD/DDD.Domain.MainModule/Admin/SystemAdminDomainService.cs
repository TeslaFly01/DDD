using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;
using DDD.Utility;

namespace DDD.Domain.MainModule.Admin
{
    public class SystemAdminDomainService : ISystemAdminDomainService
    {
        readonly ISystemAdminRepository _repository;
        private readonly IAdminRoleRepository _adminRoleRepository;
        private readonly IAdminModuleRepository _adminModuleRepository;
        public SystemAdminDomainService(ISystemAdminRepository repository, IAdminRoleRepository adminRoleRepository, IAdminModuleRepository adminModuleRepository)
        {
            this._repository = repository;
            this._adminRoleRepository = adminRoleRepository;
            this._adminModuleRepository = adminModuleRepository;
        }

        public void AddSysAdmin(SystemAdmin admin, string[] roleids)
        {
            //判断账户唯一性
            bool isexit = _repository.Exists(new DirectSpecification<SystemAdmin>(sa => sa.SAName == admin.SAName));
            if (isexit) throw new InvalidOperationException("[" + admin.SAName + "]已经被使用!");
            List<int> getids = new List<string>(roleids).ConvertAll(arid => int.Parse(arid));
            List<AdminRole> listrole = _adminRoleRepository.GetMany(new DirectSpecification<AdminRole>(ar => getids.Contains(ar.ARID)), false).ToList();
            admin.AdminRoles = listrole;
            _repository.Add(admin);
        }

        public void UpdateSysAdmin(SystemAdmin admin, string[] roleids)
        {
            List<int> result = new List<string>(roleids).ConvertAll(id => int.Parse(id));
            admin.AdminRoles = _adminRoleRepository.GetMany(new DirectSpecification<AdminRole>(ar => result.Contains(ar.ARID)), false).ToList();
            _repository.UpdateSysAdmin(admin);
            //_repository.UpdateSysAdmin(admin,roleids);
        }


        public void ChangePwd(SystemAdmin admin, string newPwd, string oldPwd)
        {
            string newPassWord = StrUtil.EncryptPassword(newPwd, "MD5");
            string oldPassWord = StrUtil.EncryptPassword(oldPwd, "MD5");
            if (!admin.SAPwd.Equals(oldPassWord)) throw new Exception("旧密码错误!");
            _repository.ChangePwd(admin, newPassWord);
        }


        public void EditCurr(SystemAdmin admin)
        {
            _repository.EditCurr(admin);
        }

        public void UpdateLogonInfo(SystemAdmin admin)
        {
            _repository.UpdateLogonInfo(admin);
        }


        public IEnumerable<AdminModule> GetsysAdminModule(SystemAdmin admin)
        {
            var AMIDs = new List<int>();
            foreach (var role in admin.AdminRoles) //导航属性效率都比较低下，以后数据量大后修改为手写的Linq或sql语句，商家后台的一样 2012-7-20
            {
                foreach (var rm in role.AdminRole_Modules)
                {
                    AMIDs.Add(rm.AMID);
                }
            }

            var listAdm = _adminModuleRepository.GetMany(new DirectSpecification<AdminModule>(am => AMIDs.Contains(am.AMID) && am.IsEnable));

            var listResult = new List<AdminModule>();
            //排除多余数据           
            foreach (var adm in listAdm)
            {
                if (!listResult.Contains(adm))
                    listResult.Add(adm);
            }
            listResult = listResult.OrderBy(x => x.SortFlag).ToList();

            return listResult;
        }

        public bool CheckIsRepeatLogon(int said, string currIP)
        {
            bool IsConsistent = false;
            //读取Sysadmin对象
            string currip = _repository.GetIPBysysAdmin(said);
            if (currip.Equals(currIP))
                IsConsistent = true;
            return IsConsistent;
        }
    }
}
