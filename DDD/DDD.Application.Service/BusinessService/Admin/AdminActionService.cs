using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.MainModule.Admin;
using DDD.Domain;
using DDD.Domain.Specification;
using System.Transactions;
using DDD.Application.Service;
using DDD.Application.Service.BusinessService.Admin;

namespace DDD.Application.Service.BusinessService.Admin
{
    public class AdminActionService : BaseServices<AdminAction>, IAdminActionService
    {
        private readonly IAdminActionDomainService _adminActionDomainService;
        private readonly IAdminLogService _adminLogService;
        public AdminActionService(IUnitOfWork unitOfWork, IAdminActionRepository repository, IAdminActionDomainService adminActionDomainService, IAdminLogService adminLogService)
            : base(unitOfWork, repository)
        {
            _adminActionDomainService = adminActionDomainService;
            _adminLogService = adminLogService;
        }

        public void Move(int aaid, bool Flag)
        {
            _adminActionDomainService.Move(aaid, Flag);
            base.unitOfWork.Commit();
        }

        public void DeleteList(string ids)
        {
            string newids = ids.Substring(0, ids.Length - 1);
            string[] adtionids = newids.Split('|');
            using (TransactionScope scope = new TransactionScope())
            {

                List<int> result = new List<string>(adtionids).ConvertAll(id => int.Parse(id));
                this.Remove(new DirectSpecification<AdminAction>(aa => result.Contains(aa.AAID)));

                _adminLogService.Log("批量删除action", "删除ID：" + ids);
                scope.Complete();
            }
        }


        public void AddAction(AdminAction aat)
        {
             using (TransactionScope scope = new TransactionScope())
             {
                 base.Add(aat);
                 _adminLogService.Log("新增action", "操作名称:" + aat.OptName + " || action名称:" + aat.ActionName + " || Controller名称:" + aat.ControllerName + " || 权值:" + aat.Weight);

                 scope.Complete();
             }
        }

        public void EditAction(AdminAction aat)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                base.Modify(aat);
                _adminLogService.Log("修改action", "操作名称:" + aat.OptName + " || action名称:" + aat.ActionName + " || Controller名称:" + aat.ControllerName + " || 权值:" + aat.Weight);
                scope.Complete();
            }
        }

        public void RemoveAction(int aid)
        {
            AdminAction adac = base.GetByCondition(new DirectSpecification<AdminAction>(aa => aa.AAID == aid));
            using (TransactionScope scope = new TransactionScope())
            {

                base.Remove(adac);
                _adminLogService.Log("删除action", "操作名称:" + adac.OptName + " || action名称:" + adac.ActionName + " || Controller名称:" + adac.ControllerName + " || 权值:" + adac.Weight);
                scope.Complete();
            }
        }
    }
}