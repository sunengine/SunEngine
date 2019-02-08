using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using SunEngine.Models;
using SunEngine.Models.Authorization;
using SunEngine.Security.Authorization;

namespace SunEngine.Utils
{
    public class UserGroupsLoaderFromJson
    {
        public List<UserGroup> userGroups = new List<UserGroup>();
        public List<CategoryAccess> categoryAccesses = new List<CategoryAccess>(); 
        public List<CategoryOperationAccess> categoryOperationAccesses = new List<CategoryOperationAccess>();

        private readonly IDictionary<string, Category> categories;
        private readonly IDictionary<string, OperationKey> operationKeys;
        private readonly JsonSchema4 schema;
        
        public UserGroupsLoaderFromJson(
            IDictionary<string, Category> categories,
            IDictionary<string,OperationKey> operationKeys,
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
                    
                UserGroup userGroup = new UserGroup
                {
                    Id = id,
                    Name = jProp.Name,
                    Title = (string) userGroupJson["Title"],
                    IsSuper = ((IDictionary<string, JToken>) userGroupJson).ContainsKey("IsSuper") && (bool) userGroupJson["IsSuper"],
                    SortNumber = id
                };
                userGroup.NormalizedName = Normalizer.Singleton.Normalize(userGroup.Name);

                userGroups.Add(userGroup);

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
                        
                        CategoryAccess categoryAccess = new CategoryAccess
                        {
                            Id = categoryAccessId,
                            CategoryId = category.Id,
                            UserGroupId = userGroup.Id
                        };

                        categoryAccesses.Add(categoryAccess);

                        var operationKeysJsonObject = (JObject) categoriesAccessJson["OperationKeys"];

                        foreach (var operationKeyJson in operationKeysJsonObject.Properties())
                        {                            
                            string keyName = operationKeyJson.Name;

                            if (!userGroup.IsSuper && allSuperKeys.Contains(keyName))
                            {
                                throw new Exception($"Ordinary UserGroup '{userGroup.Name}' can not contain IsSuper key '{keyName}'");
                            }
                            
                            if (!operationKeys.ContainsKey(keyName))
                            {
                                throw new Exception($"No such key in registered keys '{keyName}'");
                            }

                            var operationKey = operationKeys[keyName];
                            
                            CategoryOperationAccess categoryOperationAccess = new CategoryOperationAccess
                            {
                                CategoryAccessId = categoryAccess.Id,
                                OperationKeyId = operationKey.OperationKeyId,
                                Access = (bool) operationKeyJson.Value
                            };

                            categoryOperationAccesses.Add(categoryOperationAccess);
                        }
                    }
                }
            }
        }
    }
}

