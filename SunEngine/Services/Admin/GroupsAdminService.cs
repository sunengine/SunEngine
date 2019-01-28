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

        public async Task<string> GetGroupsJsonAsync()
        {
            var groups = await db.UserGroups.LoadWith(x => x.CategoryAccesses.First().CategoryOperationAccesses.First().OperationKeyDb)
                .ToListAsync();

            var categories = await db.Categories.ToDictionaryAsync(x => x.Id, x => x);
            
            var jobject = groups.Select(x =>
                    new
                    {
                        Name = x.Name,
                        Group = new
                        {
                            Id = x.Id,
                            Title = x.Title,
                            IsSuper = x.IsSuper ? (object)true : null,
                            Categories = x.CategoryAccesses.Count > 0 ? x.CategoryAccesses.Select(y =>
                                new
                                {
                                    Category = categories[y.CategoryId].Name, 
                                    OperationKeys =
                                        y.CategoryOperationAccesses?.ToDictionary(z => z.OperationKeyDb.Name,
                                            z => z.Access)
                                }).ToArray() : null
                        }
                    })
                .ToDictionary(x => x.Name, x => x.Group);

            JsonSerializerSettings jo = new JsonSerializerSettings();
            jo.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(jobject,Formatting.Indented,jo);
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
            catch (Exception e)
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
                BulkCopyType = BulkCopyType.Default,
                KeepIdentity = true
            };

            // TODO Copy only New Groups, если так сделать не понадобится стирать и сохранять таблицу ПользователиГруппы

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

        private class UserToGroupTmp
        {
            public int userId;
            public string roleName;
        }
    }
}