using GeneralBusinessData;
using System;

namespace GeneralBusinessRepository
{
    /// <summary>
    /// 通用业务平台仓储处理类（For MS SqlServer)
    /// </summary>
    public class BusinessForSqlServerRepository:IBusinessRepository
    {
        /// <summary>
        /// 数据库操作类
        /// </summary>
        ISqlHelper _sqlHelper;
        /// <summary>
        /// 实例化sqlserver的仓储对象
        /// </summary>
        /// <param name="sqlHelper"></param>
        public BusinessForSqlServerRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
     
    }
}
