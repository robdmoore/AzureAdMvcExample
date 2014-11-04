using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Humanizer;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AzureAdMvcExample.Infrastructure.Auth
{
    public interface IAzureADGraphConnection
    {
        IList<AppRoles> GetRolesForUser(ClaimsPrincipal userPrincipal);
        IList<User> SearchUsers(string query);
        User GetUser(Guid id);
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

        public IList<User> SearchUsers(string query)
        {
            var displayNameFilter = ExpressionHelper.CreateStartsWithExpression(typeof(User), GraphProperty.DisplayName, query);
            var surnameFilter = ExpressionHelper.CreateStartsWithExpression(typeof(User), GraphProperty.Surname, query);
            var usersByDisplayName = _graphConnection
                .List<User>(null, new FilterGenerator { QueryFilter = displayNameFilter })
                .Results;
            var usersBySurname = _graphConnection
                .List<User>(null, new FilterGenerator { QueryFilter = surnameFilter })
                .Results;

            return usersByDisplayName.Union(usersBySurname, new UserComparer()).ToArray();
        }

        public User GetUser(Guid id)
        {
            try
            {
                return _graphConnection.Get<User>(id.ToString());
            }
            catch (ObjectNotFoundException)
            {
                return null;
            }
        }

        class UserComparer : IEqualityComparer<User>
        {
            public bool Equals(User x, User y)
            {
                return x.ObjectId == y.ObjectId;
            }

            public int GetHashCode(User obj)
            {
                return obj.ObjectId.GetHashCode();
            }
        }
    }
}