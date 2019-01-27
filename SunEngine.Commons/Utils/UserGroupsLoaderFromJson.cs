using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using SunEngine.Commons.Models;
using SunEngine.Commons.Models.UserGroups;
using SunEngine.Commons.Services;

namespace SunEngine.Commons.Utils
{
    public class UserGroupsLoaderFromJson
    {
        public List<UserGroupDB> userGroups = new List<UserGroupDB>();
        public List<CategoryAccessDB> categoryAccesses = new List<CategoryAccessDB>(); 
        public List<CategoryOperationAccessDB> categoryOperationAccesses = new List<CategoryOperationAccessDB>();

        private readonly IDictionary<string, Category> categories;
        private readonly IDictionary<string, OperationKeyDB> operationKeys;
        private readonly JsonSchema4 schema;
        
        public UserGroupsLoaderFromJson(
            IDictionary<string, Category> categories,
            IDictionary<string,OperationKeyDB> operationKeys,
            JsonSchema4 schema)
        {
            this.categories = categories;
            this.operationKeys = operationKeys;
            this.schema = schema;
        }
            
        public void Seed(string jsonText)
        {
            IList<string> allSuperKeys = OperationKeysContainer.GetAllSuperKeys();
            
            JObject groupsJson = JObject.Parse(jsonText);

            var errors = schema.Validate(groupsJson);
                
            if(errors!=null && errors.Count > 0)
            {
                throw new Exception(string.Join(@"\n\n\n",errors));
            }
            
            
            int id = 0;
            int categoryAccessId = 0;
            
            foreach (JProperty jProp in groupsJson.Properties())
            {
                JObject userGroupJson = (JObject)jProp.Value;

                
                id++;

                if (!((IDictionary<string, JToken>) userGroupJson).ContainsKey("Title"))
                    throw new Exception("Can not find category title"); 
                    
                UserGroupDB userGroupDb = new UserGroupDB
                {
                    Id = id,
                    Name = jProp.Name,
                    Title = (string) userGroupJson["Title"],
                    IsSuper = ((IDictionary<string, JToken>) userGroupJson).ContainsKey("IsSuper") && (bool) userGroupJson["IsSuper"],
                    SortNumber = id
                };
                userGroupDb.NormalizedName = Normalizer.Singleton.Normalize(userGroupDb.Name);

                userGroups.Add(userGroupDb);

                if (userGroupJson.TryGetValue("Categories",out var categoriesAccessJsonList))
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

