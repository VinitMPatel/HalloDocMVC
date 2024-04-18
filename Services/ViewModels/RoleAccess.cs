using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class RoleAccess
    {
        public List<Menu>? menuList { get; set; }

        public List<Role>? rolemenuList { get; set; }

        public int roleId { get; set; }

        public string? roleName { get; set; }

        public int accountType { get; set; }

        public List<Menu>? selectedMenu { get; set; }
    }


    public class UserAccessData
    {
        public List<UserAccess> userAccessList { get; set;}
    }


    public class UserAccess
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public int accountType { get; set; }

        public string Phone { get; set; }

        public short Status { get; set; }

        public int openRequest { get; set; }
    }
}
