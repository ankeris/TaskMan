using System;
using System.Collections.Generic;

namespace TaskManagement.Models
{
    public partial class UserRole
    {
        public UserRole()
        {
            Account = new HashSet<Account>();
        }

        public int UserRoleId { get; set; }
        public string UserRoleName { get; set; }

        public ICollection<Account> Account { get; set; }
    }
}
