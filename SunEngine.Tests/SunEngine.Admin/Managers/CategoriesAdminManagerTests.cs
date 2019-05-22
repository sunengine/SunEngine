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
        private readonly CategoriesAdminManager categoryAdminManager;
        private DataBaseConnection dbConnection;

        public CategoriesAdminManagerTests()
        {
            categoryAdminManager = DefaultCategoryAdminManager();
        }

        private CategoriesAdminManager DefaultCategoryAdminManager()
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

                await categoryAdminManager.CreateCategoryAsync(category);

                int countAfter = dbConnection.Categories.Count();
                transaction.Rollback();

                Assert.NotEqual(countAfter, countBefore);
            }
        }

        [Fact]
        public async void ShouldThrowExceptionIfCategoryIsNullWhenCreate()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await categoryAdminManager.CreateCategoryAsync(null));
        }

        [Fact]
        public async void ShouldThrowExceptionIfParentIdIsNull()
        {
            var category = new Category {Name = "Test", Title ="test"};

            await Assert.ThrowsAsync<ParentCategoryNotFoundByIdException>(async () =>
                await categoryAdminManager.CreateCategoryAsync(category));
        }

        [Fact]
        public async void ShouldThrowExceptionIfCategoryNameIsNull()
        {
            var category = new Category { Name = null, Title = "test" };

            await Assert.ThrowsAsync<InvalidModelException>(async () =>
                await categoryAdminManager.CreateCategoryAsync(category));
        }

        [Fact]
        public async void ShouldThrowExceptionIfCategoryTitleIsNull()
        {
            var category = new Category { Name = "Test", Title =  null};

            await Assert.ThrowsAsync<InvalidModelException>(async () =>
                await categoryAdminManager.CreateCategoryAsync(category));
        }

        #endregion

        #region Test UpdateCategoryAsync

        [Fact]
        public async void ShouldUpdateCategory()
        {
            var category = DefaultCategory;

            using (var transaction = dbConnection.BeginTransaction())
            {
                await categoryAdminManager.CreateCategoryAsync(category);
                category = dbConnection.Categories.FirstOrDefault(x => x.Name == category.Name);

                Assert.NotNull(category);

                category.Name = "UpdatedTestCategory";
                await categoryAdminManager.UpdateCategoryAsync(category);

                var resultCategory = dbConnection.Categories.FirstOrDefault(x => x.Name == "UpdatedTestCategory");

                Assert.NotNull(resultCategory);
                transaction.Rollback();
            }
        }

        [Fact]
        public async void ShouldThrowExceptionWhenUpdateCategoryWithNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await categoryAdminManager.UpdateCategoryAsync(null));
        }

        [Fact]
        public void ShouldThrowExceptionWhenUpdateCategoryWithWrongId()
        {
            var category = DefaultCategory;
            category.Id = -1;
            var ex = Record.ExceptionAsync(async () => await categoryAdminManager.UpdateCategoryAsync(category));

            Assert.Equal($"No category with {category.Id} id", ex.Result.Message);
        }

        [Fact]
        public async void ShouldThrowExceptionWhenUpdateCategoryWithWrongParentId()
        {
            var category = DefaultCategory;

            using (var transaction = dbConnection.BeginTransaction())
            {
                await categoryAdminManager.CreateCategoryAsync(category);
                category = dbConnection.Categories.FirstOrDefault(x => x.Name == category.Name);

                Assert.NotNull(category);

                category.ParentId = -1;

                await Assert.ThrowsAsync<ParentCategoryNotFoundByIdException>(async () =>
                    await categoryAdminManager.UpdateCategoryAsync(category));
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
                await categoryAdminManager.CreateCategoryAsync(category);

                var result = await categoryAdminManager.CategoryUp("TestCategory");
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
            var result = await categoryAdminManager.CategoryUp(name);

            Assert.Equal(ServiceResult.BadResult().Failed, result.Failed);
        }

        #endregion

        #region Test CategoryDown

        [Fact]
        public async void ShouldMoveCategoryDown()
        {
            using (var transaction = dbConnection.BeginTransaction())
            {
                var result = await categoryAdminManager.CategoryDown("Articles");
                transaction.Rollback();

                Assert.Equal(ServiceResult.OkResult().Succeeded, result.Succeeded);
            }
        }

        [Theory]
        [InlineData("fakeCategory")]
        public async void ShouldReturnBadResultWhenCategoryDown(string name)
        {
            var result = await categoryAdminManager.CategoryUp(name);

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
                await categoryAdminManager.CreateCategoryAsync(category);

                await categoryAdminManager.CategoryMoveToTrashAsync(category.Name);

                var result = dbConnection.Categories.FirstOrDefault(x =>x.Name == category.Name);

                Assert.NotNull(result);
                Assert.True(result.IsDeleted);
                transaction.Rollback();
            }
        }
        #endregion
    }
}
