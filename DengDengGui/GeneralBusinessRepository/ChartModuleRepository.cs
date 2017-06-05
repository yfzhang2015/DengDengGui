using GeneralBusinessData;
using System;
using System.Collections.Generic;

namespace GeneralBusinessRepository.SqlServer
{
    /// <summary>
    /// 图表模块仓储处理类
    /// </summary>
    public class ChartModuleRepository : IChartModuleRepository
    {
        /// <summary>
        /// 数据库操作类
        /// </summary>
        ISqlHelper _sqlHelper;
        /// <summary>
        /// 实例化sqlserver的仓储对象
        /// </summary>
        /// <param name="sqlHelper"></param>
        public ChartModuleRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        #region 图表模块管理
        public List<Dictionary<string, dynamic>> GetChartModules()
        {
            var sql = "select * from chartmodules";
            return _sqlHelper.QueryList(sql);
        }
        #endregion
    }
}
