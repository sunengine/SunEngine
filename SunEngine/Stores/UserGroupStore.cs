using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using SunEngine.DataBase;
using SunEngine.Models.Authorization;
using SunEngine.Stores.Models;

namespace SunEngine.Stores
{
    public interface IUserGroupStore : IMemoryStore
    {
        IImmutableList<OperationKeyStored> AllOperationKeys { get; }
        RoleStored GetUserGroup(string name);
        IImmutableDictionary<string, RoleStored> AllGroups { get; }
    }
    
    public class UserGroupStore : IUserGroupStore
    {
        private readonly IDataBaseFactory dataBaseFactory;

        public UserGroupStore(IDataBaseFactory dataBaseFactory)
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


        protected ImmutableDictionary<string, RoleStored> _allGroups;

        public IImmutableDictionary<string, RoleStored> AllGroups
        {
            get
            {
                if (_allGroups == null)
                {
                    Initialize();
                }

                return _allGroups;
            }
        }

        public RoleStored GetUserGroup(string name)
        {
            if (_allGroups == null)
            {
                Initialize();
            }

            return AllGroups.ContainsKey(name) ? AllGroups[name] : null;
        }

        public void Reset()
        {
            _allOperationKeys = null;
            _allGroups = null;
        }
        

        public void Initialize()
        {
            using (var db = dataBaseFactory.CreateDb())
            {
                var userGroups = db.Roles.Select(x => new RoleTmp(x)).ToDictionary(x => x.Id);

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
                    userGroups[categoryAccess.UserGroupId].CategoryAccesses
                        .Add(categoryAccess);
                }

                _allGroups = userGroups.Values.ToImmutableDictionary(x => x.Name, x => new RoleStored(x));
            }
        }

        public async Task InitializeAsync()
        {
            throw new NotImplementedException();
        }
    }
}