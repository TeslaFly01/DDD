using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DDD.Utility
{
    public class ExcelHelper
    {
        /// <summary>
        /// 
        /// </summary>
        private ExcelHelper()
        {
        }

        #region Const Variables

        /// <summary>
        /// Excel 版本号
        /// </summary>
        private const string ExcelDefaultVersion = "8.0";

        /// <summary>
        /// 连接字符串模板
        /// </summary>
        private const string ConnectionStringTemplate = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source ={0};Extended Properties=Excel {1}";
        #endregion


        #region Public Methods
        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="excelPath">Excel文件绝对路径。</param>
        /// <returns></returns>
        internal static OleDbConnection CreateConnection(string excelPath)
        {
            return CreateConnection(excelPath, ExcelDefaultVersion);
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="excelPath">Excel文件绝对路径。</param>
        /// <param name="excelVersion">Excel版本号。默认为 8.0</param>
        /// <returns></returns>
        internal static OleDbConnection CreateConnection(string excelPath, string excelVersion)
        {
            return new OleDbConnection(GetConnectionString(excelPath, excelVersion));
        }

        /// <summary>
        /// 获取Excel的第一个Sheet的数据。注意，这里的第一个是按Sheet名排列后的第一个Sheet。
        /// <example>
        /// DataTable dt = Query(@"C:\My Documents\1.xls");
        /// </example>
        /// </summary>
        /// <param name="excelPath">Excel文件绝对路径。</param>
        /// <returns></returns>
        public static DataTable Query(string excelPath)
        {
            return Query(excelPath, 0);
        }

        /// <summary>
        /// 获取Excel指定Sheet名称的数据。
        /// <example>
        /// DataTable dt = Query(@"C:\My Documents\1.xls", "sheet1");
        /// </example>
        /// </summary>
        /// <param name="excelPath">Excel文件绝对路径。</param>
        /// <param name="sheetName">Sheet名，允许空格存在。如：sheet1</param>
        /// <returns></returns>
        public static DataTable Query(string excelPath, string sheetName)
        {
            OleDbConnection conn = CreateConnection(excelPath);
            conn.Open();

            DataTable dt = new DataTable();
            dt = QueryBySheetName(conn, sheetName + "$");

            conn.Close();
            return dt;
        }

        /// <summary>
        /// 获取Excel指定Sheet名称的数据。
        /// <example>
        /// DataTable dt = Query(@"C:\My Documents\1.xls", "sheet1$");
        /// DataTable dt = Query(@"C:\My Documents\1.xls", "'My Sheet'$");
        /// </example>
        /// </summary>
        /// <param name="excelPath">Excel文件绝对路径。</param>
        /// <param name="rawSheetName">Sheet名，允许空格存在。如：sheet1$, 'My Sheet'$</param>
        /// <returns></returns>
        public static DataTable QueryEx(string excelPath, string rawSheetName)
        {
            OleDbConnection conn = CreateConnection(excelPath);
            conn.Open();

            DataTable dt = new DataTable();
            dt = QueryBySheetName(conn, rawSheetName);

            conn.Close();
            return dt;
        }

        /// <summary>
        /// 获取指定序号的Sheet的数据。序号从0开始。注意，是按Sheet名排列后的第Index个Sheet。
        /// <example>
        /// DataTable dt = Query(@"C:\My Documents\1.xls", 0);
        /// </example>
        /// </summary>
        /// <param name="excelPath">Excel文件绝对路径。</param>
        /// <param name="sheetIndex">Sheet的序号，从0开始。</param>
        /// <returns></returns>
        public static DataTable Query(string excelPath, int sheetIndex)
        {
            OleDbConnection conn = CreateConnection(excelPath);
            conn.Open();

            ArrayList arrSheets = GetSheetNames(conn);
            if (arrSheets.Count <= sheetIndex)
                throw new ArgumentOutOfRangeException();

            string sheetName = arrSheets[sheetIndex].ToString();
            DataTable dt = QueryBySheetName(conn, sheetName);
            conn.Close();
            return dt;
        }

        /// <summary>
        /// 获取Excel的所有的Sheet的名称。
        /// </summary>
        /// <param name="excelPath">Excel文件绝对路径。</param>
        /// <returns></returns>
        public static ArrayList GetSheetNames(string excelPath)
        {
            OleDbConnection conn = CreateConnection(excelPath);
            ArrayList arrSheets = GetSheetNames(conn);
            conn.Close();
            return arrSheets;
        }

        /// <summary>
        /// 将DataTable的内容保存到Excel的一个指定模板的Sheet中。
        /// 指定模板是指指定了的列头。
        /// </summary>
        /// <param name="dt">要保存的数据</param>
        /// <param name="excelPath">Excel文件绝对路径。</param>
        /// <param name="sheetName">Sheet名，允许空格存在。如：sheet1</param>
        public static void DataTableToExcel(DataTable dt, string excelPath, string sheetName)
        {
            OleDbConnection conn = CreateConnection(excelPath);
            conn.Open();
            DataTableToExcel(conn, dt, sheetName + "$");
            conn.Close();
        }

        /// <summary>
        /// 将DataTable的内容保存到Excel的一个指定模板的Sheet中。
        /// 指定模板是指指定了的列头。
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelPath">Excel文件绝对路径。</param>
        /// <param name="sheetIndex">Sheet的序号，从0开始。</param>
        public static void DataTableToExcel(DataTable dt, string excelPath, int sheetIndex)
        {
            OleDbConnection conn = CreateConnection(excelPath);
            conn.Open();

            ArrayList arrSheets = GetSheetNames(conn);
            if (arrSheets.Count <= sheetIndex)
                throw new ArgumentOutOfRangeException();

            string sheetName = arrSheets[sheetIndex].ToString();
            DataTableToExcel(conn, dt, sheetName);
            conn.Close();
        }


        /// <summary>
        /// Allowed:
        ///     12121.32432434e+10
        ///     -1.32432434E-20
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string ToNumberString(string s)
        {
            // 先检查是不是合法的数字字符串
            if (!Regex.IsMatch(s, @"^[\-\+]?[0-9]+(\.[0-9]+)?[eE]+[\+\-][0-9]+$"))
                return s;
            decimal value = decimal.Parse(s,
                System.Globalization.NumberStyles.AllowExponent | System.Globalization.NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture);
            return value.ToString();
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="excelVersion"></param>
        /// <returns></returns>
        private static string GetConnectionString(string excelPath, string excelVersion)
        {
            return string.Format(CultureInfo.InvariantCulture, ConnectionStringTemplate, excelPath, excelVersion);
        }

        /// <summary>
        /// 根据Sheet的名获取数据。
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private static DataTable QueryBySheetName(OleDbConnection conn, string sheetName)
        {
            string cmd = "select * from [" + sheetName + "]";
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;

        }

        /// <summary>
        /// 获取所有的Sheet名
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        private static ArrayList GetSheetNames(OleDbConnection conn)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            ArrayList arrSheets = new ArrayList();
            foreach (DataRow row in dt.Rows)
            {
                arrSheets.Add(row[2]);
            }
            return arrSheets;
        }

        /// <summary>
        /// 两个DataTable的数据对拷
        /// </summary>
        /// <param name="srcTable"></param>
        /// <param name="destTable"></param>
        private static void CopyDataTable(DataTable srcTable, DataTable destTable)
        {
            foreach (DataRow row in srcTable.Rows)
            {
                DataRow newRow = destTable.NewRow();
                for (int i = 0; i < destTable.Columns.Count; i++)
                {
                    newRow[i] = row[i];
                }
                destTable.Rows.Add(newRow);
            }
        }

        /// <summary>
        /// 将DataTable的内容保存到Excel中。
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="dt"></param>
        /// <param name="sheetName"></param>
        private static void DataTableToExcel(OleDbConnection conn, DataTable dt, string sheetName)
        {
            string cmd = "select * from [" + sheetName + "$]";
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd, conn);
            OleDbCommandBuilder cmdBuilder = new OleDbCommandBuilder(adapter);
            cmdBuilder.QuotePrefix = "[";
            cmdBuilder.QuoteSuffix = "]";
            DataSet ds = new DataSet();
            adapter.Fill(ds, "Table1");

            CopyDataTable(dt, ds.Tables[0]);

            adapter.Update(ds, "Table1");
        }

        #endregion
    }
}
