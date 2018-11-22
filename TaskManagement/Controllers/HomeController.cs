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

        private readonly ManagementContext _context;

        public HomeController(ManagementContext context)
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
                HttpContext.Session.SetString("AccID", account.AccountId.ToString());
                HttpContext.Session.SetString("Username", account.AccountUserFirstName);
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
