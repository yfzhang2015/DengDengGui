using System;
using System.Collections.Generic;

namespace GeneralBusinessRepository
{
    /// <summary>
    /// 通用业务平台仓储处理接口
    /// </summary>
    public interface IBusinessRepository
    {

        #region 菜单管理
        /// <summary>
        /// 按用户名查询菜单
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        List<Dictionary<string, dynamic>> GetMenusByUserName(string userName);



        #endregion
    }
}
