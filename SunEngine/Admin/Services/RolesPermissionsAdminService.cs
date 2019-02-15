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
using SunEngine.DataBase;
using SunEngine.Models;
using SunEngine.Models.Authorization;
using SunEngine.Services;
using SunEngine.Utils;

namespace SunEngine.Admin.Services
{
    public class RolesPermissionsAdminService : DbService
    {
        private const string UserGroupsSchemaFileName = "UserGroups.schema.json";
        private readonly string UserGroupSchemaPath;


        public RolesPermissionsAdminService(
            DataBaseConnection db,
            IHostingEnvironment env) : base(db)
        {
            UserGroupSchemaPath = Path.Combine(env.ContentRootPath, "Resources", UserGroupsSchemaFileName);
        }

        public async Task<string> GetGroupsJsonAsync()
        {
            var groups = await db.Roles
                .LoadWith(x => x.CategoryAccesses.First().CategoryOperationAccesses.First().OperationKey)
                .ToListAsync();

            var categories = await db.Categories.ToDictionaryAsync(x => x.Id, x => x);

            var jObject = groups.Select(x =>
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
                                        OperationKeys = y.CategoryOperationAccesses.Count > 0
                                            ? y.CategoryOperationAccesses.ToDictionary(z => z.OperationKey.Name,
                                                z => z.Access)
                                            : null
                                    }).ToArray()
                                : null
                        }
                    })
                .ToDictionary(x => x.Name, x => x.Group);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(jObject, Formatting.Indented, serializerSettings);
        }

        public async Task LoadUserGroupsFromJsonAsync(string json)
        {
            IDictionary<string, Category> categories =
                await db.Categories.ToDictionaryAsync(x => x.Name);
            IDictionary<string, OperationKey> operationKeys =
                await db.OperationKeys.ToDictionaryAsync(x => x.Name);


            JsonSchema4 schema = await JsonSchema4.FromFileAsync(UserGroupSchemaPath);

            RolesLoaderFromJson loader = new RolesLoaderFromJson(categories, operationKeys, schema);


            loader.Seed(json);

            try
            {
                db.BeginTransaction();
                await UpdateUserGroups(loader.roles);
                await ClearAccessesAsync();
                await CopyToDb(loader);
                db.CommitTransaction();
            }
            catch (Exception e)
            {
                db.RollbackTransaction();
                throw e;
            }
        }

        private async Task UpdateUserGroups(List<Role> groupsNew)
        {
            var groups = await db.Roles.ToListAsync();

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

            List<Role> errorGroups = new List<Role>();

            foreach (var group in toDelete)
            {
                if(!await db.UserRoles.AnyAsync(x => x.RoleId == group.Id))
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

        private async Task CopyToDb(RolesLoaderFromJson loader)
        {
            BulkCopyOptions options = new BulkCopyOptions
            {
                CheckConstraints = false,
                BulkCopyType = BulkCopyType.Default,
                KeepIdentity = true
            };

            db.BulkCopy(options, loader.categoryAccesses);
            db.UpdateSequence("CategoryAccesses","Id");
            db.BulkCopy(options, loader.categoryOperationAccesses);
        }

        private class UserToGroupTmp
        {
            public int UserId;
            public string RoleName;
        }
    }
}