using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityTest.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<SDUserModel> _signInManager;

        public HomeController(SignInManager<SDUserModel> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var test = _signInManager.IsSignedIn(User);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
