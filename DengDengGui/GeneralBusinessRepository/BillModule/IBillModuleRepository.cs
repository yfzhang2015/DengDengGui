using System;
using System.Collections.Generic;

namespace GeneralBusinessRepository
{
    /// <summary>
    /// 单据仓储处理接口
    /// </summary>
    public interface IBillModuleRepository
    {
        #region 单据模块管理
        List<Dictionary<string, dynamic>> GetBillModules();
        #endregion

    }
}
