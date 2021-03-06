﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// 全部小写ContractResolver
    /// </summary>
    public class LowercaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        /// <summary>
        /// 重写ResolvePropertyName
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}
