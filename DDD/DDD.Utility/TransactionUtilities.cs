using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DDD.Utility
{
    public static class TransactionUtilities
    {
        public static TransactionScope CreateTransactionScopeWithNoLock()
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = IsolationLevel.ReadUncommitted;

            return new TransactionScope(TransactionScopeOption.Required, options);
        }

        /// <summary>
        /// 快照隔离事务
        /// 数据库必须配置： 属性 右键 设置  读提交快照打开以及 允许快照隔离
        /// 或
        /// ALTER DATABASE WQuan
///SET ALLOW_SNAPSHOT_ISOLATION ON 
///ALTER DATABASE WQuan
///SET READ_COMMITTED_SNAPSHOT ON
        /// 
        /// </summary>
        /// <returns></returns>
        public static TransactionScope CreateTransactionScopeWithSnapshot()
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = IsolationLevel.Snapshot;

            return new TransactionScope(TransactionScopeOption.Required, options);
        }

        public static TransactionScope CreateTransactionScopeWithNoLock(TransactionScopeOption option)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = IsolationLevel.ReadUncommitted;

            return new TransactionScope(option, options);
        }
    }

}
