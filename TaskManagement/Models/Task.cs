using System;
using System.Collections.Generic;

namespace TaskManagement.Models
{
    public partial class Task
    {
        public Task()
        {
            Comment = new HashSet<Comment>();
            JAccountTask = new HashSet<JAccountTask>();
        }

        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public DateTime TaskCreatedDateTime { get; set; }
        public string TaskDescription { get; set; }
        public int TaskProjectId { get; set; }
        public int TaskTaskStateId { get; set; }
        public DateTime TaskTaskStateLastChangeDateTime { get; set; }

        public TaskState TaskTaskState { get; set; }
        public ICollection<Comment> Comment { get; set; }
        public ICollection<JAccountTask> JAccountTask { get; set; }
    }
}
