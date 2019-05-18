using System;
using System.Collections.Generic;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Authorization;
using SunEngine.Core.Models.Materials;

namespace SunEngine.DataSeed
{
    /// <summary>
    /// Container for data to seed in memory.
    /// After it goes to database with DataBaseSeeder
    /// </summary>
    public class DataContainer
    {
        private int currentUserId = 1;
        
        private int currentSectionTypeId = 1;

        private int currentCategoryId = 1;

        public int currentCommentId = 1;

        public int currentMaterialId = 1;

        private int currentCategoryAccessId = 1;

        private int currentUserGroupId = 1;

        private int operationKeyId = 1;
        
        private int menuItemId = 1;
        
        private DateTime commentPublishDate = DateTime.UtcNow.AddMinutes(-3);

        public Category RootCategory;
        public List<SectionType> SectionTypes = new List<SectionType>(); 
        public List<Category> Categories = new List<Category>();
        public List<Comment> Comments = new List<Comment>();
        public List<Material> Materials = new List<Material>();
        public List<User> Users = new List<User>();
        public List<Role> Roles = new List<Role>();
        public List<UserRole> UserRoles = new List<UserRole>();
        public List<CategoryAccess> CategoryAccesses = new List<CategoryAccess>(); 
        public List<CategoryOperationAccess> CategoryOperationAccesses = new List<CategoryOperationAccess>();
        public List<OperationKey> OperationKeys = new List<OperationKey>();
        public List<MenuItem> MenuItems = new List<MenuItem>();
        public CacheSettings CacheSettings = null;

        public Random ran = new Random();
        
        public int NextSectionTypeId()
        {
            return currentSectionTypeId++;
        }
        
        public int NextCategoryId()
        {
            return currentCategoryId++;
        }

        public int MaxUserId => currentUserId;

        public int NextUserId()
        {
            return currentUserId++;
        }

        public int NextCommentId()
        {
            return currentCommentId++;
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
        
        public int NextMenuItemId()
        {
            return menuItemId++;
        }

        public int GetRandomUserId()
        {
            return Users[ran.Next(Users.Count)].Id;
        }
        
        public DateTime IterateCommentPublishDate()
        {
            commentPublishDate = commentPublishDate.AddMinutes(-5);
            return commentPublishDate;
        }
    }
}