using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using NJsonSchema;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Models.UserGroups;
using SunEngine.Commons.Utils;
using SunEngine.EntityServices;

namespace SunEngine.Admin.Services
{
    public class GroupsAdminService : DbService
    {
        private const string UserGroupsSchemaFileName = "UserGroups.schema.json";
        private readonly string UserGroupSchemaPath;


        public GroupsAdminService(
            DataBaseConnection db,
            IHostingEnvironment env) : base(db)
        {
            UserGroupSchemaPath = Path.Combine(env.ContentRootPath,"Resources", UserGroupsSchemaFileName);
        }

        public async Task<string> GetGroupsJsonAsync()
        {
            var groups = await db.UserGroups
                .LoadWith(x => x.CategoryAccesses.First().CategoryOperationAccesses.First().OperationKeyDb)
                .ToListAsync();

            var categories = await db.Categories.ToDictionaryAsync(x => x.Id, x => x);

            var jobject = groups.Select(x =>
                    new
                    {
                        Name = x.Name,
                        Group = new
                        {
                            Title = x.Title,
                            IsSuper = x.IsSuper ? (object) true : null,
                            Categories = x.CategoryAccesses.Count > 0
                                ? x.CategoryAccesses.Select(y =>
                                    new
                                    {
                                        Category = categories[y.CategoryId].Name,
                                        OperationKeys = y.CategoryOperationAccesses.Count > 0 ?
                                            y.CategoryOperationAccesses.ToDictionary(z => z.OperationKeyDb.Name,
                                                z => z.Access) : null
                                    }).ToArray()
                                : null
                        }
                    })
                .ToDictionary(x => x.Name, x => x.Group);

            JsonSerializerSettings jo = new JsonSerializerSettings();
            jo.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(jobject, Formatting.Indented, jo);
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
                await UpdateUserGroups(loader.userGroups);
                await ClearAccessesAsync();
                await CopyToDb(loader, userToGroups);
                db.CommitTransaction();
            }
            catch (Exception e)
            {
                db.RollbackTransaction();
                throw e;
            }
        }

        private async Task UpdateUserGroups(List<UserGroupDB> groupsNew)
        {
            var groups = await db.UserGroups.ToListAsync();

            var toDelete = groups
                .Where(x =>
                    !groupsNew.Any(y => string.Equals(x.NormalizedName, y.NormalizedName))
                )
                .ToList();

            var toInsert = groupsNew
                .Where(x =>
                    !groups.Any(y => string.Equals(x.NormalizedName, y.NormalizedName))
                )
                .ToList();

            var toUpdate = groupsNew.Where(x => groups.Any(y => string.Equals(x.NormalizedName, y.NormalizedName)))
                .ToList();

            List<UserGroupDB> errorGroups = new List<UserGroupDB>();

            foreach (var group in toDelete)
            {
                db.UserToGroups.AnyAsync(x => x.RoleId == group.Id);
                errorGroups.Add(group);
            }

            if (errorGroups.Count > 0)
            {
                var errorNames = string.Join(", ", errorGroups.Select(y => "'" + y.Name + "'"));
                throw new Exception(
                    "В этих группах есть пользователи, очистите их перед удалением: " + errorNames);
            }

            toDelete.ForEach(async x => await db.DeleteAsync(x));
            toInsert.ForEach(async x => await db.InsertAsync(x));
            toUpdate.ForEach(async x => await db.UpdateAsync(x));
        }

        private async Task ClearAccessesAsync()
        {
            await db.CategoryOperationAccess.DeleteAsync();
            await db.CategoryAccess.DeleteAsync();
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
                BulkCopyType = BulkCopyType.Default,
                KeepIdentity = true
            };

            db.BulkCopy(options, loader.categoryAccesses);
            db.BulkCopy(options, loader.categoryOperationAccesses);
        }

        private class UserToGroupTmp
        {
            public int userId;
            public string roleName;
        }
    }
}