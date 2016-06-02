using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace DDD.Utility
{
    public class OLEdbHelper
    {
        /// <summary>
        /// 获得连接对象
        /// </summary>
        /// <param name="ConnStr">System.Web.HttpContext.Current.Server.MapPath(ConnStr)</param>
        /// <returns></returns>
        public static OleDbConnection GetOleDbConnection(string ConnStr)
        {
            return new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + ConnStr);
        }

        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn,OleDbTransaction trans, string cmdText, OleDbParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Parameters.Clear();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            //cmd.CommandTimeout = 30;

            if (cmdParms != null)
            {
                foreach (OleDbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        public static DataSet ExecuteDataset(string ConnStr, string cmdText, params OleDbParameter[] cmdParms)
        {
            DataSet ds = new DataSet();
            OleDbCommand command = new OleDbCommand();
            using (OleDbConnection connection = GetOleDbConnection(ConnStr))
            {
                PrepareCommand(command, connection,null, cmdText, cmdParms);
                OleDbDataAdapter da = new OleDbDataAdapter(command);
                da.Fill(ds);
            }

            return ds;
        }

        public static DataRow ExecuteDataRow(string ConnStr, string cmdText, params OleDbParameter[] cmdParms)
        {
            DataSet ds = ExecuteDataset(ConnStr, cmdText, cmdParms);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0];
            return null;
        }

        /// <summary>
        /// 返回受影响的行数
        /// </summary>
        /// <param name="cmdText">a</param>
        /// <param name="commandParameters">传入的参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string ConnStr, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand command = new OleDbCommand();

            using (OleDbConnection connection = GetOleDbConnection(ConnStr))
            {
                PrepareCommand(command, connection,null, cmdText, cmdParms);
                return command.ExecuteNonQuery();
            }
        }

        public static int ExecuteNonQuery(OleDbTransaction trans, string cmdText, params OleDbParameter[] commandParameters)
        {
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 返回OleDataReader对象
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters">传入的参数</param>
        /// <returns></returns>
        public static OleDbDataReader ExecuteReader(string ConnStr, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand command = new OleDbCommand();
            OleDbConnection connection = GetOleDbConnection(ConnStr);
            try
            {
                PrepareCommand(command, connection,null, cmdText, cmdParms);
                OleDbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch
            {
                connection.Close();
                throw;
            }
        }

        /// <summary>
        /// 返回结果集中的第一行第一列，忽略其他行或列
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters">传入的参数</param>
        /// <returns></returns>
        public static object ExecuteScalar(string ConnStr, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection connection = GetOleDbConnection(ConnStr))
            {
                PrepareCommand(cmd, connection,null, cmdText, cmdParms);
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="recordCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="cmdText"></param>
        /// <param name="countText"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static DataSet ExecutePager(string ConnStr, ref int recordCount, int pageIndex, int pageSize, string cmdText, string countText, params OleDbParameter[] cmdParms)
        {
            if (recordCount < 0)
                recordCount = int.Parse(ExecuteScalar(ConnStr, countText, cmdParms).ToString());

            DataSet ds = new DataSet();

            OleDbCommand command = new OleDbCommand();
            using (OleDbConnection connection = GetOleDbConnection(ConnStr))
            {
                PrepareCommand(command, connection,null, cmdText, cmdParms);
                OleDbDataAdapter da = new OleDbDataAdapter(command);
                da.Fill(ds, (pageIndex - 1) * pageSize, pageSize, "result");
            }
            return ds;
        }

        public static string GetPageIDs(IDataReader dr, int myPageIndex, int myPageSize)
        {
            int myPageLowerBound = (myPageIndex - 1) * myPageSize;
            int myPageUpperBound = myPageLowerBound + myPageSize;
            int myIndex = 0;
            string result = string.Empty;
            while (dr.Read())
            {
                if (myIndex >= myPageLowerBound && myIndex < myPageUpperBound)
                {
                    result += dr.GetInt32(0) + ",";
                }
                myIndex++;
            }
            dr.Close();
            if (myIndex > 0)
                result = result.Substring(0, result.LastIndexOf(","));
            else result = "0";
            return result;
        }

    }
}
