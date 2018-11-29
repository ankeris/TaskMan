using System;
using System.Collections.Generic;

namespace TaskManagement.Models
{
    public partial class Account
    {
        public Account()
        {
            Comment = new HashSet<Comment>();
            JAccountCompany = new HashSet<JAccountCompany>();
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

        public UserRole AccountRole { get; set; }
        public ICollection<Comment> Comment { get; set; }
        public ICollection<JAccountCompany> JAccountCompany { get; set; }
        public ICollection<JAccountTask> JAccountTask { get; set; }
        public ICollection<Project> Project { get; set; }
    }
}
