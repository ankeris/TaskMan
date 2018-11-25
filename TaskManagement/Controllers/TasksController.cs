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
            ViewData["Creator"] = new SelectList(_context.Account, "AccountId", "AccountEmail");
            ViewData["TaskList"] = new SelectList(_context.TaskState, "TaskStateId", "TaskStateName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,TaskName,TaskCreatedDateTime,TaskDescription,TaskCreatorAccountId,TaskTaskStateId,TaskProjectId,TaskTaskStateLastChangeDateTime")] Models.Task task, int ProjectID)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Projects", new { id = ProjectID });
            }
            ViewData["ProjectCreatorAccountId"] = new SelectList(_context.Account, "AccountId", "AccountEmail", task.TaskCreatorAccountId);
            return View(task);
        }

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int ProjectID)
        {
            var task = await _context.Task.FindAsync(id);
            HttpContext.Session.SetString("DeletedMessage", task.TaskName + " has been successfully deleted");
            _context.Task.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Projects", new { id = ProjectID });
        }
    }
}