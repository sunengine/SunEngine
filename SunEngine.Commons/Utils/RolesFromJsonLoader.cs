using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using SunEngine.Commons.Models;
using SunEngine.Commons.Models.Authorization;
using SunEngine.Commons.Security;

namespace SunEngine.Commons.Utils
{
    public class RolesFromJsonLoader
    {
        public readonly List<Role> roles = new List<Role>();
        public readonly List<CategoryAccess> categoryAccesses = new List<CategoryAccess>(); 
        public readonly List<CategoryOperationAccess> categoryOperationAccesses = new List<CategoryOperationAccess>();

        private readonly IDictionary<string, Category> categories;
        private readonly IDictionary<string, OperationKey> operationKeys;
        private readonly JsonSchema4 schema;
        
        public RolesFromJsonLoader(
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
            
            JObject rolesJson = JObject.Parse(jsonText);

            var errors = schema.Validate(rolesJson);
                
            if(errors!=null && errors.Count > 0)
            {
                throw new Exception(string.Join(@"\n\n\n",errors));
            }
            
            
            int id = 0;
            int categoryAccessId = 0;
            
            foreach (JProperty jProp in rolesJson.Properties())
            {
                JObject userGroupJson = (JObject)jProp.Value;

                id++;

                if (!((IDictionary<string, JToken>) userGroupJson).ContainsKey("Title"))
                    throw new Exception("Can not find category title"); 
                    
                Role role = new Role
                {
                    Id = id,
                    Name = jProp.Name,
                    Title = (string) userGroupJson["Title"],
                    IsSuper = ((IDictionary<string, JToken>) userGroupJson).ContainsKey("IsSuper") && (bool) userGroupJson["IsSuper"],
                    SortNumber = id
                };
                role.NormalizedName = Normalizer.Normalize(role.Name);

                roles.Add(role);

                if (!userGroupJson.TryGetValue("Categories", out var categoriesAccessJsonList)) continue;
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
                        RoleId = role.Id
                    };

                    categoryAccesses.Add(categoryAccess);

                    var operationKeysJsonObject = (JObject) categoriesAccessJson["OperationKeys"];

                    foreach (var operationKeyJson in operationKeysJsonObject.Properties())
                    {                            
                        string keyName = operationKeyJson.Name;

                        if (!role.IsSuper && allSuperKeys.Contains(keyName))
                        {
                            throw new Exception($"Ordinary UserGroup '{role.Name}' can not contain IsSuper key '{keyName}'");
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

