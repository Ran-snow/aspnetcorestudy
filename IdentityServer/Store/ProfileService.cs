using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace IdentityServer.Store
{
    /// <summary>
    /// ProfileService
    /// </summary>
    /// <remarks>
    /// 需要给User额外添加claim时需要
    /// </remarks>
    public class ProfileService : IProfileService
    {
        //private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        //private readonly UserManager<ApplicationUser> _userManager;

        //public ProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        //{
        //    _userManager = userManager;
        //    _claimsFactory = claimsFactory;
        //}

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            ////获得登录用户的ID
            //var sub = context.Subject.GetSubjectId();
            //var user = await _userManager.FindByIdAsync(sub);
            ////创建一个以当前用户为主体的凭证
            //var principal = await _claimsFactory.CreateAsync(user);

            //var claims = principal.Claims.ToList();
            ////idsv服务器的默认claim
            //claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            ////自定义claims区间
            //claims.Add(new Claim(JwtClaimTypes.GivenName, user.UserName));
            //claims.Add(new Claim("headimgurl", user.HeadImgUrl));
            //claims.Add(new Claim("gender", user.Gender));

            ////设置claims
            //context.IssuedClaims = claims;

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            //var sub = context.Subject.GetSubjectId();
            //var user = await _userManager.FindByIdAsync(sub);
            //context.IsActive = user != null;

            return Task.CompletedTask;
        }
    }
}
