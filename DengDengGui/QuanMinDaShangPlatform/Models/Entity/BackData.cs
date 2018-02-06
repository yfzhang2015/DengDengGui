using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanMinDaShangPlatform.Models.Entity
{
    /// <summary>
    /// 返回值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BackData<T>
    {
        public bool Result { get; set; }
        public T Data { get; set; }
        public string ErrMeg { get; set; }
    }
}
