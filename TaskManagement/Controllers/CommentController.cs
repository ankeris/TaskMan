using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    public class CommentController : Controller
    {
        private readonly TaskManagementContext _context;
        private bool accountLoggedIn { get; set; }
        public void AuthorizeUser()
        {
            if (HttpContext.Session.GetInt32("AccID") != null)
            {
                accountLoggedIn = true;
            }
            else
            {
                ViewBag.error = "Account is inaccessible";
                accountLoggedIn = false;
            }
        }

        public CommentController(TaskManagementContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("CommentText")] Comment comm, int id, string on)
        {
            if (on == "Project")
            {
                if (comm.CommentText == null)
                {
                    HttpContext.Session.SetString("errorMessage", "Comment has to include characters!");
                    return RedirectToAction("Details", "Projects", new { id });
                }

                comm.CommentAccountId = HttpContext.Session.GetInt32("AccID");
                comm.CommentProjectId = id;
                comm.CommentTaskId = null;
                _context.Add(comm);
                await _context.SaveChangesAsync();
                // Redirect to Project where where comment belongs
                return RedirectToAction("Details", "Projects", new { id });
            }
            if (on == "Task")
            {
                if (comm.CommentText == null)
                {
                    HttpContext.Session.SetString("errorMessage", "Comment has to include characters!");
                    return RedirectToAction("Details", "Tasks", new { id });
                }
                comm.CommentAccountId = HttpContext.Session.GetInt32("AccID");
                comm.CommentTaskId = id;
                comm.CommentProjectId = null;
                _context.Add(comm);
                await _context.SaveChangesAsync();
                // Redirect to Project where where comment belongs
                return RedirectToAction("Details", "Tasks", new { id });
            }
            return RedirectToAction("Index", "Home");
        }

        // Only render the View for Delete
        public IActionResult Delete(int? id)
        {
            AuthorizeUser();
            if (id == null) { return NotFound(); }

            Comment comm = _context.Comment.Where(tsk => tsk.CommentId == id).FirstOrDefault();
            comm.CommentAccount = _context.Account.Where(acc => acc.AccountId == comm.CommentAccountId).FirstOrDefault();

            if (comm == null) { return NotFound(); }

            if (accountLoggedIn)
            {
                return View(comm);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { Unlogged = 1 });
            }
        }

        // Actually Delete Task
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CommentId)
        {
            var comm = await _context.Comment.FindAsync(CommentId);
            HttpContext.Session.SetString("DeletedMessage", "Comment has been successfully deleted");

            _context.Comment.Remove(comm);
            await _context.SaveChangesAsync();

            if (comm.CommentTaskId.HasValue)
            {
                return RedirectToAction("Details", "Tasks", new { id = comm.CommentTaskId });
            }
            if (comm.CommentProjectId.HasValue)
            {
                return RedirectToAction("Details", "Projects", new { id = comm.CommentProjectId });
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Back(int id)
        {
            var comm = await _context.Comment.FindAsync(id);

            if (comm == null) { return NotFound(); }

            if (comm.CommentTaskId.HasValue)
            {
                return RedirectToAction("Details", "Tasks", new { id = comm.CommentTaskId });
            }
            if (comm.CommentProjectId.HasValue)
            {
                return RedirectToAction("Details", "Projects", new { id = comm.CommentProjectId });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}