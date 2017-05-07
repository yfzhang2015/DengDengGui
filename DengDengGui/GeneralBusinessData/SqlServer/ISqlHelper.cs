using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;


namespace GeneralBusinessData.SqlServer
{
    /// <summary>
    /// MSSqlServer语句执行类
    /// </summary>
    public class SqlServerHelper : ISqlHelper
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="parmeters">参数列表</param>
        /// <returns></returns>
        public List<Dictionary<string, dynamic>> QueryList(string sql, params DbParameter[] parmeters)
        {
            return null;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="sqlOperation">SQL操作对象</param>
        /// <returns></returns>
        public List<Dictionary<string, dynamic>> QueryList(SqlOperation sqlOperation)
        {
            return null;
        }
        /// <summary>
        /// 查询单值
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="parmeters">参数列表</param>
        /// <returns></returns>
        public object QueryValue(string sql, params DbParameter[] parmeters)
        {
            return null;
        }
        /// <summary>
        /// 查询单值
        /// </summary>
        /// <param name="sqlOperation">SQL操作对象</param>
        /// <returns></returns>
        public object QueryValue(SqlOperation sqlOperation)
        {
            return null;
        }

        /// <summary>
        /// 增删改数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parmeters">参数列表</param>
        /// <returns></returns>
        public int ChangeData(string sql, params DbParameter[] parmeters)
        {
            return 0;
        }

        /// <summary>
        /// 增删改数据
        /// </summary>
        /// <param name="sqlOperation">SQL操作对象</param>
        /// <returns></returns>
        public int ChangeData(SqlOperation sqlOperation)
        {
            return 0;
        }

        /// <summary>
        /// 在事务中执行批量SQL
        /// </summary>
        /// <param name="sqlOperations">多个SQL操作对象</param>
        /// <returns></returns>
        public bool ExecuteTransactionSql(List<SqlOperation> sqlOperations)
        {
            return false;
        }

    }

}
