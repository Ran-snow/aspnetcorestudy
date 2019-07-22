using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MVC.Client.Hybird.Requirement
{
    public class AliceInSomewhereHandler : AuthorizationHandler<AliceInSomewhereRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AliceInSomewhereRequirement requirement)
        {
            var filterContext = context.Resource as AuthorizationFilterContext;
            if (filterContext == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var email = context.User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value;

            if (email == "AliceSmith@email.com")
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            context.Fail();
            return Task.CompletedTask;

            //!! 若想满足requirement 必须没有Fail且有一个Succeed
        }
    }
}
