using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;
using TaskManagement.Models.ViewModels;

namespace TaskManagement.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskManagementSystemContext _context;
        TaskDetailsViewModel task_details_model = new TaskDetailsViewModel();

        public TasksController(TaskManagementSystemContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Projects/Create
        public IActionResult Create(int? id)
        {
            ViewBag.ProjectID = id;
            ViewData["TaskCreatorAccountId"] = new SelectList(_context.Account, "AccountId", "AccountEmail");
            ViewData["TaskList"] = new SelectList(_context.TaskState, "TaskStateId", "TaskStateName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,TaskName,TaskCreatedDateTime,TaskDescription,TaskCreatorAccountId,TaskTaskStateId,TaskProjectId,TaskTaskStateLastChangeDateTime")] Models.Task task, [Bind("AccountId,TaskId")] JAccountTask junction, int ProjectID)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                junction.TaskId = task.TaskId;
                _context.Add(junction);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Projects", new { id = ProjectID });
            }
            ViewData["TaskCreatorAccountId"] = new SelectList(_context.Account, "AccountId", "AccountEmail");
            ViewData["ProjectCreatorAccountId"] = new SelectList(_context.Account, "AccountId", "AccountEmail", task.TaskCreatorAccountId);
            return View(task);
        }

        // Only render the View for Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Models.Task task = await _context.Task.Where(tsk => tsk.TaskId == id).FirstOrDefaultAsync();
            task.TaskTaskState = await _context.TaskState.Where(ts => ts.TaskStateId == task.TaskTaskStateId).FirstOrDefaultAsync();
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // Actually Delete Task
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int ProjectID)
        {
            var task = await _context.Task.FindAsync(id);
            var j_task_acc = _context.JAccountTask.Where(t => t.TaskId == id);
            HttpContext.Session.SetString("DeletedMessage", task.TaskName + " has been successfully deleted");

            _context.JAccountTask.RemoveRange(j_task_acc);
            _context.Task.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Projects", new { id = ProjectID });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FirstOrDefaultAsync(m => m.TaskId == id);

            List<Account> assignees = await
                (from acc in _context.Account
                 join jt in _context.JAccountTask
                 on acc.AccountId equals jt.AccountId
                 where jt.TaskId == id
                 select acc).ToListAsync();

            List<Comment> comments = await _context.Comment.Where(c => c.CommentProjectId == id).ToListAsync();

            task_details_model = new TaskDetailsViewModel
            {
                Task = task,
                Assignees = assignees,
                Comments = comments
            };

            return View(task_details_model);
        }
    }
}