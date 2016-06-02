using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDD.Utility
{
    public static class SqlUtil
    {
        public static string FormatSql(string sql)
        {
            if (!string.IsNullOrEmpty(sql))
                sql = sql.Replace("'", "''");
            return sql;
        }
    }
}
