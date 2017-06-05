using GeneralBusinessData;
using System;
using System.Collections.Generic;

namespace GeneralBusinessRepository.SqlServer
{
    /// <summary>
    /// 查询模块仓储处理类
    /// </summary>
    public class QueryModuleRepository : IQueryModuleRepository
    {
        /// <summary>
        /// 数据库操作类
        /// </summary>
        ISqlHelper _sqlHelper;
        /// <summary>
        /// 实例化sqlserver的仓储对象
        /// </summary>
        /// <param name="sqlHelper"></param>
        public QueryModuleRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        #region 查询模块管理
        public List<Dictionary<string, dynamic>> GetQueryModules()
        {
            var sql = "select * from querymodules";
            return _sqlHelper.QueryList(sql);
        }
        #endregion
    }
}
