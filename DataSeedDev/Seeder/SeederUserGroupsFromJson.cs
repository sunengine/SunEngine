using System;
using System.IO;
using System.Linq;
using LinqToDB;
using Newtonsoft.Json.Linq;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Models.UserGroups;

namespace SunEngine.Seeder
{
    public class SeederUserGroupsFromJson
    {
        private readonly DataContainer _dataContainer;

        public SeederUserGroupsFromJson(DataContainer dataContainer)
        {
            _dataContainer = dataContainer;
        }

        public void Seed(string fileName)
        {
            string jsonText = File.ReadAllText(fileName);
            JArray groupsJson = JArray.Parse(jsonText);
            foreach (JObject userGroupJson in groupsJson)
            {
                UserGroupDB userGroupDb = new UserGroupDB
                {
                    Id = _dataContainer.NextUserGroupId(),
                    Name = (string) userGroupJson["Name"],
                    Title = (string) userGroupJson["Title"]
                };
                userGroupDb.NormalizedName = userGroupDb.Name.ToUpper();

                _dataContainer.UserGroups.Add(userGroupDb);

                var categoriesAccessJsonList = (JArray) userGroupJson["Categories"];
                if (categoriesAccessJsonList != null)
                {
                    foreach (var categoriesAccessJson in categoriesAccessJsonList)
                    {
                        string name = (string) categoriesAccessJson["Category"];
                        Category category = _dataContainer.Categories.FirstOrDefault(x => x.Name == name);
                        if (category == null)
                        {
                            throw new Exception("No such category: " + name);
                        }

                        CategoryAccessDB categoryAccessDb = new CategoryAccessDB
                        {
                            Id = _dataContainer.NextCategoryAccessId(),
                            CategoryId = category.Id,
                            UserGroupId = userGroupDb.Id
                        };

                        _dataContainer.CategoryAccesses.Add(categoryAccessDb);

                        var operationKeysJsonObject = (JObject) categoriesAccessJson["OperationKeys"];

                        foreach (var operationKeyJson in operationKeysJsonObject.Properties())
                        {
                            string keyName = operationKeyJson.Name;
                            
                            var operationKey = _dataContainer.OperationKeys.FirstOrDefault(x => x.Name == keyName);
                            if (operationKey == null)
                            {
                                throw new Exception("No suck key in registered keys: " + keyName);
                            }
                            
                            CategoryOperationAccessDB categoryOperationAccessDb = new CategoryOperationAccessDB
                            {
                                CategoryAccessId = categoryAccessDb.Id,
                                OperationKeyId = operationKey.OperationKeyId,
                                Access = (bool) operationKeyJson.Value
                            };

                            _dataContainer.CategoryOperationAccesses.Add(categoryOperationAccessDb);
                        }
                    }
                }
            }
        }
    }
}