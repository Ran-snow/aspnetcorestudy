using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;

namespace JwtAuthSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        JwtSettings _JwtSettings;

        public AuthorizeController(IOptions<JwtSettings> jwtSettings)
        {
            _JwtSettings = jwtSettings.Value;
        }

        public IActionResult Token(LoginViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            if (!(model.User.Equals("tom") && model.Password.Equals("666"))) return BadRequest();

            var claims = new Claim[]{
                new Claim (ClaimTypes.Name,"tom"),
                new Claim (ClaimTypes.Role,"admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _JwtSettings.Issuer,
                _JwtSettings.Audience,
                claims,
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
