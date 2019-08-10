using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace IdentityServer.Store
{
    public class ResourceStoreCache : IResourceStore
    {
        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            return await Task.Run(() =>
            {
                return Config.GetApis().Where(x => name == x.Name).FirstOrDefault();
            });
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            return await Task.Run(() =>
            {
                return Config.GetApis().Where(x => scopeNames.Contains(x.Name));
            });
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            return await Task.Run(() =>
            {
                return Config.GetIdentityResources().Where(x => scopeNames.Contains(x.Name));
            });
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            return await Task.Run(() =>
            {
                return new Resources
                {
                    ApiResources = Config.GetApis().ToList(),
                    IdentityResources = Config.GetIdentityResources().ToList(),
                    OfflineAccess = true
                };
            });
        }
    }
}
