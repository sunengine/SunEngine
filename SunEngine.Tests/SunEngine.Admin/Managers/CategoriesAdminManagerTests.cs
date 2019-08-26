using System;
using System.Linq;
using System.Reflection;
using System.Transactions;
using LinqToDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SunEngine.Admin.Managers;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors;
using SunEngine.Core.Models;
using SunEngine.Core.Services;
using Xunit;
using Xunit.Sdk;


namespace SunEngine.Tests.SunEngine.Admin.Managers
{
    public class CategoriesAdminManagerTests
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class TestBeforeAfter : BeforeAfterTestAttribute
        {
            private TransactionScope transactionScope;

            public override void Before(MethodInfo methodUnderTest)
            {
                transactionScope = new TransactionScope();
            }

            public override void After(MethodInfo methodUnderTest)
            {
                transactionScope.Dispose();
                Console.WriteLine("RollbackTransaction");
            }
        }


        private readonly CategoriesAdminManager categoryAdminManager;
        private static readonly DataBaseConnection dbConnection = DefaultInit.GetTestDataBaseConnection();

        public CategoriesAdminManagerTests()
        {
            categoryAdminManager = DefaultCategoryAdminManager();
        }

        private CategoriesAdminManager DefaultCategoryAdminManager()
        {
            var dbFactory = DefaultInit.GetTestDataBaseFactory();
            var catCache = new CategoriesCache(dbFactory);

            /*return new CategoriesAdminManager(dbConnection,
                Options.Create(new MaterialsOptions {SubTitleLength = 100, PreviewLength = 800}), catCache,
                new Sanitizer(new SanitizerOptions()));*/

            throw new NotSupportedException("Need moq SanitizerService");
        }

        private Category DefaultCategory => new Category
            {Name = "TestCategory", ParentId = 1, Title = "TestCategoryTitle"};

        private Category DefaultCategory2 => new Category
            {Name = "TestCategory2", ParentId = 1, Title = "TestCategoryTitle2"};


        #region Test CreateCategoryAsync

        [Fact]
        public async void ShouldCreateCategory()
        {
            var category = DefaultCategory;

            int countBefore = dbConnection.Categories.Count();

            await categoryAdminManager.CreateCategoryAsync(category);

            int countAfter = dbConnection.Categories.Count();

            Assert.NotEqual(countAfter, countBefore);

            await dbConnection.Categories.Where(x => x.Id == category.Id).DeleteAsync();
        }

        [Fact]
        public async void ShouldThrowExceptionIfCategoryIsNullWhenCreate()
        {
            using (dbConnection.BeginTransaction())
            {
                await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                    await categoryAdminManager.CreateCategoryAsync(null));
            }
        }

        [Fact]
        public async void ShouldThrowExceptionIfParentIdIsNull()
        {
            var category = new Category {Name = "Test", Title = "test"};

            using (dbConnection.BeginTransaction())
            {
                await Assert.ThrowsAsync<SunParentEntityNotFoundException>(async () =>
                    await categoryAdminManager.CreateCategoryAsync(category));
            }
        }

        [Fact]
        public async void ShouldThrowExceptionIfCategoryNameIsNull()
        {
            var category = new Category {Name = null, Title = "test"};

            using (dbConnection.BeginTransaction())
            {
                await Assert.ThrowsAsync<SunModelValidationException>(async () =>
                    await categoryAdminManager.CreateCategoryAsync(category));
            }
        }

        [Fact]
        public async void ShouldThrowExceptionIfCategoryTitleIsNull()
        {
            var category = new Category {Name = "Test", Title = null};

            using (dbConnection.BeginTransaction())
            {
                await Assert.ThrowsAsync<SunModelValidationException>(async () =>
                    await categoryAdminManager.CreateCategoryAsync(category));
            }
        }

        #endregion

        #region Test UpdateCategoryAsync

        [Fact]
        public async void ShouldUpdateCategory()
        {
            var category = DefaultCategory;

            using (dbConnection.BeginTransaction())
            {
                await categoryAdminManager.CreateCategoryAsync(category);
                category = dbConnection.Categories.FirstOrDefault(x => x.Name == category.Name);

                Assert.NotNull(category);

                category.Name = "UpdatedTestCategory";
                await categoryAdminManager.UpdateCategoryAsync(category);

                var resultCategory = dbConnection.Categories.FirstOrDefault(x => x.Name == "UpdatedTestCategory");

                Assert.NotNull(resultCategory);
            }
        }

        [Fact]
        public async void ShouldThrowExceptionWhenUpdateCategoryWithNull()
        {
            using (dbConnection.BeginTransaction())
            {
                await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                    await categoryAdminManager.UpdateCategoryAsync(null));
            }
        }

        [Fact]
        public void ShouldThrowExceptionWhenUpdateCategoryWithWrongId()
        {
            var category = DefaultCategory;
            category.Id = -1;
            using (dbConnection.BeginTransaction())
            {
                var ex = Record.ExceptionAsync(async () => await categoryAdminManager.UpdateCategoryAsync(category));

                Assert.Equal($"No category with {category.Id} id", ex.Result.Message);
            }
        }

        [Fact]
        public async void ShouldThrowExceptionWhenUpdateCategoryWithWrongParentId()
        {
            var category = DefaultCategory;

            using (dbConnection.BeginTransaction())
            {
                await categoryAdminManager.CreateCategoryAsync(category);
                category = dbConnection.Categories.FirstOrDefault(x => x.Name == category.Name);

                Assert.NotNull(category);

                category.ParentId = -1;

                await Assert.ThrowsAsync<SunParentEntityNotFoundException>(async () =>
                    await categoryAdminManager.UpdateCategoryAsync(category));
            }
        }

        #endregion

        #region Test CategoryUp

        [Fact]
        public async void ShouldMoveCategoryUp()
        {
            var category1 = DefaultCategory;
            var category2 = DefaultCategory2;

            using (dbConnection.BeginTransaction())
            {
                await categoryAdminManager.CreateCategoryAsync(category1);
                await categoryAdminManager.CreateCategoryAsync(category2);

                int sortNumber1 = dbConnection.Categories.First(x => x.Name == category1.Name).SortNumber;
                int sortNumber2 = dbConnection.Categories.First(x => x.Name == category2.Name).SortNumber;

                await categoryAdminManager.CategoryUp(category2.Name);

                int sortNumber1Next = dbConnection.Categories.First(x => x.Name == category1.Name).SortNumber;
                int sortNumber2Next = dbConnection.Categories.First(x => x.Name == category2.Name).SortNumber;

                Assert.Equal(sortNumber1, sortNumber2Next);
                Assert.Equal(sortNumber2, sortNumber1Next);
            }
        }

        /*[Theory]
        [InlineData("fakeCategory")]
        [InlineData("Root")]
        public async void ShouldReturnBadResultWhenCategoryUp(string name)
        {
            using (dbConnection.BeginTransaction())
            {
                await Assert.ThrowsAsync<SunEntityNotFoundException>(
                    () => categoryAdminManager.CategoryUp(name)
                );
            }
        }*/

        #endregion

        #region Test CategoryDown

        [Fact]
        public async void ShouldMoveCategoryDown()
        {
            var category1 = DefaultCategory;
            var category2 = DefaultCategory2;

            using (dbConnection.BeginTransaction())
            {
                await categoryAdminManager.CreateCategoryAsync(category1);
                await categoryAdminManager.CreateCategoryAsync(category2);

                int sortNumber1 = dbConnection.Categories.First(x => x.Name == category1.Name).SortNumber;
                int sortNumber2 = dbConnection.Categories.First(x => x.Name == category2.Name).SortNumber;

                await categoryAdminManager.CategoryDown(category1.Name);

                int sortNumber1Next = dbConnection.Categories.First(x => x.Name == category1.Name).SortNumber;
                int sortNumber2Next = dbConnection.Categories.First(x => x.Name == category2.Name).SortNumber;

                Assert.Equal(sortNumber1, sortNumber2Next);
                Assert.Equal(sortNumber2, sortNumber1Next);
            }
        }

        /*[Theory]
        [InlineData("fakeCategory")]
        public async void ShouldReturnBadResultWhenCategoryDown(string name)
        {
            using (dbConnection.BeginTransaction())
            {
                await Assert.ThrowsAsync<SunEntityNotFoundException>(
                    () => categoryAdminManager.CategoryDown(name)
                );
            }
        }*/

        #endregion

        #region Test CategoryMoveToTrash

        [Fact]
        public async void ShouldMoveCategoryToTrash()
        {
            var category = DefaultCategory;
            using (dbConnection.BeginTransaction())
            {
                await categoryAdminManager.CreateCategoryAsync(category);

                await categoryAdminManager.CategoryMoveToTrashAsync(category.Name);

                var result = dbConnection.Categories.FirstOrDefault(x => x.Name == category.Name);

                Assert.NotNull(result);
                Assert.True(result.DeletedDate != null);
            }
        }

        #endregion
    }
}
