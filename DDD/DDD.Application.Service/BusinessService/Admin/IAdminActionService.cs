using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD.Domain.Model.Entities.Admin;

namespace DDD.Application.Service.BusinessService.Admin
{
    public interface IAdminActionService : IBaseServices<AdminAction>
    {
        /// <summary>
        /// 上下排序
        /// </summary>
        /// <param name="aaid">action  id</param>
        /// <param name="Flag">排序 true表示上 false 表示下</param>
        void Move(int aaid, bool Flag);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">action ids</param>
        void DeleteList(string ids);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="aat">action</param>
        void AddAction(AdminAction aat);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="aat">action</param>
        void EditAction(AdminAction aat);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="aid">action id</param>
        void RemoveAction(int aid);
    }
}
