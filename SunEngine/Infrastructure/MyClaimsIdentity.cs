using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using SunEngine.Commons.StoreModels;
using SunEngine.Stores;

namespace SunEngine.Infrastructure
{
    public class MyClaimsPrincipal : ClaimsPrincipal
    {
        public int UserId { get; } = 0;

        public IReadOnlyDictionary<string, UserGroup> UserGroups { get; }
        
        /// <summary>
        /// If only one group
        /// </summary>
        public UserGroup UserGroup { get; }

        public MyClaimsPrincipal(ClaimsPrincipal user, IUserGroupStore userGroupStore) : base(user)
        {
            if (this.Identity.IsAuthenticated)
            {
                UserId = int.Parse(this.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            
            UserGroups = (IReadOnlyDictionary<string, UserGroup>) GetUserGroups(userGroupStore);
            if (UserGroups.Count == 1)
            {
                UserGroup = UserGroups.Values.ElementAt(0);
            }
        }

        private IReadOnlyDictionary<string, UserGroup> GetUserGroups(IUserGroupStore userGroupStore)
        {
            if (!Identity.IsAuthenticated)
            {
                return new Dictionary<string, UserGroup>()
                {
                    [UserGroup.UserGroupUnregistered] = userGroupStore.GetUserGroup(UserGroup.UserGroupUnregistered)
                };
            }

            var roles = GetRolesNames();
            var allGroups = userGroupStore.AllGroups;

            //Dictionary<string, UserGroup> userGroups = new Dictionary<string, UserGroup>(roles.Count + 1);

            var dictionaryBuilder = ImmutableDictionary.CreateBuilder<string,UserGroup>();

            var registeredGroup = userGroupStore.GetUserGroup(UserGroup.UserGroupRegistered);
            dictionaryBuilder.Add(registeredGroup.Name, registeredGroup);
            foreach (var role in roles)
            {
                if (!allGroups.ContainsKey(role)) continue;

                var userGroup = allGroups[role];
                dictionaryBuilder.Add(userGroup.Name, userGroup);
            }

            return dictionaryBuilder.ToImmutable();
        }

        public IReadOnlyList<string> GetRolesNames()
        {
            return Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToImmutableList();
        }
    }

    public class MyAuthUserFilter : IAuthorizationFilter
    {
        private readonly IUserGroupStore _userGroupStore;

        public MyAuthUserFilter(IUserGroupStore userGroupStore)
        {
            _userGroupStore = userGroupStore;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            context.HttpContext.User = new MyClaimsPrincipal(context.HttpContext.User, _userGroupStore);
        }
    }
}