using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MVC.Client.Code.Models;

namespace MVC.Client.Code.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            string content = string.Empty;

            // call api
            client.SetBearerToken(await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken));

            var response = await client.GetAsync("http://localhost:5001/api/values");
            if (!response.IsSuccessStatusCode)
            {
                //if (response.StatusCode == HttpStatusCode.Unauthorized)
                //{
                //    await RenewTokensAsync();
                //    return RedirectToAction();
                //}

                content = response.ReasonPhrase;
            }
            else
            {
                content = await response.Content.ReadAsStringAsync();
            }

            return View("index", content);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        private async Task<string> RenewTokensAsync()
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            var tokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = disco.TokenEndpoint,
                RefreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken),
                ClientId = "mvc_code",
                ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0",
                Scope = "api1",
                GrantType = OpenIdConnectGrantTypes.RefreshToken
            });

            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }

            var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);

            var tokens = new[]
{
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.IdToken,
                    Value = tokenResponse.IdentityToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = tokenResponse.AccessToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = tokenResponse.RefreshToken
                },
                new AuthenticationToken
                {
                    Name = "expires_at",
                    Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                }
            };

            // 获取身份认证的结果，包含当前的pricipal和properties
            var currentAuthenticateResult =
                await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // 把新的tokens存起来
            currentAuthenticateResult.Properties.StoreTokens(tokens);

            // 登录
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                currentAuthenticateResult.Principal, currentAuthenticateResult.Properties);

            return tokenResponse.AccessToken;
        }

        [Authorize]
        public async Task<IActionResult> GetClaim()
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
