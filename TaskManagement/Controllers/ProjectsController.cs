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

            if (HttpContext.Session.GetString("DeletedMessage") != null)
            {
                DeletedMessage = HttpContext.Session.GetString("DeletedMessage");
                HttpContext.Session.Remove("DeletedMessage");
            }
            ViewBag.DeletedMessage = DeletedMessage;

            var project = await _context.Project
                .Include(p => p.ProjectCreatorAccount)
                .FirstOrDefaultAsync(m => m.ProjectId == id);

            //returns the first occurrence within the entire List
            //Project project = projects.Find(x => x.ProjectId == id);

            List<Models.Task> tasks = await _context.Task
                .Where(t => t.TaskProjectId == id).ToListAsync();

            details_model = new ProjectDetailsViewModel
            {
                Project = project,
                Tasks = tasks
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,ProjectName,ProjectCreatedDateTime,ProjectDeadline,ProjectDescription,ProjectCreatorAccountId,ProjectActive,ProjectEndDateTime")] Project project)
        {
            if (ModelState.IsValid)
            {
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            var project = await _context.Project.FindAsync(id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }
    }
}
