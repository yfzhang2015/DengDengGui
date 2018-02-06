using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanMinDaShangPlatform.Models.Entity
{
    public class T_QMDS_EmployeeUser
    {
        public int ID { get; set; }

        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string EmployeeID { get; set; }
        public int FValid { get; set; }
    }
}
