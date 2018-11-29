using System;
using System.Collections.Generic;

namespace TaskManagement.Models
{
    public partial class JAccountCompany
    {
        public int AccountId { get; set; }
        public int CompanyId { get; set; }
        public DateTime UserAssignedDateTime { get; set; }
        public bool CompanyCreator { get; set; }
        public bool CompanyOwner { get; set; }

        public Account Account { get; set; }
        public Company Company { get; set; }
    }
}
