using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
namespace Common
{
    /// <summary>
    /// action处理类型
    /// </summary>
    public class ActionHandle
    {
        /// <summary>
        /// 获取一个程序集中的所有Controller下的action
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        public static string[] GetActions(System.Reflection.Assembly assembly)
        {
            var actions = new List<string>();
            foreach (var type in assembly.GetTypes())
            {
                if (Activator.CreateInstance(type) is Controller)
                {
                    actions.AddRange(GetActionNames(type));
                }
            }
            return actions.ToArray();
        }
        /// <summary>
        /// 获取controller下的所有action
        /// </summary>
        /// <param name="type">controller类型</param>
        /// <returns></returns>
        static string[] GetActionNames(Type type)
        {
            var list = new List<string>();
            foreach (var method in type.GetMethods())
            {

                var count = list.Count;
                var actionName = string.Empty;
                foreach (var att in method.GetCustomAttributes(false))
                {

                    if (att is RouteAttribute)
                    {
                        list.Add((att as RouteAttribute).Name);
                    }
                    if (att is HttpGetAttribute)
                    {
                        list.Add((att as HttpGetAttribute).Name);
                    }
                    if (att is HttpPostAttribute)
                    {
                        list.Add((att as HttpPostAttribute).Name);
                    }
                    if (att is HttpDeleteAttribute)
                    {
                        list.Add((att as HttpDeleteAttribute).Name);
                    }
                    if (att is HttpPutAttribute)
                    {
                        list.Add((att as HttpPutAttribute).Name);
                    }
                }
                //当没有Route特性时用controller名称和action名称
                if (count == list.Count)
                {
                    list.Add($"/{type.Name}/{method.Name}");
                }
            }
            return list.ToArray();
        }
    }
}
