using TaskManagement.Models;
using System.Collections.Generic;

namespace TaskManagement.Models.ViewModels
{
    // Login view model class.  
    public class ProjectDetailsViewModel
    {
        public Project Project { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
    }
}
