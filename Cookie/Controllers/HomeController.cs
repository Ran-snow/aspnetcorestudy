using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cookie.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Cookie.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //public HomeController(IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}

        public async Task<IActionResult> Index()
        {
            if (HttpContext.User.Claims.Where(x => x.Type == "userId").FirstOrDefault() == null)
            {

                var claims = new[] { new Claim("userId", "2") };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24),//24
                };

                await HttpContext.SignInAsync(
                                   CookieAuthenticationDefaults.AuthenticationScheme,
                                   new ClaimsPrincipal(claimsIdentity),
                                   authProperties);
            }

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
