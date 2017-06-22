using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Routing;

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
        /// <param name="exclude">排除的Controller或action</param>
        /// <returns></returns>
        public static ActionMessage[] GetActions(params string[] exclude)
        {
            var assembly = Assembly.GetEntryAssembly();
            var actions = new List<ActionMessage>();
            foreach (var type in assembly.GetTypes())
            {

                if (typeof(Controller).IsAssignableFrom(type))
                {
                    actions.AddRange(GetActionNames(type, exclude));
                }
            }
            return actions.ToArray();
        }
        /// <summary>
        /// 获取controller下的所有action
        /// </summary>
        /// <param name="type">controller类型</param>
        /// <returns></returns>
        static ActionMessage[] GetActionNames(Type type, params string[] exclude)
        {
            var list = new List<ActionMessage>();
            foreach (var method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance))
            {
                var controllerName = type.Name.ToLower();
                //判断本controller是否包含在排除中
                if (IsContain(exclude, controllerName))
                {
                    continue;
                }
                var count = list.Count;
                foreach (var att in method.GetCustomAttributes(false))
                {
                    var actionName = $"/{(att as HttpMethodAttribute).Template.ToLower().Split('{')[0].TrimEnd('/').TrimEnd('\\')}";
                    //判断本action是否包含在排除中
                    if (IsContain(exclude, actionName))
                    {
                        continue;
                    }
                    if (att is RouteAttribute && (att as RouteAttribute).Template != null)
                    {                  
                        if (IsContain(exclude, actionName))
                        {
                            continue;
                        }
                        list.Add(new ActionMessage() { ControllerName = controllerName, ActionName = actionName, Predicate = Predicate.Get });
                    }

                    if (att is HttpGetAttribute)
                    {
                        if ((att as HttpGetAttribute).Template != null)
                        {
                            list.Add(new ActionMessage() { ControllerName = controllerName, ActionName = actionName, Predicate = Predicate.Get });
                        }
                        else
                        {
                            list.Add(new ActionMessage() { ControllerName = controllerName, ActionName = actionName, Predicate = Predicate.Get });
                        }
                    }
                    if (att is HttpPostAttribute)
                    {
                        if ((att as HttpPostAttribute).Template != null)
                        {
                            list.Add(new ActionMessage() { ControllerName = controllerName, ActionName = actionName, Predicate = Predicate.Post });
                        }
                        else
                        {
                            list.Add(new ActionMessage() { ControllerName = controllerName, ActionName = actionName, Predicate = Predicate.Post });
                        }
                    }
                    if (att is HttpDeleteAttribute)
                    {
                        if ((att as HttpDeleteAttribute).Template != null)
                        {
                            list.Add(new ActionMessage() { ControllerName = controllerName, ActionName = actionName, Predicate = Predicate.Delete });
                        }
                        else
                        {
                            list.Add(new ActionMessage() { ControllerName = controllerName, ActionName = actionName, Predicate = Predicate.Delete });
                        }
                    }
                    if (att is HttpPutAttribute)
                    {
                        if ((att as HttpPutAttribute).Template != null)
                        {
                            list.Add(new ActionMessage() { ControllerName = controllerName, ActionName = actionName, Predicate = Predicate.Put });
                        }
                        else
                        {
                            list.Add(new ActionMessage() { ControllerName = controllerName, ActionName = actionName, Predicate = Predicate.Put });
                        }
                    }
                }
                //当没有Route特性时用controller名称和action名称
                if (count == list.Count)
                {
                    list.Add(new ActionMessage() { ControllerName = controllerName, ActionName = $"/{controllerName.Replace("controller", "")}/{method.Name.ToLower().Split('{')[0].TrimEnd('/').TrimEnd('\\')}", Predicate = Predicate.Get });
                }
            }
            return list.ToArray();
        }
        /// <summary>
        /// 判断value是否包含在exclued数组里
        /// </summary>
        /// <param name="exclude">数组</param>
        /// <param name="value">单值</param>
        /// <returns></returns>
        static bool IsContain(string[] exclude, string value)
        {
            foreach (var item in exclude)
            {
                if (item.ToLower() == value.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
