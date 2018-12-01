namespace TaskManagement.Models.ViewModels
{
    public class CommentCreateViewModel
    {
        // In this application we have database loop
        // Task <-- Comment --> Project
        // Therefore we need to get ID of either project or task
        // And in string CommentOn tell if it's a project or task
        public int ID { get; set; }
        public string CommentOn { get; set; }
    }
}
