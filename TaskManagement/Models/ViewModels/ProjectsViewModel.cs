using TaskManagement.Models;
using System.Collections.Generic;

namespace TaskManagement.Models.ViewModels  
{
    // Login view model class.  
    public class ProjectsViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public string CompanyName { get; set; }
    }
}
