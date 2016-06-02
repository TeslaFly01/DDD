using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;

namespace DDD.Domain.MainModule.Admin
{
    public class AdminActionDomainService : IAdminActionDomainService
    {
        private readonly IAdminActionRepository _repository;
        public AdminActionDomainService(IAdminActionRepository repository)
        {
            this._repository = repository;
        }

        public void Move(int aaid, bool Flag)
        {
            AdminAction oadminaction = _repository.GetByCondition(new DirectSpecification<AdminAction>(oaa => oaa.AAID == aaid));//旧数据
            if (oadminaction == null)
                throw new InvalidOperationException("未找到要排序的功能菜单");//无数据
            AdminAction adminaction = null;//根据条件查处的上/下条的数据
            if (Flag)
            {
                //找上一条(时间刚好比当前数据时间小的数据)
                var listadminaction = _repository.GetListByTopN(1, new DirectSpecification<AdminAction>(aat => aat.SortFlag < oadminaction.SortFlag && aat.AMID == oadminaction.AMID), x => x.SortFlag, true);
                if (listadminaction.Count() > 0) adminaction = listadminaction.FirstOrDefault();
            }
            else
            {
                //找下一条 (时间刚好比当前时间大的数据)
                var listadminaction = _repository.GetListByTopN(1, new DirectSpecification<AdminAction>(aat => aat.SortFlag > oadminaction.SortFlag && aat.AMID == oadminaction.AMID), x => x.SortFlag, false);
                if (listadminaction.Count() > 0) adminaction = listadminaction.FirstOrDefault();
            }
            string result = Flag == true ? "已经是第一条了" : "已经是最后一条了";
            if (adminaction == null)
                throw new InvalidOperationException(result);//未找到上/下的数据
            DateTime tsf = oadminaction.SortFlag;
            oadminaction.SortFlag = adminaction.SortFlag;
            adminaction.SortFlag = tsf;
            _repository.Move(oadminaction);
            _repository.Move(adminaction);
        }
    }
}
