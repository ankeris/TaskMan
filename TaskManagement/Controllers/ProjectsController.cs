﻿using System;
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
        private readonly TaskManagementSystemContext _context;
        private int AccID { get; set; }
        private string AccName { get; set; }
        ProjectsViewModel model = new ProjectsViewModel();

        public ProjectsController(TaskManagementSystemContext context)
        {
            _context = context;
        }

        // GET: Projects
        public IActionResult Index()
        {
            if (Int32.TryParse(HttpContext.Session.GetString("AccID"), out int val))
            {
                AccName = HttpContext.Session.GetString("Username");
                AccID = val;
            }
            else
            {
                ViewBag.error = "Account is inaccessible";
                return RedirectToAction("Index", "Home", new { area = "Unlogged" });
            }
            if (HttpContext.Session.GetString("AccID") != null)
            {
                //var managementContext = _context.Project.Include(p => p.ProjectCreatorAccount);
                
                SqlParameter userid = new SqlParameter("@p_account_id", AccID);
                List<Project> projects = _context.Project.FromSql<Project>("exec ProjectList @p_account_id", userid).ToList();
                List<Company> company = _context.Company.FromSql("exec CompanyName @p_account_id", userid).ToList();
                // Constructor for Viewmodel
                model = new ProjectsViewModel
                {
                    Projects = projects,
                    Company = company[0]
                };
                return View(model);
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

            var project = await _context.Project
                .Include(p => p.ProjectCreatorAccount)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
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
