using TaskManagement.Models;
using System.Collections.Generic;

namespace TaskManagement.Models.ViewModels
{
    // Login view model class.  
    public class ProjectsPageViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public Company Company { get; set; }
    }
}
