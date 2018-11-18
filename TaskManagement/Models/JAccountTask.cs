using System;
using System.Collections.Generic;

namespace TaskManagement.Models
{
    public partial class JAccountTask
    {
        public int AccountId { get; set; }
        public int TaskId { get; set; }

        public Account Account { get; set; }
        public Task Task { get; set; }
    }
}
