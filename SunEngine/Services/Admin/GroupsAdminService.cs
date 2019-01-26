using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Identity;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Models.UserGroups;
using SunEngine.EntityServices;

namespace SunEngine.Services.Admin
{
    public class GroupsAdminService : DbService
    {
        public GroupsAdminService(DataBaseConnection db) : base(db)
        {
        }

        public async Task<string> GetGroupsJson()
        {
            return "";
        }

        public async Task LoadUserGroupsFromJsonAsync(string json)
        {
            IDictionary<string, Category> categories =
                await db.Categories.ToDictionaryAsync(x => x.Name);
            IDictionary<string, OperationKeyDB> operationKeys =
                await db.OperationKeys.ToDictionaryAsync(x => x.Name);

            UserGroupsLoaderFromJson loader = new UserGroupsLoaderFromJson(categories, operationKeys);
            loader.Seed(json);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                List<UserToGroupTmp> userToGroups = await SaveUserToGroupsAsync();
                await ClearGroupsAsync();
                await CopyToDb(loader,userToGroups);
                SaveToFile(json);
                transaction.Complete();
            }
        }

        void SaveToFile(string json)
        {
            File.WriteAllText(Path.GetFullPath("userGroups.json"), json);
        }

        async Task ClearGroupsAsync()
        {
            await db.UserToGroups.DeleteAsync();
            await db.CategoryOperationAccess.DeleteAsync();
            await db.CategoryAccess.DeleteAsync();
            await db.UserGroups.DeleteAsync();
        }

        Task<List<UserToGroupTmp>> SaveUserToGroupsAsync()
        {
            return db.UserToGroups.Select(x=>new UserToGroupTmp {userId = x.UserId,roleName = x.UserGroup.Name}).ToListAsync();
        }

        async Task CopyToDb(UserGroupsLoaderFromJson loader, List<UserToGroupTmp> userToGroups)
        {
            BulkCopyOptions options = new BulkCopyOptions
            {
                CheckConstraints = false,
                //BulkCopyType = BulkCopyType.Default,
                KeepIdentity = true
            };

            db.BulkCopy(options, loader.userGroups);
            db.BulkCopy(options, loader.categoryAccesses);
            db.BulkCopy(options, loader.categoryOperationAccesses);

            List<UserToGroup> userToGroupsNew = userToGroups.Select(x => new UserToGroup
            {
                UserId = x.userId,
                RoleId = loader.userGroups.FirstOrDefault(y => y.Name == x.roleName).Id
            }).ToList();
            
            db.BulkCopy(options, userToGroupsNew);
        }

        public class UserToGroupTmp
        {
            public int userId;
            public string roleName;
        }
    }
}