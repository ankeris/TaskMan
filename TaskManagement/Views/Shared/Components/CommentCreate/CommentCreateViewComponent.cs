using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Models.ViewModels;

namespace TaskManagement.Views.Shared.Components.CommentCreate
{
    public class CommentCreateViewComponent : ViewComponent
    {
        public CommentCreateViewComponent() { }
        public IViewComponentResult Invoke(int id, string commentOn)
        {
            CommentCreateViewModel details = new CommentCreateViewModel
            {
                ID = id,
                CommentOn = commentOn
            };
            return View("Default", details);
        }
    }
}
