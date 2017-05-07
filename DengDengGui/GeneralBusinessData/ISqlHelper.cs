using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;


namespace GeneralBusinessData
{
    public interface ISqlHelper
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="parmeters">参数列表</param>
        /// <returns></returns>
        List<Dictionary<string, dynamic>> QueryList(string sql, params DbParameter[] parmeters);
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="sqlOperation">SQL操作对象</param>
        /// <returns></returns>
        List<Dictionary<string, dynamic>> QueryList(SqlOperation sqlOperation);
        /// <summary>
        /// 查询单值
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="parmeters">参数列表</param>
        /// <returns></returns>
        object QueryValue(string sql, params DbParameter[] parmeters);
        /// <summary>
        /// 查询单值
        /// </summary>
        /// <param name="sqlOperation">SQL操作对象</param>
        /// <returns></returns>
        object QueryValue(SqlOperation sqlOperation);

        /// <summary>
        /// 增删改数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parmeters">参数列表</param>
        /// <returns></returns>
        int ChangeData(string sql, params DbParameter[] parmeters);

        /// <summary>
        /// 增删改数据
        /// </summary>
        /// <param name="sqlOperation">SQL操作对象</param>
        /// <returns></returns>
        int ChangeData(SqlOperation sqlOperation);

        /// <summary>
        /// 在事务中执行批量SQL
        /// </summary>
        /// <param name="sqlOperations">多个SQL操作对象</param>
        /// <returns></returns>
        bool ExecuteTransactionSql(List<SqlOperation> sqlOperations);

    }

}
