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
}
