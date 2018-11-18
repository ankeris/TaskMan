using System;
using System.Collections.Generic;

namespace TaskManagement.Models
{
    public partial class TaskState
    {
        public TaskState()
        {
            Task = new HashSet<Task>();
        }

        public int TaskStateId { get; set; }
        public string TaskStateName { get; set; }

        public ICollection<Task> Task { get; set; }
    }
}
