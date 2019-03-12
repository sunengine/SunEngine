using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using SunEngine.Cache.CacheModels;
using SunEngine.DataBase;
using SunEngine.Models.Authorization;

namespace SunEngine.Cache
{
    public interface IRolesCache : IMyMemoryCache
    {
        IImmutableList<OperationKeyCached> AllOperationKeys { get; }
        RoleCached GetRole(string name);
        IImmutableDictionary<string, RoleCached> AllRoles { get; }
    }
    
    public class RolesCache : IRolesCache
    {
        private readonly IDataBaseFactory dataBaseFactory;

        public RolesCache(IDataBaseFactory dataBaseFactory)
        {
            this.dataBaseFactory = dataBaseFactory;
        }

        protected IImmutableList<OperationKeyCached> _allOperationKeys;

        public IImmutableList<OperationKeyCached> AllOperationKeys
        {
            get
            {
                if (_allOperationKeys == null)
                {
                    Initialize();
                }

                return _allOperationKeys;
            }
        }


        protected ImmutableDictionary<string, RoleCached> _allRoles;

        public IImmutableDictionary<string, RoleCached> AllRoles
        {
            get
            {
                if (_allRoles == null)
                {
                    Initialize();
                }

                return _allRoles;
            }
        }

        public RoleCached GetRole(string name)
        {
            if (_allRoles == null)
            {
                Initialize();
            }

            return AllRoles.ContainsKey(name) ? AllRoles[name] : null;
        }

        public void Reset()
        {
            _allOperationKeys = null;
            _allRoles = null;
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
            }
        }

        public async Task InitializeAsync()
        {
            throw new NotImplementedException();
        }
    }
}