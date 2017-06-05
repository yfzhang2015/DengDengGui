using System;
using System.Collections.Generic;

namespace GeneralBusinessRepository
{
    /// <summary>
    /// 查询仓储处理接口
    /// </summary>
    public interface IQueryModuleRepository
    {
        #region 查询模块管理
        List<Dictionary<string, dynamic>> GetQueryModules();

        #endregion

    }
}
