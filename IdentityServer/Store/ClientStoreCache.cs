using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace IdentityServer.Store
{
    public class ClientStoreCache : IClientStore
    {
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            return await Task.Run(() => Config.GetClients().Where(x => x.ClientId == clientId).FirstOrDefault());
        }
    }
}
