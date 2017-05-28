using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace GeneralBusinessData.SqlServer
{
    /// <summary>
    /// MSSqlServer语句执行类
    /// </summary>
    public class SqlServerHelper : ISqlHelper
    {
        /// <summary>
        /// 创建MSSqlServerHelper对象
        /// </summary>
        /// <param name="connectionString">MSSqlServer连接字符串</param>
        public SqlServerHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString
        { get; private set; }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="parmeters">参数列表</param>
        /// <returns></returns>
        public List<Dictionary<string, dynamic>> QueryList(string sql, params DbParameter[] parmeters)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = sql;
                command.Parameters.AddRange(parmeters);
                connection.Open();
                var reader = command.ExecuteReader();
                var list = new List<Dictionary<string, dynamic>>();
                //读取list数据
                while (reader.Read())
                {
                    list.Add(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue));
                }
                return list;
            }
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="sqlOperation">SQL操作对象</param>
        /// <returns></returns>
        public List<Dictionary<string, dynamic>> QueryList(SqlOperation sqlOperation)
        {
            return QueryList(sqlOperation.Sql, sqlOperation.parmeters);
        }
        /// <summary>
        /// 查询单值
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="parmeters">参数列表</param>
        /// <returns></returns>
        public object QueryValue(string sql, params DbParameter[] parmeters)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = sql;
                command.Parameters.AddRange(parmeters);
                connection.Open();
                var value = command.ExecuteScalar();
                return value.ToString();
            }
        }
        /// <summary>
        /// 查询单值
        /// </summary>
        /// <param name="sqlOperation">SQL操作对象</param>
        /// <returns></returns>
        public object QueryValue(SqlOperation sqlOperation)
        {
            return QueryValue(sqlOperation.Sql, sqlOperation.parmeters);
        }

        /// <summary>
        /// 增删改数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parmeters">参数列表</param>
        /// <returns></returns>
        public int ChangeData(string sql, params DbParameter[] parmeters)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = sql;
                command.Parameters.AddRange(parmeters);
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 增删改数据
        /// </summary>
        /// <param name="sqlOperation">SQL操作对象</param>
        /// <returns></returns>
        public int ChangeData(SqlOperation sqlOperation)
        {
            return ChangeData(sqlOperation.Sql, sqlOperation.parmeters);
        }

        /// <summary>
        /// 在事务中执行批量SQL
        /// </summary>
        /// <param name="sqlOperations">多个SQL操作对象</param>
        /// <returns></returns>
        public bool ExecuteTransactionSql(List<SqlOperation> sqlOperations)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {
                    var command = new SqlCommand();
                    command.Connection = connection;
                    command.Transaction = transaction;
                    foreach (var sqlOperation in sqlOperations)
                    {
                        command.CommandText = sqlOperation.Sql;
                        command.Parameters.Clear();
                        command.Parameters.AddRange(sqlOperation.parmeters);
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception exc)
                {
                    transaction.Rollback();
                    throw exc;
                }
            }
        }


        /// <summary>
        /// 批量保存单据数据,两表对考
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="pars">sql参数</param>
        /// <returns></returns>
        public bool BlukCopyData(string tableName, List<string> fields, string sql, params SqlParameter[] parmeters)
        {
            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    var cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parmeters);
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    using (var conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        using (var bulkCopy = new SqlBulkCopy(conn))
                        {
                            bulkCopy.DestinationTableName = tableName;
                            foreach (var field in fields)
                            {
                                bulkCopy.ColumnMappings.Add(field, field);
                            }
                            bulkCopy.WriteToServer(dr);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
