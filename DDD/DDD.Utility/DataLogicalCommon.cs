using System;
using System.Data;
using System.Data.SqlClient;

namespace DDD.Utility
{
    public class DataLogicalCommon
    {
        public DataLogicalCommon()
        { 
        
        }
        public static int getRecordCount(string ConnectionString, string TableName)
        {
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@table", SqlDbType.VarChar, 50) };
            parameters[0].Value = TableName;
            SqlDataReader dr = DataAccessHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "sp_getRecordCount", parameters);

            try
            {
                if (dr.Read())
                {
                    int RecordCount = dr.GetInt32(0);
                    return RecordCount;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                dr.Close();
                throw ex;
            }
        }

        public static int getPageCount(IPage itfc, int PageSize)
        {
            long TotalRecordCount = itfc.getRecordCount();
            int TotalPage = 0;
            if (TotalRecordCount % PageSize == 0)
                TotalPage = Int32.Parse(Convert.ToString(TotalRecordCount / PageSize));
            else
                TotalPage = Int32.Parse(Convert.ToString(TotalRecordCount / PageSize)) + 1;
            return TotalPage;
        }

        public static int getPageCount(int RecordCount, int PageSize)
        {
            int TotalPage = 0;
            if (RecordCount % PageSize == 0)
                TotalPage = Int32.Parse(Convert.ToString(RecordCount / PageSize));
            else
                TotalPage = Int32.Parse(Convert.ToString(RecordCount / PageSize)) + 1;
            return TotalPage;
        }

        /// <summary>
        /// 取得页索引在分页显示中的第几页
        /// </summary>
        /// <param name="Pageindex">当页索引</param>
        /// <param name="PagesSize">每页显示多少页索引</param>
        /// <returns></returns>
        public static int getPagesIndex(int Pageindex, int PagesSize)
        {
            int CurPage = 0;
            if (Pageindex % PagesSize == 0)
                CurPage = Int32.Parse(Convert.ToString(Pageindex / PagesSize));
            else
                CurPage = Int32.Parse(Convert.ToString(Pageindex / PagesSize)) + 1;
            return CurPage;
        }
    }


}
