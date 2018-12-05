using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public class ProjectsController : Controller
    {
        // Global Variables for this Controller
        private readonly TaskManagementContext _context;
        private int? AccID { get; set; }
        private string AccName { get; set; }
        private string DeletedMessage { get; set; }
        private string errorMessage { get; set; }
        public void GetNotifications()
        {
            if (HttpContext.Session.GetString("DeletedMessage") != null)
            {
                DeletedMessage = HttpContext.Session.GetString("DeletedMessage");
                HttpContext.Session.Remove("DeletedMessage");
            }
            if (HttpContext.Session.GetString("errorMessage") != null)
            {
                errorMessage = HttpContext.Session.GetString("errorMessage");
                HttpContext.Session.Remove("errorMessage");
            }
            ViewBag.errorMessage = errorMessage;
            ViewBag.DeletedMessage = DeletedMessage;
        }

        public List<Project> projects = new List<Project>();
        public List<Company> company = new List<Company>();

        ProjectsPageViewModel projects_model = new ProjectsPageViewModel();
        ProjectDetailsViewModel details_model = new ProjectDetailsViewModel();

        public ProjectsController(TaskManagementContext context)
        {
            _context = context;
        }

        // GET: Projects
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("AccID") != null)
            {
                AccName = HttpContext.Session.GetString("Username");
                AccID = HttpContext.Session.GetInt32("AccID");
            }
            else
            {
                ViewBag.error = "Account is inaccessible";
                return RedirectToAction("Index", "Home", new { area = "Unlogged" });
            }

            // User Login successful!
            if (HttpContext.Session.GetString("AccID") != null)
            {
                SqlParameter userid = new SqlParameter("@p_account_id", AccID);
                try
                {
                    projects = _context.Project.FromSql("exec ProjectList @p_account_id", userid).ToList();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Log Exception: " + ex.Message);
                }
                company = _context.Company.FromSql("exec CompanyName @p_account_id", userid).ToList();

                GetNotifications();
                // Count all "Done" tasks and set DoneTasks object property
                projects.ForEach(
                    prj => prj.DoneTasks =
                    _context.Task
                    .Where(tsk =>
                    tsk.TaskProjectId == prj.ProjectId
                    &&
                    tsk.TaskTaskStateId == 6)
                    .Count());
                // Count sum of all Tasks and set TotalTasks object property
                projects.ForEach(
                    prj => prj.TotalTasks =
                    _context.Task
                    .Where(tsk =>
                    tsk.TaskProjectId == prj.ProjectId)
                    .Count());

                // Constructor for Viewmodel
                projects_model = new ProjectsPageViewModel
                {
                    Projects = projects,
                    Company = company[0]
                };
                return View(projects_model);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "Unlogged" });
            }
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            GetNotifications();
            // Project details
            var project = await _context.Project
                .Include(p => p.ProjectCreatorAccount)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            // Find relevant tasks for Project
            List<Models.Task> tasks = await _context.Task
                .Where(t => t.TaskProjectId == id).ToListAsync();
            // Find all comments that are related to project
            List<Comment> comments = await _context.Comment
                .Where(t => t.CommentProjectId == id).ToListAsync();

            comments.ForEach(comm => comm.CommentAccount = _context.Account.Where(acc => acc.AccountId == comm.CommentAccountId).FirstOrDefault());

            details_model = new ProjectDetailsViewModel
            {
                Project = project,
                Tasks = tasks,
                Comments = comments
            };
            if (project == null)
            {
                return NotFound();
            }

            return View(details_model);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            ViewData["ProjectCreatorAccountId"] = new SelectList(_context.Account, "AccountId", "AccountEmail");
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,ProjectName,ProjectCreatedDateTime,ProjectDeadline,ProjectDescription,ProjectCreatorAccountId,ProjectActive,ProjectEndDateTime")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.ProjectCreatedDateTime = DateTime.Now;
                project.ProjectEndDateTime = null;
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectCreatorAccountId"] = new SelectList(_context.Account, "AccountId", "AccountEmail", project.ProjectCreatorAccountId);
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["ProjectCreatorAccountId"] = new SelectList(_context.Account, "AccountId", "AccountEmail", project.ProjectCreatorAccountId);
            return View(project);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,ProjectName,ProjectCreatedDateTime,ProjectDeadline,ProjectDescription,ProjectCreatorAccountId,ProjectActive,ProjectEndDateTime")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectCreatorAccountId"] = new SelectList(_context.Account, "AccountId", "AccountEmail", project.ProjectCreatorAccountId);
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .Include(p => p.ProjectCreatorAccount)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comments = _context.Comment.Where(t => t.CommentProjectId == id);
            _context.Comment.RemoveRange(comments);

            var project = await _context.Project.FindAsync(id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("DeletedMessage", $"Project {project.ProjectName} has been deleted!");
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }
    }
}
