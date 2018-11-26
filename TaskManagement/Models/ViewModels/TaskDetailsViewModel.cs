using TaskManagement.Models;
using System.Collections.Generic;

namespace TaskManagement.Models.ViewModels
{
    // Login view model class.  
    public class TaskDetailsViewModel
    {
        public Task Task { get; set; }
        public List<Account> Assignees { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
