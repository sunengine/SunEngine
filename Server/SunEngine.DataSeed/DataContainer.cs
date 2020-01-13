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
		public int currentCommentId = 1;
		public int currentMaterialId = 1;

		private int currentUserId = 1;
		private int currentSectionTypeId = 1;
		private int currentCategoryId = 2;
		private int currentComponentId = 1;
		private int currentCategoryAccessId = 1;
		private int currentUserGroupId = 1;
		private int operationKeyId = 1;
		private int menuItemId = 2;
		private DateTime commentPublishDate = DateTime.UtcNow.AddMinutes(-3);

		public Category RootCategory;
		public MenuItem RootMenuItem;

		public List<Category> Categories = new List<Category>();
		public List<Component> Components = new List<Component>();
		public List<Comment> Comments = new List<Comment>();
		public List<Material> Materials = new List<Material>();
		public List<User> Users = new List<User>();
		public List<Role> Roles = new List<Role>();
		public List<UserRole> UserRoles = new List<UserRole>();
		public List<CategoryAccess> CategoryAccesses = new List<CategoryAccess>();
		public List<CategoryOperationAccess> CategoryOperationAccesses = new List<CategoryOperationAccess>();
		public List<OperationKey> OperationKeys = new List<OperationKey>();
		public List<MenuItem> MenuItems = new List<MenuItem>();
		public List<CipherSecret> CipherSecrets = new List<CipherSecret>();

		public Random ran = new Random();

		public int NextSectionTypeId() => currentSectionTypeId++;

		public int NextCategoryId() => currentCategoryId++;

		public int NextComponentId() => currentComponentId++;

		public int MaxUserId => currentUserId;

		public int NextUserId() => currentUserId++;

		public int NextCommentId() => currentCommentId++;

		public int NextMaterialId() => currentMaterialId++;

		public int NextCategoryAccessId() => currentCategoryAccessId++;

		public int NextUserGroupId() => currentUserGroupId++;

		public int NextOperationKeyId() => operationKeyId++;

		public int NextMenuItemId() => menuItemId++;

		public int GetRandomUserId() => Users[ran.Next(Users.Count)].Id;

		public DateTime IterateCommentPublishDate()
		{
			commentPublishDate = commentPublishDate.AddMinutes(-3);
			return commentPublishDate;
		}
	}
}