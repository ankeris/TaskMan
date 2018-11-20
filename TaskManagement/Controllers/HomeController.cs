using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models;


using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace TaskManagement.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult LoggedIn()
        {
            ViewData["Message"] = "Welcome, you have logged in";

            return View();
        }

        public IActionResult Auth(string Username, string Password)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine(Username);
            Console.Write(Password);
            Console.WriteLine("--------------------------------------");
            return View();
        }

        public ViewResult Users() => View(new Dictionary<string, object> {["Placeholder"] = "Placeholder" });
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
