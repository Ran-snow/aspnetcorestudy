// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Dave.IdentityProvider
{
    public static class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "Nick",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Nick"),
                        new Claim("family_name", "Carter"),
                        new Claim("email", "12345@qq.com"),
                        new Claim("role", "管理员")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "Dave",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Dave"),
                        new Claim("family_name", "Mustaine"),
                        new Claim("email", "67890@qq.com"),
                        new Claim("role", "注册用户")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("roles", "角色", new List<string>{ "role" })
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("restapi", "RESTful API", new List<string> {
                    "role",
                    "given_name"
                })
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "mvcclient",
                    ClientName = "MVC 客户端",
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    // 登陆后跳转到这
                    RedirectUris =
                    {
                        "https://localhost:5002/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:5002/signout-callback-oidc"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "roles",
                        "restapi"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                }
            };
        }
    }
}