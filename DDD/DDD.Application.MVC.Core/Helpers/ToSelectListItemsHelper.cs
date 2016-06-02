using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DDD.Domain.Model.Entities.Admin;
using DDD.Infrastructure.CrossCutting.Common;

namespace DDD.Application.MVC.Core.Helpers
{
    public static class ToSelectListItemsHelper
    {
        
        #region 管理员功能菜单

        public static IEnumerable<AdminModule> ToSelectListItemsForSysModules(this IEnumerable<AdminModule> Modules)
        {
            return GetAdminModuleTreeList(Modules.ToList(), 0);
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(
              this IEnumerable<AdminModule> Modules, int rootID, int selectedId)
        {
            return
               GetAdminModuleTreeList(Modules.ToList(), rootID)
                      .Select(m =>
                          new SelectListItem
                          {
                              Selected = (m.AMID == selectedId),
                              Text = m.ModuleName,
                              Value = m.AMID.ToString()
                          });
        }

        /// <summary>
        /// 传入一个树的根节点 得到整个树   (只查一次数据库)
        /// </summary>
        /// <param name="_Root">根节点</param>
        /// <returns></returns>
        public static List<AdminModule> GetAdminModuleTreeList(List<AdminModule> dtos, int _RootID)
        {

            List<AdminModule> _dtos = new List<AdminModule>();

            BuildTree(dtos, _dtos, _RootID, 0);

            //_dtos.Insert(0, new BlogCategory { CateID = 0, CateName = "顶级分类", State = 1, ParentID = 0 });
            return _dtos;

        }

        /// <summary>
        /// 得到一个xx树的递归方法
        /// </summary>
        /// <param name="dtos">源数据</param>
        /// <param name="_dtos">复制的数据</param>
        /// <param name="_Parent">父节点</param>
        /// <param name="_Deep">当前深度</param>
        /// <returns></returns>
        public static List<AdminModule> BuildTree(List<AdminModule> dtos, List<AdminModule> _dtos, int _ParentID, int _Deep)
        {

            string DeepStr = TreeHelper.GetDeepStr(_Deep);

            for (int i = 0; i < dtos.Count; i++)
            {
                if (dtos[i].FID == _ParentID)
                {

                    //加上树的符号
                    dtos[i].ModuleName = DeepStr + dtos[i].ModuleName;
                    //再插入
                    _dtos.Add(dtos[i]);

                    BuildTree(dtos, _dtos, dtos[i].AMID, (_Deep + 1));
                }
            }

            return _dtos;

        }
        #endregion
    }
}
