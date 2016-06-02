using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;
using System.Transactions;

namespace DDD.Domain.MainModule.Admin
{
    public class AdminModuleDomainService : IAdminModuleDomainService
    {
        readonly IAdminModuleRepository _repository;
        readonly IAdminActionRepository _adminActionRepostory;
        public AdminModuleDomainService(IAdminModuleRepository repository, IAdminActionRepository adminActionRepostory)
        {
            this._repository = repository;
            this._adminActionRepostory = adminActionRepostory;
        }

        public void Move(int amid, bool Flag)
        {
            AdminModule oadminmodule = _repository.GetByCondition(new DirectSpecification<AdminModule>(oam => oam.AMID == amid));//旧数据
            if (oadminmodule == null)
                throw new InvalidOperationException("未找到要排序的功能菜单");//无数据
            AdminModule adminmodule = null;//根据条件查找上/下条的数据
            if (Flag)//true 上
            {
                //找上一条(时间刚好比当前数据时间小的数据)
                var adminModules = _repository.GetListByTopN(1, new DirectSpecification<AdminModule>(amd => amd.SortFlag < oadminmodule.SortFlag && amd.FID == oadminmodule.FID), x => x.SortFlag, true);
                if (adminModules.Count() > 0) adminmodule = adminModules.FirstOrDefault();
            }
            else//flase 下
            {
                //找下一条 (时间刚好比当前时间大的数据)
                var adminModules = _repository.GetListByTopN(1, new DirectSpecification<AdminModule>(amd => amd.SortFlag > oadminmodule.SortFlag && amd.FID == oadminmodule.FID), x => x.SortFlag, false);
                if (adminModules.Count() > 0) adminmodule = adminModules.FirstOrDefault();
            }
            string result = Flag == true ? "已经是第一条了" : "已经是最后一条了";
            if (adminmodule == null)
                throw new InvalidOperationException(result);//未找到上/下的数据
            DateTime tsf = oadminmodule.SortFlag;
            oadminmodule.SortFlag = adminmodule.SortFlag;
            adminmodule.SortFlag = tsf;
            _repository.Move(oadminmodule);
            _repository.Move(adminmodule);
        }


        public string GetCheckStr(IEnumerable<AdminRole_Module> checkids)
        {
            try
            {
                StringBuilder ModuleText = new StringBuilder();
                IEnumerable<AdminModule> AllModule = _repository.GetAll().OrderBy(c => c.SortFlag);
                List<AdminModule> listmodulefid0 = AllModule.Where(am => am.FID == 0).ToList();
                List<AdminModule> listmodule = null;
                ModuleText.Append("<ul class='checktree'>");
                foreach (var modulefid in listmodulefid0)
                {
                    //配置一级功能
                    ModuleText.Append("<li><input type='checkbox' name='checkmoduleitem' value='" + modulefid.AMID + "'><label style='cursor:default'>" + modulefid.ModuleName + "</label>");
                    listmodule = AllModule.Where(am => am.FID == modulefid.AMID).ToList();
                    if (listmodule.Count > 0)
                        ModuleText.Append("<ul>");
                    foreach (var module in listmodule)
                    {
                        //二级功能
                        if (IdSein(module.AMID, checkids))
                            ModuleText.Append("<li><input type='checkbox' checked='checked' name='checkmoduleitem' value='" + module.AMID + "'><span style='cursor:default'>" + module.ModuleName + "</span></li>");
                        else
                            ModuleText.Append("<li><input type='checkbox' name='checkmoduleitem' value='" + module.AMID + "'><span style='cursor:default'>" + module.ModuleName + "</span></li>");
                    }
                    if (listmodule.Count > 0)
                    {
                        ModuleText.Append("</ul>");
                        listmodule = null;
                    }
                    ModuleText.Append("</li>");
                }
                ModuleText.Append("</ul>");
                return ModuleText.ToString();
            }
            catch
            {
                throw new InvalidOperationException("获取树形时出错!");
            }
        }
        bool IdSein(int amid, IEnumerable<AdminRole_Module> valids)
        {
            bool resu = false;
            if (valids.Any())
            {
                foreach (var valid in valids)
                {
                    if (amid == valid.AMID)
                    {
                        resu = true;
                        break;
                    }
                }
            }
            return resu;
        }

        public void Delete(AdminModule am)
        {
            bool tadm = false;
            if (am.FID == 0)
                tadm = _repository.Exists(new DirectSpecification<AdminModule>(admm => admm.FID == am.AMID));
            if (tadm)
                throw new InvalidOperationException("该功能菜单包含子菜单内容,不能删除!");
            using (var scope = new TransactionScope())
            {
                //删从表
                _adminActionRepostory.Remove(new DirectSpecification<AdminAction>(aa => aa.AMID == am.AMID));
                //删主表
                _repository.Remove(am);
                scope.Complete();
            }
        }

        public void DeleteList(string[] ids)
        {
            //List<int> result = new List<string>(ids).ConvertAll(id => int.Parse(id));
            //_repository.Remove(new DirectSpecification<AdminModule>(am => result.Contains(am.AMID)));
            using (TransactionScope scope = new TransactionScope())
            {
                int amid = 0;
                foreach (var item in ids)
                {
                    amid = int.Parse(item);
                    AdminModule adminM = _repository.GetByCondition(new DirectSpecification<AdminModule>(adm => adm.AMID == amid));
                    Delete(adminM);
                    amid = 0;
                }
                scope.Complete();
            }
        }
    }
}
