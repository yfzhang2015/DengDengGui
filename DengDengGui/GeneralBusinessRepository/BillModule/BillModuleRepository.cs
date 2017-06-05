using GeneralBusinessData;
using System;
using System.Collections.Generic;

namespace GeneralBusinessRepository.SqlServer
{
    /// <summary>
    /// 单据模块仓储处理类
    /// </summary>
    public class BillModuleRepository: IBillModuleRepository
    {
        /// <summary>
        /// 数据库操作类
        /// </summary>
        ISqlHelper _sqlHelper;
        /// <summary>
        /// 实例化sqlserver的仓储对象
        /// </summary>
        /// <param name="sqlHelper"></param>
        public BillModuleRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        #region 单据模块管理
        public List<Dictionary<string, dynamic>> GetBillModules()
        {
            var sql = "select * from billmodules";
            return _sqlHelper.QueryList(sql);
        }
        #endregion
    }
}
