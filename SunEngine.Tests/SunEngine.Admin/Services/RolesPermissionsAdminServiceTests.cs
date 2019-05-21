using System.IO;
using System.Linq;
using System.Text;
using LinqToDB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using SunEngine.Admin.Services;
using SunEngine.Core.DataBase;
using Xunit;

namespace SunEngine.Tests.SunEngine.Admin.Services
{
    public class RolesPermissionsAdminServiceTests
    {
        private readonly IHostingEnvironment environment;
        private readonly DataBaseConnection dbConnection;

        public RolesPermissionsAdminServiceTests()
        {
            dbConnection = DefaultInit.GetTestDataBaseConnection();
            environment = new HostingEnvironment {ContentRootPath = GetUserGroupSchemaPath()};
        }
        

        private string GetUserGroupSchemaPath() => Path.Combine(GetParentDirectoryAtLevel(Directory.GetCurrentDirectory(), 4), "SunEngine.Cli");
        private string GetUserGroupJsonPath() => Path.Combine(GetParentDirectoryAtLevel(Directory.GetCurrentDirectory(), 4), "SunEngine.Cli", "Config", "UserGroups.json");

        private string GetParentDirectoryAtLevel(string path, int level)
        {
            while (level != 0)
            {
                var newPath = Directory.GetParent(path).FullName;
                return GetParentDirectoryAtLevel(newPath, --level);
            }

            return path;
        }

        [Fact]
        public async void ShouldGetGroupJson()
        {
            var rolesService = new RolesPermissionsAdminService(dbConnection, environment);

            Assert.NotEqual(string.Empty, await rolesService.GetGroupsJsonAsync());
        }

        
        [Fact]
        public async void ShouldLoadUserGroupsFromJson()
        {
            string json = File.ReadAllText(GetUserGroupJsonPath(), Encoding.Default);
            var rolesService = new RolesPermissionsAdminService(dbConnection, environment);

            using (var transaction = dbConnection.BeginTransaction())
            {
                await dbConnection.Roles.Set(x => x.Name, "test").UpdateAsync();
                var before = dbConnection.Roles.Select(x => x.Name).ToList();

                await rolesService.LoadUserGroupsFromJsonAsync(json);

                var after = dbConnection.Roles.Select(x => x.Name).ToList();
                transaction.Rollback();

                Assert.NotEqual(before, after);
            }  
        }
    }
}