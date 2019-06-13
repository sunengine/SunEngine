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
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Authorization;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;

namespace SunEngine.Admin.Services
{
    public class RolesPermissionsAdminService : DbService
    {
        private const string RolesSchemaFileName = "Roles.schema.json";
        private readonly string RolesSchemaPath;


        public RolesPermissionsAdminService(
            DataBaseConnection db,
            IHostingEnvironment env) : base(db)
        {
            RolesSchemaPath = Path.Combine(env.ContentRootPath, "Resources", RolesSchemaFileName);
        }

        public async Task<string> GetRolesJsonAsync()
        {
            var roles = await db.Roles
                .LoadWith(x => x.CategoryAccesses.First().CategoryOperationAccesses.First().OperationKey)
                .ToListAsync();

            var categories = await db.Categories.ToDictionaryAsync(x => x.Id, x => x);

            var jObject = roles.Select(x =>
                    new
                    {
                        Name = x.Name,
                        Role = new
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
                .ToDictionary(x => x.Name, x => x.Role);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(jObject, Formatting.Indented, serializerSettings);
        }

        public async Task LoadRolesFromJsonAsync(string json)
        {
            IDictionary<string, Category> categories =
                await db.Categories.ToDictionaryAsync(x => x.Name);
            IDictionary<string, OperationKey> operationKeys =
                await db.OperationKeys.ToDictionaryAsync(x => x.Name);

            JsonSchema schema = await JsonSchema.FromFileAsync(RolesSchemaPath);

            RolesFromJsonLoader rolesFromJsonLoader = new RolesFromJsonLoader(categories, operationKeys, schema);


            rolesFromJsonLoader.Seed(json);

            try
            {
                db.BeginTransaction();
                await UpdateRoles(rolesFromJsonLoader.roles);
                await ClearAccessesAsync();
                await CopyToDb(rolesFromJsonLoader);
                db.CommitTransaction();
            }
            catch (Exception e)
            {
                db.RollbackTransaction();
                throw e;
            }
        }

        private async Task UpdateRoles(List<Role> rolesNew)
        {
            var roles = await db.Roles.ToListAsync();

            var toDelete = roles
                .Where(x =>
                    !rolesNew.Any(y => string.Equals(x.NormalizedName, y.NormalizedName))
                )
                .ToList();

            var toInsert = rolesNew
                .Where(x =>
                    !roles.Any(y => string.Equals(x.NormalizedName, y.NormalizedName))
                )
                .ToList();

            var toUpdate = rolesNew.Where(x => roles.Any(y => string.Equals(x.NormalizedName, y.NormalizedName)))
                .ToList();

            List<Role> errorRoles = new List<Role>();

            foreach (var role in toDelete)
            {
                if (await db.UserRoles.AnyAsync(x => x.RoleId == role.Id))
                    errorRoles.Add(role);
            }

            if (errorRoles.Count > 0)
            {
                var errorNames = string.Join(", ", errorRoles.Select(y => "'" + y.Name + "'"));
                throw new SunViewException(
                    new ErrorView("CanNotDeleteRolesItHasUsers",
                        "This roles can not be deleted because it has users, remove them first.", ErrorType.Soft,
                        errorNames));
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

        private async Task CopyToDb(RolesFromJsonLoader fromJsonLoader)
        {
            BulkCopyOptions options = new BulkCopyOptions
            {
                CheckConstraints = false,
                BulkCopyType = BulkCopyType.Default,
                KeepIdentity = true
            };

            db.BulkCopy(options, fromJsonLoader.categoryAccesses);
            db.UpdateSequence("CategoryAccesses", "Id");
            db.BulkCopy(options, fromJsonLoader.categoryOperationAccesses);
        }
    }
}
