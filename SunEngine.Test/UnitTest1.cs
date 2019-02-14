using System;
using Google.Protobuf.Reflection;
using Xunit;
using SunEngine;


namespace SunEngine.Test
{
    public class UnitTest1
    {
        [Fact]
        public void ShouldGetDataBaseConnection()
        {
            var dbConnection = TestUtils.GetTestDataBaseConnection();
            Assert.NotNull(dbConnection);
        }

        [Fact]
        public void ShouldGetCategories()
        {
            var dbConnection = TestUtils.GetTestDataBaseConnection();
            var categories = dbConnection.Categories;
            Assert.NotNull(dbConnection.Categories);
        }
    }
}
