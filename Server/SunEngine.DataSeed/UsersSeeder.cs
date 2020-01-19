using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Authorization;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;

namespace SunEngine.DataSeed
{
	/// <summary>
	/// Seed users from json file from config dir to DataContainer
	/// </summary>
	public class UsersSeeder
	{
		private const string UsersDefaultPassword = "password";


		private readonly DataContainer dataContainer;
		private readonly string configDir;
		private readonly string uploadImagesDir;
		private readonly string configSeedAvatarsDir;
		private readonly PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

		private string avatarsDir;
		private JArray usersJArray;

		public UsersSeeder(DataContainer dataContainer, string configDir)
		{
			this.dataContainer = dataContainer;
			this.configDir = configDir;
			configSeedAvatarsDir = Path.Combine(configDir, SeederPathsNames.InitDir, SeederPathsNames.AvatarsDir);
			PathService pathService = new PathService(configDir);
			uploadImagesDir = pathService.GetPath(PathNames.UploadImagesDirName);
		}

		public void SeedUsers()
		{
			string fileName = Path.Combine(configDir, SeederPathsNames.InitDir, SeederPathsNames.UsersJsonFile);
			string jsonText = File.ReadAllText(fileName);
			usersJArray = JArray.Parse(jsonText);

			avatarsDir = Path.Combine(uploadImagesDir, SeederPathsNames.StartAvatarsDir);

			if (Directory.Exists(avatarsDir))
				Directory.Delete(avatarsDir, true);
			Directory.CreateDirectory(avatarsDir);

			foreach (var userJ in usersJArray)
				SeedUser(userJ);
		}


		private void SeedUser(JToken usersJ)
		{
			int maxNumber = 1;
			var repeat = usersJ["Repeat"];
			if (repeat != null)
				maxNumber = (int) repeat;

			int startNumber = 1;
			if (usersJ["StartNumber"] != null)
			{
				startNumber = int.Parse((string) usersJ["StartNumber"]);
				maxNumber += startNumber - 1;
			}

			for (int j = startNumber; j <= maxNumber; j++)
			{
				string name = ((string) usersJ["UserName"]).Replace("[n]", j.ToString());

				User user = new User
				{
					Id = dataContainer.NextUserId(),
					Email = usersJ["Email"] != null
						? ((string) usersJ["Email"]).Replace("[n]", j.ToString())
						: name + "@email.email",
					UserName = name,
					EmailConfirmed = true,
					PasswordHash =
						passwordHasher.HashPassword(null, (string) (usersJ["Password"] ?? UsersDefaultPassword)),
					SecurityStamp = string.Empty,
					Information = ((string) usersJ["Information"])?.Replace("[n]", j.ToString()),
					Link = ((string) usersJ["Link"])?.Replace("[n]", j.ToString()),
					RegisteredDate = DateTime.UtcNow
				};
				if (string.IsNullOrEmpty(user.Link))
					user.Link = user.Id.ToString();

				MakeNormalizedUserFields(user);

				string[] exts = {".jpg", ".png", ".gif", ".svg"};
				string avatarPath = null;
				string avatarName = null;
				foreach (var ext in exts)
				{
					avatarName = name + ".jpg";
					var avatarPath1 = Path.Combine(this.configSeedAvatarsDir, avatarName);
					if (!File.Exists(avatarPath1))
						continue;
					avatarPath = avatarPath1;
					break;
				}

				if (avatarPath != null)
				{
					var avatarPathRez = Path.Combine(avatarsDir, avatarName);
					File.Copy(avatarPath, avatarPathRez);
					user.Avatar = user.Photo = Path.Combine(SeederPathsNames.StartAvatarsDir, avatarName);
				}

				dataContainer.Users.Add(user);
			}
		}

		public void SeedUserRoles()
		{
			foreach (var userJ in usersJArray)
				SeedUserRole(userJ);
		}

		private void SeedUserRole(JToken userJ)
		{
			int maxNumber = 1;
			var repeat = userJ["Repeat"];
			if (repeat != null)
				maxNumber = (int) repeat;

			int startNumber = 1;
			if (userJ["StartNumber"] != null)
			{
				startNumber = int.Parse((string) userJ["StartNumber"]);
				maxNumber += startNumber - 1;
			}

			for (int j = startNumber; j <= maxNumber; j++)
			{
				string userName = ((string) userJ["UserName"]).Replace("[n]", j.ToString());

				JArray rolesJ = (JArray) userJ["Roles"];
				foreach (string roleName in rolesJ)
				{
					UserRole userRole = new UserRole
					{
						UserId = dataContainer.Users.First(x => x.UserName == userName).Id,
						RoleId = dataContainer.Roles.First(x => x.Name == roleName).Id
					};

					dataContainer.UserRoles.Add(userRole);
				}
			}
		}

		private void MakeNormalizedUserFields(User user)
		{
			user.NormalizedUserName = Normalizer.Normalize(user.UserName);
			user.NormalizedEmail = Normalizer.Normalize(user.Email);
		}
	}
}