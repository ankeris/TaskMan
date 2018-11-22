using System;
using System.Collections.Generic;

namespace TaskManagement.Models
{
    public partial class Company
    {
        public Company()
        {
            Account = new HashSet<Account>();
        }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public DateTime CompanyCreatedDateTime { get; set; }
        public string CompanyInfo { get; set; }
        public int? CompanyCreatorAccountId { get; set; }

        public Account CompanyCreatorAccount { get; set; }
        public ICollection<Account> Account { get; set; }
    }
}
