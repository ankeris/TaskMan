using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TaskManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        [Route("")]
        [Route("index")]
        [Route("~/")]
        public IActionResult Index()
        {
            return View();
        }

        private readonly TaskManagementContext _context;

        public HomeController(TaskManagementContext context)
        {
            _context = context;
        }

        [Route("Login")]
        [HttpPost]
        public IActionResult Login(Account AccData)
        {
            var account = _context.Account.Where(u => u.AccountEmail == AccData.AccountEmail).FirstOrDefault();
            // && u.AccountPassword == AccData.AccountPassword
            if (account != null)
            {
                // Stores few account details into sessions
                HttpContext.Session.SetInt32("AccID", account.AccountId);
                HttpContext.Session.SetString("Username", account.AccountUserFirstName);
                HttpContext.Session.SetInt32("RoleID", account.AccountRoleId);

                ViewBag.Username = HttpContext.Session.GetString("Username");
                return RedirectToAction("Index", "Projects", new { area = "Employee" });
            }
            else
            {
                ModelState.AddModelError("", "Username or password is wrong.");
                ViewBag.error = "Invalid Account";
                return View("Index");
            }
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("AccID");
            return RedirectToAction("Index");
        }

        public IActionResult LoggedIn()
        {
            return View("LoggedIn");
        }
    }
}
