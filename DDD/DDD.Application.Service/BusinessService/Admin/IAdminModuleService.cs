using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD.Domain.Model.Entities.Admin;

namespace DDD.Application.Service.BusinessService.Admin
{
    public interface IAdminModuleService : IBaseServices<AdminModule>
    {
        /// <summary>
        /// 上下排序
        /// </summary>
        /// <param name="amid">功能id</param>
        /// <param name="Flag">排序字段 true表示上排 false表示下排</param>
        void Move(int amid, bool Flag);

        /// <summary>
        /// 批量启用禁用
        /// </summary>
        /// <param name="ids">要启用的ids</param>
        /// <param name="isEnable">启用、禁用</param>
        void Enable(string ids, bool isEnable);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">要删除的功能ids</param>
        void DeleteList(string ids);

        /// <summary>
        /// 树形控制
        /// </summary>
        /// <param name="checkids">选中的ids 不存在请传null</param>
        /// <returns></returns>
        string GetCheckStr(IEnumerable<AdminRole_Module> checkids);

        /// <summary>
        /// 根据pageurl读取功能对象
        /// </summary>
        /// <param name="PageUrl"></param>
        /// <returns></returns>
        AdminModule GetOneByPageUrl(string PageUrl);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="amid">功能id</param>
        void DeleteModule(int amid);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="am">功能</param>
        void AddModule(AdminModule am);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="am">功能</param>
        void EditModule(AdminModule am);
    }
}
