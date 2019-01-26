using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SunEngine.Commons.Models;
using SunEngine.Commons.Models.UserGroups;
using SunEngine.Commons.Services;

namespace SunEngine.Services.Admin
{
    public class UserGroupsLoaderFromJson
    {
        public List<UserGroupDB> userGroups = new List<UserGroupDB>();
        public List<CategoryAccessDB> categoryAccesses = new List<CategoryAccessDB>(); 
        public List<CategoryOperationAccessDB> categoryOperationAccesses = new List<CategoryOperationAccessDB>();

        private readonly IDictionary<string, Category> categories;
        private readonly IDictionary<string, OperationKeyDB> operationKeys;

        public UserGroupsLoaderFromJson(
            IDictionary<string, Category> categories,
            IDictionary<string,OperationKeyDB> operationKeys)
        {
            this.categories = categories;
            this.operationKeys = operationKeys;
        }
            
        public void Seed(string jsonText)
        {
            IList<string> allSuperKeys = OperationKeysContainer.GetAllSuperKeys();
            
            JArray groupsJson = JArray.Parse(jsonText);

            int id = 0;
            int categoryAccessId = 0;
            
            foreach (JObject userGroupJson in groupsJson)
            {
                id++;
                
                UserGroupDB userGroupDb = new UserGroupDB
                {
                    Id = id,
                    Name = (string) userGroupJson["Name"],
                    Title = (string) userGroupJson["Title"],
                    IsSuper = userGroupJson.ContainsKey("IsSuper") && (bool) userGroupJson["IsSuper"],
                    SortNumber = id
                };
                userGroupDb.NormalizedName = userGroupDb.Name.ToUpper();

                userGroups.Add(userGroupDb);

                var categoriesAccessJsonList = (JArray) userGroupJson["Categories"];
                if (categoriesAccessJsonList != null)
                {
                    foreach (var categoriesAccessJson in categoriesAccessJsonList)
                    {
                        string name = (string) categoriesAccessJson["Category"];
                        
                        if (!categories.ContainsKey(name))
                        {
                            throw new Exception("No such category: " + name);
                        }
                        
                        Category category = categories[name];

                        categoryAccessId++;
                        
                        CategoryAccessDB categoryAccessDb = new CategoryAccessDB
                        {
                            Id = categoryAccessId,
                            CategoryId = category.Id,
                            UserGroupId = userGroupDb.Id
                        };

                        categoryAccesses.Add(categoryAccessDb);

                        var operationKeysJsonObject = (JObject) categoriesAccessJson["OperationKeys"];

                        foreach (var operationKeyJson in operationKeysJsonObject.Properties())
                        {                            
                            string keyName = operationKeyJson.Name;

                            if (!userGroupDb.IsSuper && allSuperKeys.Contains(keyName))
                            {
                                throw new Exception($"Ordinary UserGroup '{userGroupDb.Name}' can not contain IsSuper key '{keyName}'");
                            }
                            
                            if (!operationKeys.ContainsKey(keyName))
                            {
                                throw new Exception($"No such key in registered keys '{keyName}'");
                            }

                            var operationKey = operationKeys[keyName];
                            
                            CategoryOperationAccessDB categoryOperationAccessDb = new CategoryOperationAccessDB
                            {
                                CategoryAccessId = categoryAccessDb.Id,
                                OperationKeyId = operationKey.OperationKeyId,
                                Access = (bool) operationKeyJson.Value
                            };

                            categoryOperationAccesses.Add(categoryOperationAccessDb);
                        }
                    }
                }
            }
        }
    }
}

