using System;
using System.Collections.Generic;

namespace TaskManagement.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public int? CommentAccountId { get; set; }
        public int? CommentProjectId { get; set; }
        public int? CommentTaskId { get; set; }

        public Account CommentAccount { get; set; }
        public Task CommentProject { get; set; }
        public Project CommentTask { get; set; }
    }
}
