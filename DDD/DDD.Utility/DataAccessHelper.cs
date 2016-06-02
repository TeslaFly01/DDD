using System;
using System.Data;
using System.Data.SqlClient;

namespace DDD.Utility
{
    /// <summary>
    /// DataAccess 的摘要说明。
    /// </summary>
    public class DataAccessHelper
    {
        #region CommonMethodDataAccess
        //public static  string ConStr=ConfigurationSettings.AppSettings["ConnectionString"];
        public static int ExecutNonQuery(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection con = new SqlConnection(connString);
            PrepareCmd(cmd, con, cmdType, cmdText, cmdParms);
            try
            {
                int result = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public static int RExecutNonQuery(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection con = new SqlConnection(connString);
            PrepareCmd(cmd, con, cmdType, cmdText, cmdParms);
            try
            {
                int result = -1;
                cmd.ExecuteNonQuery();
                if (cmd.Parameters[0].Value is DBNull)
                {
                    return result;
                }
                result = Convert.ToInt32(cmd.Parameters[0].Value);
                cmd.Parameters.Clear();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public static object ExecuteScalar(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection con = new SqlConnection(connString);
            PrepareCmd(cmd, con, cmdType, cmdText, cmdParms);
            try
            {
                return cmd.ExecuteScalar();
            }
            catch (SqlException ex1)
            {
                throw ex1;
            }
            finally
            {
                con.Close();
            }
        }
        
        public static SqlDataReader ExecuteReader(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection con = new SqlConnection(connString);
            PrepareCmd(cmd, con, cmdType, cmdText, cmdParms);
            try
            {
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException ex1)
            {
                throw ex1;
            }
        }


        public static DataSet EexcuteDataSet(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection con = new SqlConnection(connString);
            PrepareCmd(cmd, con, cmdType, cmdText, cmdParms);
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            try
            {
                DataSet myDS = new DataSet();
                dap.Fill(myDS);
                return myDS;
            }
            catch (SqlException ex2)
            {
                throw ex2;
            }
            finally
            {
                con.Close();
            }
        }

        public static DataSet EexcuteDataSetName(string connString, CommandType cmdType, string cmdText, string TName, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection con = new SqlConnection(connString);
            PrepareCmd(cmd, con, cmdType, cmdText, cmdParms);
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            try
            {
                DataSet myDS = new DataSet();
                dap.Fill(myDS, TName);
                return myDS;
            }
            catch (SqlException ex3)
            {
                throw ex3;
            }
            finally
            {
                con.Close();
            }
        }

        public static DataView EexcuteDataView(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            return EexcuteDataSet(connString, cmdType, cmdText, cmdParms).Tables[0].DefaultView;
        }


        private static void PrepareCmd(SqlCommand cmd, SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter SqlPar in cmdParms)
                {
                    cmd.Parameters.Add(SqlPar);
                }
            }
        }

        #endregion
    }
}
