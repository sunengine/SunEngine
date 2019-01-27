using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.AspNetCore.Hosting;
using NJsonSchema;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Models.UserGroups;
using SunEngine.Commons.Utils;
using SunEngine.EntityServices;

namespace SunEngine.Services.Admin
{
    public class GroupsAdminService : DbService
    {
        private const string UserGroupsFileName = "UserGroups.json";
        private const string UserGroupsSchemaFileName = "UserGroups.schema.json";


        private readonly string UserGroupsConfigPath;
        private readonly string UserGroupSchemaPath;


        public GroupsAdminService(
            DataBaseConnection db,
            IHostingEnvironment env) : base(db)
        {
            UserGroupsConfigPath = Path.Combine(env.ContentRootPath, UserGroupsFileName);
            UserGroupSchemaPath = Path.Combine(env.ContentRootPath, UserGroupsSchemaFileName);
        }

        public string GetGroupsJson()
        {
            return File.ReadAllText(UserGroupsConfigPath);
        }

        public async Task LoadUserGroupsFromJsonAsync(string json)
        {
            IDictionary<string, Category> categories =
                await db.Categories.ToDictionaryAsync(x => x.Name);
            IDictionary<string, OperationKeyDB> operationKeys =
                await db.OperationKeys.ToDictionaryAsync(x => x.Name);


            JsonSchema4 schema = await JsonSchema4.FromFileAsync(UserGroupSchemaPath);

            UserGroupsLoaderFromJson loader = new UserGroupsLoaderFromJson(categories, operationKeys, schema);


            loader.Seed(json);

            try
            {
                db.BeginTransaction();
                List<UserToGroupTmp> userToGroups = await SaveUserToGroupsAsync();
                await ClearGroupsAsync();
                await CopyToDb(loader, userToGroups);
                SaveToFile(json);
                db.CommitTransaction();
            }
            catch(Exception e)
            {
                db.RollbackTransaction();
                throw e;
            }
        }

        private void SaveToFile(string json)
        {
            File.WriteAllText(UserGroupsConfigPath, json);
        }

        private async Task ClearGroupsAsync()
        {
            await db.UserToGroups.DeleteAsync();
            await db.CategoryOperationAccess.DeleteAsync();
            await db.CategoryAccess.DeleteAsync();
            await db.UserGroups.DeleteAsync();
        }

        private Task<List<UserToGroupTmp>> SaveUserToGroupsAsync()
        {
            return db.UserToGroups.Select(x => new UserToGroupTmp {userId = x.UserId, roleName = x.UserGroup.Name})
                .ToListAsync();
        }

        private async Task CopyToDb(UserGroupsLoaderFromJson loader, List<UserToGroupTmp> userToGroups)
        {
            BulkCopyOptions options = new BulkCopyOptions
            {
                CheckConstraints = false,
                //BulkCopyType = BulkCopyType.Default,
                KeepIdentity = true
            };

            db.BulkCopy(options, loader.userGroups);

            throw new Exception("test");

            db.BulkCopy(options, loader.categoryAccesses);
            db.BulkCopy(options, loader.categoryOperationAccesses);

            List<UserToGroup> userToGroupsNew = userToGroups.Select(x => new UserToGroup
            {
                UserId = x.userId,
                RoleId = loader.userGroups.FirstOrDefault(y => y.Name == x.roleName).Id
            }).ToList();

            db.BulkCopy(options, userToGroupsNew);
        }

        private class UserToGroupTmp
        {
            public int userId;
            public string roleName;
        }
    }
}