using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models.UserGroups;
using SunEngine.Commons.StoreModels;

namespace SunEngine.Stores
{
    public class UserGroupStore : IUserGroupStore
    {
        private readonly IDataBaseFactory dataBaseFactory;

        public UserGroupStore(IDataBaseFactory dataBaseFactory)
        {
            this.dataBaseFactory = dataBaseFactory;
        }

        protected IImmutableList<OperationKey> _allOperationKeys;

        public IImmutableList<OperationKey> AllOperationKeys
        {
            get
            {
                if (_allOperationKeys == null)
                {
                    InitializeOrReset();
                }

                return _allOperationKeys;
            }
        }


        protected ImmutableDictionary<string, UserGroup> _allGroups;

        public IImmutableDictionary<string, UserGroup> AllGroups
        {
            get
            {
                if (_allGroups == null)
                {
                    InitializeOrReset();
                }

                return _allGroups;
            }
        }

        public UserGroup GetUserGroup(string name)
        {
            if (_allGroups == null)
            {
                InitializeOrReset();
            }

            return AllGroups.ContainsKey(name) ? AllGroups[name] : null;
        }

        public void InitializeOrReset()
        {
            using (var db = dataBaseFactory.CreateDb())
            {
                var userGroups = db.UserGroups.Select(x => new UserGroupTmp(x)).ToDictionary(x => x.Id);

                _allOperationKeys = db.OperationKeys.Select(x => new OperationKey(x)).ToImmutableList();

                
                var categoryAccesses = db.CategoryAccess.Select(x => new CategoryAccessTmp(x))
                    .ToDictionary(x => x.Id);
                
                foreach (CategoryOperationAccessDB categoryOperationAccess in db.CategoryOperationAccess.ToList())
                {
                    categoryAccesses[categoryOperationAccess.CategoryAccessId].CategoryOperationAccesses
                        .Add(categoryOperationAccess.OperationKeyId, categoryOperationAccess.Access);
                }

                foreach (var categoryAccess in categoryAccesses.Values)
                {
                    userGroups[categoryAccess.UserGroupId].CategoryAccesses
                        .Add(categoryAccess);
                }

                _allGroups = userGroups.Values.ToImmutableDictionary(x => x.Name, x => new UserGroup(x));
            }
        }

        public async Task InitializeOrResetAsync()
        {
            throw new NotImplementedException();
        }
    }
}