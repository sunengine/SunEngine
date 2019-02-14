using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using SunEngine.Stores;
using SunEngine.Stores.Models;

namespace SunEngine.Security.Authentication
{
    public class MyClaimsPrincipal : ClaimsPrincipal
    {
        public int UserId { get; }
        public long SessionId { get; }
        public string LongToken2 { get; }

        public IReadOnlyDictionary<string, RoleStored> Roles { get; }
        
        /// <summary>
        /// If only one group
        /// </summary>
        public RoleStored Role { get; }

        public MyClaimsPrincipal(ClaimsPrincipal user, IRolesCache rolesCache, long sessionId = 0, string longToken2 = null) : base(user)
        {
            this.SessionId = sessionId;
            this.LongToken2 = longToken2;
            
            if (Identity.IsAuthenticated)
            {
                UserId = int.Parse(this.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            
            Roles = GetUserRoles(rolesCache);
            if (Roles.Count == 1)
            {
                Role = Roles.Values.ElementAt(0);
            }
        }
        
        private IReadOnlyDictionary<string, RoleStored> GetUserRoles(IRolesCache rolesCache)
        {
            if (!Identity.IsAuthenticated)
            {
                return new Dictionary<string, RoleStored>
                {
                    [RoleStored.UserGroupUnregistered] = rolesCache.GetRole(RoleStored.UserGroupUnregistered)
                }.ToImmutableDictionary();
            }

            var roles = GetRolesNames();
            var allGroups = rolesCache.AllRoles;


            var dictionaryBuilder = ImmutableDictionary.CreateBuilder<string,RoleStored>();

            var registeredGroup = rolesCache.GetRole(RoleStored.UserGroupRegistered);
            dictionaryBuilder.Add(registeredGroup.Name, registeredGroup);
            foreach (var role in roles)
            {
                if (!allGroups.ContainsKey(role)) continue;

                var userGroup = allGroups[role];
                dictionaryBuilder.Add(userGroup.Name, userGroup);
            }

            return dictionaryBuilder.ToImmutable();
        }

        private IEnumerable<string> GetRolesNames()
        {
            return Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToImmutableList();
        }
    }
}