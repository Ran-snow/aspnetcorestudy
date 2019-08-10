using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace IdentityServer.Store
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //根据context.UserName和context.Password与数据库的数据做校验，判断是否合法
            //if (context.UserName == "jian1" && context.Password == "j1")
            //{
            //    context.Result = new GrantValidationResult(
            //     subject: context.UserName,
            //     authenticationMethod: OidcConstants.AuthenticationMethods.Password);
            //}
            //else
            //{

            //    //验证失败
            //    context.Result = new GrantValidationResult(
            //        TokenRequestErrors.InvalidGrant,
            //        "invalid custom credential"
            //        );
            //}

            return Task.CompletedTask;
        }
    }
}
