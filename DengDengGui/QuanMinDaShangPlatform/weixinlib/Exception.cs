using System;
using System.Collections.Generic;
using System.Web;

namespace QuanMinDaShangPlatform
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}