using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CookieOfficial.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace CookieOfficial.Controllers
{
    public class HomeController : Controller
    {
        UserManager<UserLoginInfo> _userManager;
        SignInManager<UserLoginInfo> _SignInManager;

        public HomeController(UserManager<UserLoginInfo> userManager, SignInManager<UserLoginInfo> SignInManager)
        {
            _userManager = userManager;
            _SignInManager = SignInManager;
        }

        public async Task<IActionResult> Index()
        {
            //https://www.cnblogs.com/tdfblog/p/aspnet-core-security-authentication-cookie.html/
            //https://docs.microsoft.com/zh-cn/aspnet/core/security/authentication/cookie?view=aspnetcore-2.2

            //_SignInManager.SignInAsync();

            ClaimsPrincipal principal = new ClaimsPrincipal();
            principal.AddIdentity(new ClaimsIdentity() { });

            await HttpContext.SignInAsync("MyCookieAuthenticationScheme", principal);
            await HttpContext.SignOutAsync("MyCookieAuthenticationScheme");

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
