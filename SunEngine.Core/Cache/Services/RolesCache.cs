using System.Collections.Immutable;
using System.Linq;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models.Authorization;
using SunEngine.Core.Security;

namespace SunEngine.Core.Cache.Services
{
    public interface IRolesCache : ISunMemoryCache
    {
        IImmutableList<OperationKeyCached> AllOperationKeys { get; }
        RoleCached GetRole(string name);
        IImmutableDictionary<string, RoleCached> AllRoles { get; }
        RoleCached AdminRole { get; }
        string CheckAndSetRoles(string roles);
    }

    /// <summary>
    /// Quick access roles cache for authorization system (AuthorizationService)
    /// </summary>
    public class RolesCache : IRolesCache
    {
        protected readonly object lockObject = new object();
        protected readonly IDataBaseFactory dataBaseFactory;

        protected IImmutableList<OperationKeyCached> _allOperationKeys;
        protected ImmutableDictionary<string, RoleCached> _allRoles;
        protected RoleCached _adminRole;
        
        #region Getters

        public IImmutableList<OperationKeyCached> AllOperationKeys
        {
            get
            {
                lock (lockObject)
                    if (_allOperationKeys == null)
                        Initialize();

                return _allOperationKeys;
            }
        }

        public IImmutableDictionary<string, RoleCached> AllRoles
        {
            get
            {
                lock (lockObject)
                    if (_allRoles == null)
                        Initialize();

                return _allRoles;
            }
        }

        public RoleCached AdminRole
        {
            get
            {
                lock (lockObject)
                    if (_adminRole == null)
                        Initialize();

                return _adminRole;
            }
        }

        #endregion

        public RolesCache(IDataBaseFactory dataBaseFactory)
        {
            this.dataBaseFactory = dataBaseFactory;
        }

        public RoleCached GetRole(string name)
        {
            lock (lockObject)
                if (_allRoles == null)
                    Initialize();

            return AllRoles.TryGetValue(name, out var ret) ? ret : null;
        }
        
        public virtual string CheckAndSetRoles(string roles)
        {
            if (string.IsNullOrWhiteSpace(roles))
                return string.Join(',', RoleNames.Unregistered, RoleNames.Registered);

            var rolesNames = roles.Split(',').Select(x => x.Trim()).ToList()
                .Where(x => AllRoles.ContainsKey(x));
            return string.Join(',', rolesNames);
        }

        public void Initialize()
        {
            using (var db = dataBaseFactory.CreateDb())
            {
                var roles = db.Roles.Select(x => new RoleTmp(x)).ToDictionary(x => x.Id);

                _allOperationKeys = db.OperationKeys.Select(x => new OperationKeyCached(x)).ToImmutableList();


                var categoryAccesses = db.CategoryAccess.Select(x => new CategoryAccessTmp(x))
                    .ToDictionary(x => x.Id);

                foreach (CategoryOperationAccess categoryOperationAccess in db.CategoryOperationAccess.ToList())
                {
                    categoryAccesses[categoryOperationAccess.CategoryAccessId].CategoryOperationAccesses
                        .Add(categoryOperationAccess.OperationKeyId, categoryOperationAccess.Access);
                }

                foreach (var categoryAccess in categoryAccesses.Values)
                {
                    roles[categoryAccess.RoleId].CategoryAccesses
                        .Add(categoryAccess);
                }

                _allRoles = roles.Values.ToImmutableDictionary(x => x.Name, x => new RoleCached(x));
                _adminRole = _allRoles[RoleNames.Admin];
            }
        }
        
        public void Reset()
        {
            _allOperationKeys = null;
            _allRoles = null;
            _adminRole = null;
        }
    }
}
