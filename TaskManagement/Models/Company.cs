using System;
using System.Collections.Generic;

namespace TaskManagement.Models
{
    public partial class Company
    {
        public Company()
        {
            JAccountCompany = new HashSet<JAccountCompany>();
        }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public DateTime CompanyCreatedDateTime { get; set; }
        public string CompanyInfo { get; set; }

        public ICollection<JAccountCompany> JAccountCompany { get; set; }
    }
}
