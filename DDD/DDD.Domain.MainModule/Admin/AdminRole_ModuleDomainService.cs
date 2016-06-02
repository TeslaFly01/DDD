using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;

namespace DDD.Domain.MainModule.Admin
{
    public class AdminRole_ModuleDomainService : IAdminRole_ModuleDomainService
    {
        private readonly IAdminRole_ModuleRepository _repository;
        private readonly IAdminActionRepository _adminActionRepository;
        private readonly IAdminModuleRepository _adminModuleRepository;
        public AdminRole_ModuleDomainService(IAdminRole_ModuleRepository repository, IAdminActionRepository adminActionRepository, IAdminModuleRepository adminModuleRepository)
        {
            this._repository = repository;
            this._adminModuleRepository = adminModuleRepository;
            this._adminActionRepository = adminActionRepository;
        }

        //实现添加角色关联
        public void AddList(string[] moduleids, int arid)
        {
            AdminRole_Module arm = null;
            int wgts = 0;//权值逻辑与
            foreach (var mid in moduleids)
            {
                int mmid = int.Parse(mid);
                AdminModule admd = _adminModuleRepository.GetByCondition(new DirectSpecification<AdminModule>(a => a.AMID == mmid));
                if (admd.FID == 0)//如果是顶级菜单
                {
                    IEnumerable<AdminModule> listf = _adminModuleRepository.GetMany(new DirectSpecification<AdminModule>(m => m.FID == admd.AMID));
                    if (listf.Count() == 0)//如果不存在子级菜单 抛异常
                        throw new InvalidOperationException("您选择的顶级功能菜单有不包含子级的数据,请取消那些项的选择!");
                }
                //读取当前功能id下的action 计算权值和
                List<AdminAction> listaction = _adminActionRepository.GetMany(new DirectSpecification<AdminAction>(aa => aa.AMID == mmid)).ToList();
                foreach (var mat in listaction)
                {
                    wgts |= (int)mat.Weight;
                }
                //添加一条关联数据
                try
                {
                    arm = new AdminRole_Module() { ARID = arid, AMID = int.Parse(mid), Weights = wgts };
                    _repository.Add(arm);
                    wgts = 0;
                    arm = null;
                }
                catch
                {
                    throw new InvalidOperationException("添加角色关联时出错!");
                }
            }
        }


        public void UpdateList(string[] moduleids, int arid)
        {

            //对比老数据 无改动的数据 权值不变 改动的才先删后增加
            //使用角色id找到功能ids
            List<AdminRole_Module> listarm = _repository.GetMany(new DirectSpecification<AdminRole_Module>(arm => arm.ARID == arid)).ToList();
            string addids = string.Empty;
            List<AdminRole_Module> tarm = null;
            foreach (var checkid in moduleids)
            {
                int mmid = int.Parse(checkid);
                AdminModule admd = _adminModuleRepository.GetByCondition(new DirectSpecification<AdminModule>(a => a.AMID == mmid));
                if (admd.FID == 0)//if顶级菜单
                {
                    IEnumerable<AdminModule> listf = _adminModuleRepository.GetMany(new DirectSpecification<AdminModule>(m => m.FID == admd.AMID));
                    if (listf.Count() == 0)//if不存在子级菜单
                        throw new InvalidOperationException("您选择的顶级功能菜单有不包含子级的数据,请取消那些项的选择!");
                }
                tarm = listarm.Where(ami => ami.AMID == mmid && ami.ARID == arid).ToList();
                if (tarm.Count() == 0)//if 不存在功能关联数据
                    addids += checkid + "|";
                else //存在关联数据 移除该项
                    listarm.Remove(tarm[0]);
            }
            try
            {
                //对比功能ids的差异
                if (listarm.Count > 0)//修改后不包含的以前数据删除掉
                {
                    foreach (var rarm in listarm)
                    {
                        _repository.Remove(rarm);
                    }
                }
                if (addids.Length > 0)//添加修改后多出的数据
                {
                    string newids = addids.Substring(0, addids.Length - 1);
                    string[] add = newids.Split('|');
                    AddList(add, arid);
                }
            }
            catch
            {
                throw new InvalidOperationException("修改角色时出错!");
            }
        }
    }
}
