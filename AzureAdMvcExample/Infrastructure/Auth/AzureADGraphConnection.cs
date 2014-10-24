using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Humanizer;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AzureAdMvcExample.Infrastructure.Auth
{
    public interface IAzureADGraphConnection
    {
        IList<AppRoles> GetRolesForUser(ClaimsPrincipal userPrincipal);
    }

    public class AzureADGraphConnection : IAzureADGraphConnection
    {
        const string Resource = "https://graph.windows.net";
        public readonly Guid ClientRequestId = Guid.NewGuid();
        private readonly GraphConnection _graphConnection;

        public AzureADGraphConnection(string tenantName, string clientId, string clientSecret)
        {
            var authenticationContext = new AuthenticationContext("https://login.windows.net/" + tenantName, false);
            var clientCred = new ClientCredential(clientId, clientSecret);
            var token = authenticationContext.AcquireToken(Resource, clientCred).AccessToken;

            _graphConnection = new GraphConnection(token, ClientRequestId);
        }

        public IList<AppRoles> GetRolesForUser(ClaimsPrincipal userPrincipal)
        {
            return _graphConnection.GetMemberGroups(new User(userPrincipal.Identity.Name), true)
                .Select(groupId => _graphConnection.Get<Group>(groupId))
                .Where(g => g != null)
                .Select(g => g.DisplayName)
                .Select(g => ((AppRoles?)g.DehumanizeTo(typeof(AppRoles), OnNoMatch.ReturnsNull)))
                .Where(r => r.HasValue)
                .Select(r => r.Value)
                .ToList();
        }
    }
}