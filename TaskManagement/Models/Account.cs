using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Models
{
    public partial class Account : IdentityUser
    {
        public Account()
        {
            Comment = new HashSet<Comment>();
            Company = new HashSet<Company>();
            JAccountTask = new HashSet<JAccountTask>();
            Project = new HashSet<Project>();
        }

        public int AccountId { get; set; }
        public string AccountEmail { get; set; }
        public string AccountPassword { get; set; }
        public DateTime AccountCreatedDateTime { get; set; }
        public string AccountUserFirstName { get; set; }
        public string AccountUserLastName { get; set; }
        public DateTime? AccountUserBirthDate { get; set; }
        public int AccountRoleId { get; set; }
        public DateTime AccountRoleLastChangeDateTime { get; set; }
        public DateTime AccountUserCompanyLastChangeDateTime { get; set; }
        public int? AccountUserCompanyId { get; set; }

        public UserRole AccountRole { get; set; }
        public Company AccountUserCompany { get; set; }
        public ICollection<Comment> Comment { get; set; }
        public ICollection<Company> Company { get; set; }
        public ICollection<JAccountTask> JAccountTask { get; set; }
        public ICollection<Project> Project { get; set; }
    }
}
