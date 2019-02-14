using System;
using System.Collections.Generic;
using SunEngine.Models;
using SunEngine.Models.Authorization;
using SunEngine.Models.Materials;

namespace DataSeedDev.Seeder
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
        public List<Role> UserGroups = new List<Role>();
        public List<UserRole> UserToGroups = new List<UserRole>();
        public List<CategoryAccess> CategoryAccesses = new List<CategoryAccess>(); 
        public List<CategoryOperationAccess> CategoryOperationAccesses = new List<CategoryOperationAccess>();
        public List<OperationKey> OperationKeys = new List<OperationKey>(); 

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