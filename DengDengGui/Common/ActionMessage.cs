using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// action信息类
    /// </summary>
    public class ActionMessage
    {
        /// <summary>
        /// action名称
        /// </summary>
        public string ActionName
        {
            get;set;
        }
        /// <summary>
        /// 谓词
        /// </summary>
        public Predicate Predicate
        { get; set; }
    }
}
