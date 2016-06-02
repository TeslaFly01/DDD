using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;

namespace DDD.Domain.MainModule.Admin
{
    public class AdminRoleDomainService : IAdminRoleDomainService
    {
        private readonly IAdminRoleRepository _repository;
        private readonly IAdminRole_ModuleRepository _adminR_MRepository;
        private readonly IAdminModuleRepository _adminModuleRepository;
        private readonly ISystemAdminRepository _sysAdminRepository;
        public AdminRoleDomainService(IAdminRoleRepository repository, IAdminRole_ModuleRepository adminR_MRepository, IAdminModuleRepository adminModuleRepository, ISystemAdminRepository sysAdminRepository)
        {
            this._repository = repository;
            this._adminR_MRepository = adminR_MRepository;
            this._adminModuleRepository = adminModuleRepository;
            this._sysAdminRepository = sysAdminRepository;
        }

        public List<AdminModule> GetRoleModules(int arid)
        {
            try
            {
                //读取角色表中的功能表
                IEnumerable<AdminRole_Module> listarm = _adminR_MRepository.GetMany(new DirectSpecification<AdminRole_Module>(arm => arm.ARID == arid), false);
                //封装amid到list
                // var listint = listarm.Select(item => item.AMID).ToList();
                //读取FID=0的功能表数据(一级菜单)
                //IEnumerable<AdminModule> listam = _adminModuleRepository.GetMany(new DirectSpecification<AdminModule>(am => listint.Contains(am.AMID) && am.IsEnable));
                return listarm.Select(x => x.adminModule).ToList();
            }
            catch
            {
                throw new InvalidOperationException("获取一级菜单时出错!");
            }
        }

        public string GetCurrRoleSecondModule(int arid, int firstid)
        {
            try
            {
                StringBuilder resulestr = new StringBuilder();
                //读取角色表中的功能表
                IEnumerable<AdminRole_Module> listarm = _adminR_MRepository.GetMany(new DirectSpecification<AdminRole_Module>(arm => arm.ARID == arid));
                //封装amid到list
                List<int> listint = new List<int>();
                foreach (var item in listarm)
                {
                    listint.Add(item.AMID);
                }
                //读取FID=firstid的功能表数据(二级级菜单)
                IEnumerable<AdminModule> listam = _adminModuleRepository.GetMany(new DirectSpecification<AdminModule>(am => listint.Contains(am.AMID))).Where(am => am.FID == firstid);
                int cut = 0;
                //生成input type=radio 的标签
                foreach (var item in listam)
                {
                    cut++;
                    //得到当前ARID firstid 的权值
                    resulestr.Append("<input type='radio' id='secondmd" + cut + "' name='secondModule' value='" + item.AMID + "' onclick='actionGetC(" + item.AMID + "," + arid + ")' /> <label for='secondmd" + cut + "'>" + item.ModuleName + "</label>");
                    if (cut % 4 == 0)
                        resulestr.Append("<br />");
                    else
                        resulestr.Append(" ");
                }
                return resulestr.ToString();
            }
            catch
            {
                throw new InvalidOperationException("获取二级菜单时出错!");
            }
        }

        public void UpdateWeight(int arid, string CMIDWeight)
        {
            try
            {
                var cws = new List<AMIDWeightDTO>();
                //amid|weight
                if (CMIDWeight.Length > 0)
                {
                    CMIDWeight = CMIDWeight.Substring(0, CMIDWeight.Length - 1);
                    //amid|weight数据
                    var cmwg = CMIDWeight.Split(',');
                    foreach (var s in cmwg)
                    {
                        //s amid|weight 拆分成数组
                        var cw = s.Split('|');
                        var tcmid = int.Parse(cw[0]);
                        var tweght = int.Parse(cw[1]);
                        if (cws.Any(x => x.AMID == tcmid))
                        {
                            var tcw = cws.First(x => x.AMID == tcmid);
                            var ncw = new AMIDWeightDTO { AMID = tcmid, Weight = tcw.Weight |= tweght };
                            cws.Remove(tcw);
                            cws.Add(ncw);
                        }
                        else
                        {
                            cws.Add(new AMIDWeightDTO() { AMID = tcmid, Weight = tweght });
                        }
                    }
                }

                var ocws =
                    _adminR_MRepository.GetMany(
                        new DirectSpecification<AdminRole_Module>(x => x.ARID == arid), false);
                foreach (var corpUserRoleModule in ocws)
                {
                    if (cws.Any(x => x.AMID == corpUserRoleModule.AMID))
                    {
                        corpUserRoleModule.Weights = cws.FirstOrDefault(x => x.AMID == corpUserRoleModule.AMID).Weight;
                        _adminR_MRepository.Update(corpUserRoleModule, x => x.Weights);
                    }
                }
            }
            catch
            {
                throw new InvalidOperationException("修改权值时出错!");
            }
        }

        public string GetAllRoletoCheck(IEnumerable<AdminRole> checkids, bool disabled)
        {
            StringBuilder RoleCheckString = new StringBuilder();
            IEnumerable<AdminRole> listrole = _repository.GetAll();
            int cut = 0;
            foreach (var item in listrole)
            {
                cut++;
                RoleCheckString.Append("<input type='checkbox'");
                if (IsExistence(item.ARID, checkids))//存在
                    RoleCheckString.Append(" checked='checked'");
                if (disabled)
                    RoleCheckString.Append(" disabled='disabled'");
                RoleCheckString.Append(" id='checkrole" + cut + "' name='checkroleitem' value='" + item.ARID + "' /> <label for='checkrole" + cut + "'>" + item.ARName + "</label>");
                if (cut % 4 == 0)
                    RoleCheckString.Append("<br />");
                else
                    RoleCheckString.Append(" ");
            }
            return RoleCheckString.ToString();
        }

        bool IsExistence(int arid, IEnumerable<AdminRole> checkid)
        {
            bool result = false;
            foreach (var item in checkid)
            {
                if (arid == item.ARID)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public string GetActionCheck(IEnumerable<AdminAction> checkids, int arid)
        {
            StringBuilder ActionCheckStr = new StringBuilder();
            int cut = 0;
            IEnumerable<AdminRole_Module> listarm = _adminR_MRepository.GetMany(new DirectSpecification<AdminRole_Module>(arm => arm.ARID == arid));
            int wgt = 0;
            foreach (var item in checkids)
            {
                wgt = listarm.Where(cam => cam.AMID == item.AMID).FirstOrDefault().Weights;
                cut++;
                if ((wgt & item.Weight) == item.Weight)
                    ActionCheckStr.Append("<input type='checkbox' id='setWeight" + cut + "' checked='checked' name='setWeights' value='" + item.Weight + "' /> <label for='setWeight" + cut + "'>" + item.OptName + "</label>");
                else
                    ActionCheckStr.Append("<input type='checkbox' id='setWeight" + cut + "' name='setWeights' value='" + item.Weight + "' /> <label for='setWeight" + cut + "'>" + item.OptName + "</label>");
                if (cut % 6 == 0)
                    ActionCheckStr.Append("<br />");
                else
                    ActionCheckStr.Append(" ");
                wgt = 0;
            }
            return ActionCheckStr.ToString();
        }


        public void DeleteRole(AdminRole ar)
        {
            //判断角色是否正在使用(管理员)
            //SystemAdmin admin= _sysAdminRepository.GetByCondition(new DirectSpecification<SystemAdmin>(x => x.AdminRoles.Any(y => y.ARID == arid)));
            if (CheckAdmin(ar.ARID)) throw new InvalidOperationException("删除失败：当前角色下存在管理员！");
            using (TransactionScope scope = new TransactionScope())
            {
                //删从表
                _adminR_MRepository.Remove(new DirectSpecification<AdminRole_Module>(am => am.ARID == ar.ARID));
                //删主表
                _repository.Remove(ar);
                scope.Complete();
            }
        }

        //验证是否存在管理员使用该角色
        public bool CheckAdmin(int arid)
        {
            bool ChkAdmin = _sysAdminRepository.Exists(new DirectSpecification<SystemAdmin>(x => x.AdminRoles.Any(y => y.ARID == arid)));
            return ChkAdmin;
        }
    }

    public class AMIDWeightDTO
    {
        public int AMID { get; set; }
        public int Weight { get; set; }
    }
}
