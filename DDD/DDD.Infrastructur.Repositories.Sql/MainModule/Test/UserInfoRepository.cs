using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DDD.Domain.MainModule.Test;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;

namespace DDD.Infrastructur.Repositories.Sql.MainModule.Test
{
    public class UserInfoRepository : RepositoryBase<UserInfo>, IUserInfoRepository
    {
        public UserInfoRepository(IDatabaseFactory DatabaseFactory)
            : base(DatabaseFactory)
        {

        }

        public List<UserInfo> GetList(ISpecification<UserInfo> specification)
        {
            var listAgent = base.GetMany(specification).ToList();
            return listAgent;
        }

        /// <summary>
        /// 重置未提醒扫描标记：正在扫描=>等待扫描
        /// </summary>
        public void ChangeScanningToWait()
        {
            base.DataContext.Database.ExecuteSqlCommand("dbo.[NotNoticeScanningToWait]");
        }

        /// <summary>
        /// 获取为提醒列表
        /// </summary>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public List<string> GetNotNoticeList(int topNum)
        {
            var list = new List<string>();
            var myQuery = (from x in DataContext.UserInfo
                           where x.ScanFlag==1
                           select x).Take<UserInfo>(topNum);
            list = myQuery.Select(x => x.ID.ToString()).ToList();
            return list;
        }

        /// <summary>
        /// 批量更新未提醒列表
        /// </summary>
        /// <param name="idList"></param>
        public void BatchNotNoticeScaning(List<string> idList)
        {
            string list = string.Format("{0}", string.Join(",", idList.ToArray()));
            var para = new SqlParameter[]
                {
                    new SqlParameter
                    {
                        ParameterName    = "@ID",
                        SqlDbType = SqlDbType.VarChar,
                        Value = list.ToString()
                    }
                };
            base.DataContext.Database.ExecuteSqlCommand("dbo.[NotNoticeBatchUpdateScanning] @ID", para);
        }
    }
}
