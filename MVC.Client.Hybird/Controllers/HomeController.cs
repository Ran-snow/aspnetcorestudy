using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MVC.Client.Hybrid.Models;

namespace MVC.Client.Hybrid.Controllers
{
    //[Authorize(Roles = "admin")]
    [Authorize(Policy = "AliceInSomewhere")]
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var claims = User.IsInRole("admin");
            var client = new HttpClient();
            string content = string.Empty;

            // call api
            client.SetBearerToken(await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken));

            var response = await client.GetAsync("http://localhost:5001/api/values");
            if (!response.IsSuccessStatusCode)
            {
                content = response.ReasonPhrase;
            }
            else
            {
                content = await response.Content.ReadAsStringAsync();
            }

            return View("index", content);
        }

        public async Task<IActionResult> Privacy()
        {
            var accesstoken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var idToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            var code = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.Code);

            ViewData["accesstoken"] = accesstoken;
            ViewData["idToken"] = idToken;
            ViewData["refreshToken"] = refreshToken;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
