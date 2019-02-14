using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using SunEngine.DataBase;
using SunEngine.Models.Authorization;
using SunEngine.Stores.Models;

namespace SunEngine.Stores
{
    public interface IRolesCache : IMemoryCache
    {
        IImmutableList<OperationKeyStored> AllOperationKeys { get; }
        RoleStored GetRole(string name);
        IImmutableDictionary<string, RoleStored> AllRoles { get; }
    }
    
    public class RolesCache : IRolesCache
    {
        private readonly IDataBaseFactory dataBaseFactory;

        public RolesCache(IDataBaseFactory dataBaseFactory)
        {
            this.dataBaseFactory = dataBaseFactory;
        }

        protected IImmutableList<OperationKeyStored> _allOperationKeys;

        public IImmutableList<OperationKeyStored> AllOperationKeys
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


        protected ImmutableDictionary<string, RoleStored> _allRoles;

        public IImmutableDictionary<string, RoleStored> AllRoles
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

        public RoleStored GetRole(string name)
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

                _allOperationKeys = db.OperationKeys.Select(x => new OperationKeyStored(x)).ToImmutableList();

                
                var categoryAccesses = db.CategoryAccess.Select(x => new CategoryAccessTmp(x))
                    .ToDictionary(x => x.Id);
                
                foreach (CategoryOperationAccess categoryOperationAccess in db.CategoryOperationAccess.ToList())
                {
                    categoryAccesses[categoryOperationAccess.CategoryAccessId].CategoryOperationAccesses
                        .Add(categoryOperationAccess.OperationKeyId, categoryOperationAccess.Access);
                }

                foreach (var categoryAccess in categoryAccesses.Values)
                {
                    roles[categoryAccess.UserGroupId].CategoryAccesses
                        .Add(categoryAccess);
                }

                _allRoles = roles.Values.ToImmutableDictionary(x => x.Name, x => new RoleStored(x));
            }
        }

        public async Task InitializeAsync()
        {
            throw new NotImplementedException();
        }
    }
}