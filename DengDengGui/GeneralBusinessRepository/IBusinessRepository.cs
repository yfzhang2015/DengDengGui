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
        /// 查询全部菜单
        /// </summary>
        /// <returns></returns>
        List<Dictionary<string, dynamic>> GetMenus();
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        int AddMenu(string name);

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        int ModifyMenu(int id, string name);

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        int RemoveMenu(int id);


        /// <summary>
        /// 按用户名查询菜单
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        List<Dictionary<string, dynamic>> GetMenusByUserName(string userName);



        #endregion


        #region 单据模块管理
        List<Dictionary<string, dynamic>> GetBillModules();
        #endregion

        #region 查询模块管理
        List<Dictionary<string, dynamic>> GetQueryModules();

        #endregion
        #region 图表模块管理
         List<Dictionary<string, dynamic>> GetChartModules();        
        #endregion
    }
}
