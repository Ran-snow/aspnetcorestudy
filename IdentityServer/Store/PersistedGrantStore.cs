using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace IdentityServer.Store
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            return await Task.Run(() => persistedGrants.Where(x => x.SubjectId == subjectId));
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            return await Task.Run(() => persistedGrants.Where(x => x.Key == key).FirstOrDefault());
        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            foreach (var item in await Task.Run(() => persistedGrants.Where(x => x.SubjectId == subjectId && x.ClientId == clientId)))
            {
                await Task.Run(() => persistedGrants.Remove(item));
            }
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            foreach (var item in await Task.Run(() => persistedGrants.Where(x => x.SubjectId == subjectId && x.ClientId == clientId && x.Type == type)))
            {
                await Task.Run(() => persistedGrants.Remove(item));
            }
        }

        public async Task RemoveAsync(string key)
        {
            var grant = await Task.Run(() => persistedGrants.Where(x => x.Key == key).FirstOrDefault());
            if(grant!= null) await Task.Run(() => persistedGrants.Remove(grant));
        }

        public async Task StoreAsync(PersistedGrant grant)
        {
            await Task.Run(() => persistedGrants.Add(grant));
        }

        private static List<PersistedGrant> persistedGrants = new List<PersistedGrant> ();
    }
}
