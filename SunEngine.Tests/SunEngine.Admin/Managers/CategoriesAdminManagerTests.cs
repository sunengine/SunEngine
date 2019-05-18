using System;
using System.Linq;
using SunEngine.Admin.Managers;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;
using SunEngine.Core.Utils.TextProcess;
using Xunit;

namespace SunEngine.Tests.SunEngine.Admin.Managers
{
    public class CategoriesAdminManagerTests
    {
        private readonly CategoriesAdminManager catAdminManager;
        private DataBaseConnection dbConnection;

        public CategoriesAdminManagerTests()
        {
            catAdminManager = DefaultCatAdminManager();
        }

        private CategoriesAdminManager DefaultCatAdminManager()
        {
            dbConnection = DefaultInit.GetTestDataBaseConnection();
            var dbFactory = DefaultInit.GetTestDataBaseFactory();
            var catCache = new CategoriesCache(dbFactory);

            return new CategoriesAdminManager(dbConnection, catCache, new Sanitizer());
        }

        private Category DefaultCategory => new Category { Name = "TestCategory", ParentId = 1, Title= "TestCategoryTitle"};

        #region Test CreateCategoryAsync

        [Fact]
        public async void ShouldCreateCategory()
        {
            var category = DefaultCategory;

            using (var transaction = dbConnection.BeginTransaction())
            {
                int countBefore = dbConnection.Categories.Count();
                await catAdminManager.CreateCategoryAsync(category);
                int countAfter = dbConnection.Categories.Count();
                transaction.Rollback();
                Assert.NotEqual(countAfter, countBefore);
            }
        }

        [Fact]
        public async void ShouldThrowExceptionIfCategoryIsNullWhenCreate()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await catAdminManager.CreateCategoryAsync(null));
        }

        [Fact]
        public async void ShouldThrowExceptionIfParentIdIsNull()
        {
            var category = new Category {Name = "Test", Title ="test"};
            await Assert.ThrowsAsync<ParentCategoryNotFoundByIdException>(async () =>
                await catAdminManager.CreateCategoryAsync(category));
        }

        [Fact]
        public async void ShouldThrowExceptionIfCategoryNameIsNull()
        {
            var category = new Category { Title = "test" };
            await Assert.ThrowsAsync<InvalidModelException>(async () =>
                await catAdminManager.CreateCategoryAsync(category));
        }

        [Fact]
        public async void ShouldThrowExceptionIfCategoryTitleIsNull()
        {
            var category = new Category { Name = "Test"};
            await Assert.ThrowsAsync<InvalidModelException>(async () =>
                await catAdminManager.CreateCategoryAsync(category));
        }

        #endregion

        #region Test UpdateCategoryAsync

        [Fact]
        public async void ShouldUpdateCategory()
        {
            var category = DefaultCategory;

            using (var transaction = dbConnection.BeginTransaction())
            {
                await catAdminManager.CreateCategoryAsync(category);
                category = dbConnection.Categories.FirstOrDefault(x => x.Name == category.Name);
                Assert.NotNull(category);
                category.Name = "UpdatedTestCategory";
                await catAdminManager.UpdateCategoryAsync(category);
                var resultCategory = dbConnection.Categories.FirstOrDefault(x => x.Name == "UpdatedTestCategory");
                Assert.NotNull(resultCategory);
                transaction.Rollback();
            }
        }

        [Fact]
        public async void ShouldThrowExceptionWhenUpdateCategoryWithNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await catAdminManager.UpdateCategoryAsync(null));
        }

        [Fact]
        public void ShouldThrowExceptionWhenUpdateCategoryWithWrongId()
        {
            var category = DefaultCategory;
            category.Id = -1;
            var ex = Record.ExceptionAsync(async () => await catAdminManager.UpdateCategoryAsync(category));
            Assert.Equal($"No category with {category.Id} id", ex.Result.Message);
        }

        [Fact]
        public async void ShouldThrowExceptionWhenUpdateCategoryWithWrongParentId()
        {
            var category = DefaultCategory;

            using (var transaction = dbConnection.BeginTransaction())
            {
                await catAdminManager.CreateCategoryAsync(category);
                category = dbConnection.Categories.FirstOrDefault(x => x.Name == category.Name);
                Assert.NotNull(category);
                category.ParentId = -1;
                await Assert.ThrowsAsync<ParentCategoryNotFoundByIdException>(async () =>
                    await catAdminManager.UpdateCategoryAsync(category));
                transaction.Rollback();
            }
        }


        #endregion

        #region Test CategoryUp

        [Fact]
        public async void ShouldMoveCategoryUp()
        {
            var category = DefaultCategory;
            
            using (var transaction = dbConnection.BeginTransaction())
            {
                await catAdminManager.CreateCategoryAsync(category);
                var result = await catAdminManager.CategoryUp("TestCategory");
                transaction.Rollback();
                Assert.Equal(ServiceResult.OkResult().Error, result.Error);
                Assert.Equal(ServiceResult.OkResult().Failed, result.Failed);
                Assert.Equal(ServiceResult.OkResult().Succeeded, result.Succeeded);
            }
        }

        [Theory]
        [InlineData("fakeCategory")]
        [InlineData("Root")]
        public async void ShouldReturnBadResultWhenCategoryUp(string name)
        {
            var result = await catAdminManager.CategoryUp(name);
            Assert.Equal(ServiceResult.BadResult().Failed, result.Failed);
        }

        #endregion

        #region Test CategoryDown


        [Fact]
        public async void ShouldMoveCategoryDown()
        {
            using (var transaction = dbConnection.BeginTransaction())
            {
                var result = await catAdminManager.CategoryDown("Articles");
                transaction.Rollback();
                Assert.Equal(ServiceResult.OkResult().Succeeded, result.Succeeded);
            }
        }

        [Theory]
        [InlineData("fakeCategory")]
        public async void ShouldReturnBadResultWhenCategoryDown(string name)
        {
            var result = await catAdminManager.CategoryUp(name);
            Assert.Equal(ServiceResult.BadResult().Failed, result.Failed);
        }

        #endregion

        #region Test CategoryMoveToTrash
        
        [Fact]
        public async void ShouldMoveCategoryToTrash()
        {
            var category = DefaultCategory;
            using (var transaction = dbConnection.BeginTransaction())
            {
                await catAdminManager.CreateCategoryAsync(category);
                await catAdminManager.CategoryMoveToTrashAsync(category.Name);
                var result = dbConnection.Categories.FirstOrDefault(x =>x.Name == category.Name);
                Assert.NotNull(result);
                Assert.True(result.IsDeleted);
                transaction.Rollback();
            }
        }

        #endregion
    }
}
