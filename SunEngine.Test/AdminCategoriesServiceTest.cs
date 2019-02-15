using System.Linq;
using LinqToDB;
using SunEngine.Admin.Services;
using SunEngine.Utils.TextProcess;
using Xunit;

namespace SunEngine.Test
{
    public class AdminCategoriesServiceTest
    {
        
        #region GetCategory
        
        [Theory]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(7)]
        public async void ShouldGetCategoryById(int id)
        {
            var dbConnection = TestUtils.GetTestDataBaseConnection();
            var adminCatService = new CategoriesAdminService(dbConnection, new Sanitizer());
            var category = await adminCatService.GetCategoryAsync(id);
            
            Assert.Equal(id,category.Id);
        }

        [Theory]
        [InlineData(28)]
        [InlineData(0)]
        [InlineData(-1)]
        public async void ShouldGetNullCategoryById(int id)
        {
            var dbConnection = TestUtils.GetTestDataBaseConnection();
            var adminCatService = new CategoriesAdminService(dbConnection, new Sanitizer());
            var category = await adminCatService.GetCategoryAsync(id);

            Assert.Null(category);
        }

        [Theory]
        [InlineData("")]
        [InlineData("SomeCategory")]
        [InlineData("132135")]
        public async void ShouldGetNullCategoryByName(string name)
        {
            var dbConnection = TestUtils.GetTestDataBaseConnection();
            var adminCatService = new CategoriesAdminService(dbConnection, new Sanitizer());
            var category = await adminCatService.GetCategoryAsync(name);

            Assert.Null(category);
        }

        [Theory]
        [InlineData("Forum")]
        [InlineData("Thread2")]
        [InlineData("Forum2L")]
        public async void ShouldGetCategoryByName(string name)
        {
            var dbConnection = TestUtils.GetTestDataBaseConnection();
            var adminCatService = new CategoriesAdminService(dbConnection, new Sanitizer());
            var category = await adminCatService.GetCategoryAsync(name);

            Assert.Equal(name,category.Name);
        }

        #endregion

        #region AddCategory

        [Fact]
        public async void ShouldAddNewCategory()
        {
            var dbConnection = TestUtils.GetTestDataBaseConnection();
            var adminCatService = new CategoriesAdminService(dbConnection, new Sanitizer());
            var defaultCount = dbConnection.Categories.Count();
            await adminCatService.AddCategoryAsync(TestUtils.TestableValidCategory);
            var newCount = dbConnection.Categories.Count();
            Assert.Equal(defaultCount+1,newCount);
            dbConnection.Categories.Delete(item => item.Name == TestUtils.TestableValidCategory.Name);
            Assert.Equal(defaultCount,dbConnection.Categories.Count());
        }

        [Fact]
        public void ShouldThrowExceptionByAddingInMaterialContainer()
        {
            var dbConnection = TestUtils.GetTestDataBaseConnection();
            var adminCatService = new CategoriesAdminService(dbConnection, new Sanitizer());
            var category = TestUtils.TestableValidCategory;
            category.ParentId = 2;
            var ex = Record.ExceptionAsync(async () => await adminCatService.AddCategoryAsync(category));
            Assert.NotNull(ex);
            Assert.Equal(ex.Result.Message, "Can not add in MaterialContainer category type");
        }

        [Fact]
        public void ShouldThrowExceptionIfParentIdIsNull()
        {
            var dbConnection = TestUtils.GetTestDataBaseConnection();
            var adminCatService = new CategoriesAdminService(dbConnection, new Sanitizer());
            var category = TestUtils.TestableValidCategory;
            category.ParentId = null;
            //TODO: test throw exception if parentid invalid
            var ex = Record.ExceptionAsync(async () => await adminCatService.AddCategoryAsync(category));
            Assert.NotNull(ex);
            //TODO: Check type of Exception
            Assert.Equal(ex.Result.Message, "Exception message text if exception type is System.Exception");
        }

        #endregion

        #region EditCategory

        [Fact]
        public async void ShouldEditCategoryWithSuccess()
        {
            var category = TestUtils.TestableValidCategory;
            var dbConnection = TestUtils.GetTestDataBaseConnection();
            var adminCategoryService = new CategoriesAdminService(dbConnection, new Sanitizer());
            
            await adminCategoryService.AddCategoryAsync(category);
            var editCategory = dbConnection.Categories.FirstOrDefault(cat => cat.Name == category.Name);
            Assert.NotNull(editCategory);
            editCategory.Title = "ТестИзм";
            await adminCategoryService.EditCategoryAsync(editCategory);
            Assert.Equal("ТестИзм", dbConnection.Categories.FirstOrDefault(cat => cat.Name == category.Name).Title);

            dbConnection.Categories.Delete(cat => cat.Name == category.Name);
        }

        #endregion
    }
}
