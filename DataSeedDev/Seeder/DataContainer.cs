using System;
using System.Collections.Generic;
using LinqToDB.Identity;
using SunEngine.Commons.Models;
using SunEngine.Commons.Models.UserGroups;

namespace SunEngine.Seeder
{
    public class DataContainer
    {
        private int currentUserId = 1;

        private int currentCategoryId = 1;

        private int currentMessageId = 1;

        private int currentMaterialId = 1;

        private int currentCategoryAccessId = 1;

        private int currentUserGroupId = 1;

        private int operationKeyId = 1;
        
        private DateTime messagePublishDate = DateTime.UtcNow.AddMinutes(-3);

        public Category RootCategory;
        public List<Category> Categories = new List<Category>();
        public List<Message> Messages = new List<Message>();
        public List<Material> Materials = new List<Material>();
        public List<User> Users = new List<User>();
        public List<UserGroupDB> UserGroups = new List<UserGroupDB>();
        public List<IdentityUserRole<int>> IdentityUserRoles = new List<IdentityUserRole<int>>();
        public List<CategoryAccessDB> CategoryAccesses = new List<CategoryAccessDB>(); 
        public List<CategoryOperationAccessDB> CategoryOperationAccesses = new List<CategoryOperationAccessDB>();
        public List<OperationKeyDB> OperationKeys = new List<OperationKeyDB>(); 

        public Random _ran = new Random();
        
        public int NextCategoryId()
        {
            return currentCategoryId++;
        }

        public int MaxUserId => currentUserId;

        public int NextUserId()
        {
            return currentUserId++;
        }

        public int NextMessageId()
        {
            return currentMessageId++;
        }

        public int NextMaterialId()
        {
            return currentMaterialId++;
        }
        
        public int NextCategoryAccessId()
        {
            return currentCategoryAccessId++;
        }
        
        public int NextUserGroupId()
        {
            return currentUserGroupId++;
        }
        
        public int NextOperationKeyId()
        {
            return operationKeyId++;
        }

        public int GetRandomUserId()
        {
            return Users[_ran.Next(Users.Count)].Id;
        }
        
        public DateTime IterateMessagePublishDate()
        {
            messagePublishDate = messagePublishDate.AddMinutes(-5);
            return messagePublishDate;
        }
    }
}