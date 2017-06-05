using System;
using System.Collections.Generic;

namespace GeneralBusinessRepository
{
    /// <summary>
    /// 图表仓储处理接口
    /// </summary>
    public interface IChartModuleRepository
    {
        #region 图表模块管理
        List<Dictionary<string, dynamic>> GetChartModules();
        #endregion

    }
}
