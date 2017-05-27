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
        public static ActionMessage[] GetActions()
        {
            var assembly = Assembly.GetEntryAssembly();
            var actions = new List<ActionMessage>();
            foreach (var type in assembly.GetTypes())
            {

                if (typeof(Controller).IsAssignableFrom(type))
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
        static ActionMessage[] GetActionNames(Type type)
        {
            var list = new List<ActionMessage>();
            foreach (var method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance))
            {

                var count = list.Count;
                var actionName = string.Empty;
                foreach (var att in method.GetCustomAttributes(false))
                {
                    //todo 这里有bug
                    if (att is RouteAttribute && (att as RouteAttribute).Template != null)
                    {
                        list.Add(new ActionMessage() { ControllerName=type.Name, ActionName = (att as RouteAttribute).Template, Predicate = Predicate.Get });
                    }

                    if (att is HttpGetAttribute)
                    {
                        if ((att as HttpGetAttribute).Template != null)
                        {
                            list.Add(new ActionMessage() { ControllerName = type.Name, ActionName = $"/{(att as HttpGetAttribute).Template}", Predicate = Predicate.Get });
                        }
                        else
                        {
                            list.Add(new ActionMessage() { ControllerName = type.Name, ActionName = $"/{type.Name}/{method.Name}", Predicate = Predicate.Get });
                        }
                    }
                    if (att is HttpPostAttribute)
                    {
                        if ((att as HttpPostAttribute).Template != null)
                        {
                            list.Add(new ActionMessage() { ControllerName = type.Name, ActionName = $"/{(att as HttpPostAttribute).Template}", Predicate = Predicate.Post });
                        }
                        else
                        {
                            list.Add(new ActionMessage() { ControllerName = type.Name, ActionName = $"/{type.Name}/{method.Name}", Predicate = Predicate.Post });
                        }
                    }
                    if (att is HttpDeleteAttribute)
                    {
                        if ((att as HttpDeleteAttribute).Template != null)
                        {
                            list.Add(new ActionMessage() { ControllerName = type.Name, ActionName = $"/{(att as HttpDeleteAttribute).Template}", Predicate = Predicate.Delete });
                        }
                        else
                        {
                            list.Add(new ActionMessage() { ControllerName = type.Name, ActionName = $"/{type.Name}/{method.Name}", Predicate = Predicate.Delete });
                        }
                    }
                    if (att is HttpPutAttribute)
                    {
                        if ((att as HttpPutAttribute).Template != null)
                        {
                            list.Add(new ActionMessage() { ControllerName = type.Name, ActionName = $"/{(att as HttpPutAttribute).Template}", Predicate = Predicate.Put });
                        }
                        else
                        {
                            list.Add(new ActionMessage() { ControllerName = type.Name, ActionName = $"/{type.Name}/{method.Name}", Predicate = Predicate.Put });
                        }
                    }
                }
                //当没有Route特性时用controller名称和action名称
                if (count == list.Count)
                {
                    list.Add(new ActionMessage() { ControllerName = type.Name, ActionName = $"/{type.Name}/{method.Name}", Predicate = Predicate.Get });
                }
            }
            return list.ToArray();
        }
    }
}
