using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using LinqToDB;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors;
using SunEngine.Core.Errors.Exceptions;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;

namespace SunEngine.Admin.Services
{
	public class SkinsAdminService : DbService
	{
		public readonly string WwwRootPath;
		public readonly string SkinsPath;
		public readonly string PartialSkinsPath;

		private readonly int MaxArchiveSize;
		private readonly int MaxExtractArchiveSize;

		protected readonly ConfigurationAdminService configurationAdminService;

		private readonly List<string> requiredFilesMain = new List<string>()
		{
			"styles.css",
			"preview.png",
			"readme.md"
		};

		private readonly List<string> requiredFilesPartial = new List<string>()
		{
			"styles.css",
		};

		private readonly List<string> allowedExtensions = new List<string>()
		{
			".scss", ".sass", ".css", ".map", ".png", ".jpg", ".jpeg", ".gif",
			".svg", ".eot", ",otf", ".ttf", ".woff", ".woff2", ".json", ".md"
		};

		public SkinsAdminService(
			IPathService pathService,
			IOptionsMonitor<SkinsOptions> fileLoadingOptions,
			ConfigurationAdminService configurationAdminService,
			DataBaseConnection db) : base(db)
		{
			this.configurationAdminService = configurationAdminService;
			SkinsPath = pathService.GetPath(PathNames.SkinsDirName);
			PartialSkinsPath = pathService.GetPath(PathNames.PartialSkinsDirName);
			MaxArchiveSize = fileLoadingOptions.CurrentValue.MaxArchiveSizeKb * 1024;
			MaxExtractArchiveSize = fileLoadingOptions.CurrentValue.MaxExtractArchiveSizeKb * 1024;
			WwwRootPath = pathService.WwwRootDir;
		}

		public void UploadSkin(IFormFile file, SkinType skinType)
		{
			var fileName = file.FileName;
			var fileStream = file.OpenReadStream();
			var extension = Path.GetExtension(fileName);

			if (extension != ".zip")
				throw new SunErrorException(new Error("NotValidSkinFileNotZip", "Skin file has to be .zip"));

			if (file.Length > MaxArchiveSize)
				throw new SunErrorException(new Error("VeryBigFile", $"Max file size {MaxArchiveSize}Kb"));

			var zipArchive = new ZipArchive(fileStream);
			var zipEntry = zipArchive.GetEntry("info.json");
			if (zipEntry == null)
				throw new SunErrorException(new Error("SkinFileNotContainInfoJson",
					"Skin archive do not contain info.json file"));

			if (zipArchive.Entries.Sum(entry => entry.Length) > MaxExtractArchiveSize)
			{
				throw new SunErrorException(new Error("VeryBigExtractArchive",
					$"Max extract archive size {MaxExtractArchiveSize}Kb"));
			}

			var fileNames = zipArchive.Entries.Select(x => x.Name);
			var missingFiles = (skinType == SkinType.Main ? requiredFilesMain : requiredFilesPartial)
				.Where(x => !fileNames.Contains(x)).ToList();
			if (missingFiles.Count > 0)
			{
				var strMissingFiles = missingFiles.Aggregate((x, y) => $"{x}, {y}");
				throw new SunErrorException(new Error("MissingRequiredFiles",
					$"Missing required files: {strMissingFiles}"));
			}

			var hasDisallowedFile =
				zipArchive.Entries.All(entry => allowedExtensions.Contains(Path.GetExtension(entry.FullName)));
			if (!hasDisallowedFile)
				throw new SunErrorException(new Error("HasDisallowedFile", ""));

			var jsonString = new StreamReader(zipEntry.Open()).ReadToEnd();
			var skinInfo = JsonConvert.DeserializeObject<SkinInfo>(jsonString);
			var skinDirPath = Path.Combine(skinType == SkinType.Main ? SkinsPath : PartialSkinsPath,
				PathUtils.ClearPathToken(skinInfo.Name));

			if (Directory.Exists(skinDirPath))
				Directory.Delete(skinDirPath, true);
			Directory.CreateDirectory(skinDirPath);

			zipArchive.ExtractToDirectory(skinDirPath, true);
		}

		public void DeleteSkin(string name, SkinType skinType)
		{
			var secureSkinName = PathUtils.ClearPathToken(name);

			if (skinType == SkinType.Main)
			{
				if (db.ConfigurationItems.Single(x => x.Name == "Skins:CurrentSkinName").Value == name)
					throw new SunException("Can not delete current skin");
			}
			else
			{
				if (db.ConfigurationItems.Single(x => x.Name == "Skins:PartialSkinsNames").Value.Split(",")
					.Select(x => x.Trim()).Contains(name))
					throw new SunException("Can not delete enabled partial skin");
			}

			var pathToDelete = Path.Combine(skinType == SkinType.Main ? SkinsPath : PartialSkinsPath, secureSkinName);
			Directory.Delete(pathToDelete, true);
		}

		public void ChangeSkin(string name)
		{
			var secureSkinName = PathUtils.ClearPathToken(name);

			var selectedSkinPath = Path.Combine(SkinsPath, secureSkinName);

			if (!Directory.Exists(selectedSkinPath))
				throw new SunException($"No skin {secureSkinName} in skins directory");

			db.ConfigurationItems.Where(x => x.Name == "Skins:CurrentSkinName").Set(x => x.Value, secureSkinName)
				.Update();

			configurationAdminService.ReloadConfigurationOptions();
			configurationAdminService.UpdateClientScripts();
		}

		public void EnablePartialSkin(string name, bool enable)
		{
			var secureSkinName = PathUtils.ClearPathToken(name);

			var selectedSkinPath = Path.Combine(PartialSkinsPath, secureSkinName);

			if (!Directory.Exists(selectedSkinPath))
				throw new SunException($"No skin {secureSkinName} in skins directory");

			var names = db.ConfigurationItems.Single(x => x.Name == "Skins:PartialSkinsNames").Value.Split(",")
				.Select(x => x.Trim()).ToList();

			names.Remove("");

			if (enable)
			{
				names.Add(secureSkinName);
				names = names.Distinct().ToList();
			}
			else
				names.Remove(secureSkinName);

			var namesString = string.Join(",", names);
			db.ConfigurationItems.Where(x => x.Name == "Skins:PartialSkinsNames").Set(x => x.Value, x => namesString)
				.Update();

			configurationAdminService.ReloadConfigurationOptions();
			configurationAdminService.UpdateClientScripts();
		}

		public string GetCustomCss()
		{
			var customCssPath = Path.Combine(WwwRootPath, PathNames.StaticsDirName, PathNames.CustomCssFileName);
			return !File.Exists(customCssPath) ? "" : File.ReadAllText(customCssPath);
		}

		public void UpdateCustomCss(string cssText)
		{
			var customCssPath = Path.Combine(WwwRootPath, PathNames.StaticsDirName, PathNames.CustomCssFileName);
			if (File.Exists(customCssPath))
				File.Delete(customCssPath);

			File.WriteAllText(customCssPath, cssText);
			
			configurationAdminService.UpdateCustomCssVersion();
		}

		public List<SkinInfo> GetAllSkins(SkinType skinType)
		{
			var skinsPaths = Directory.GetDirectories(skinType == SkinType.Main ? SkinsPath : PartialSkinsPath);
			var skins = skinsPaths.Select(Path.GetFileName).OrderBy(x => x).ToArray();

			List<string> selectedSkinsNames =
				skinType == SkinType.Main
					? new List<string> {db.ConfigurationItems.Single(x => x.Name == "Skins:CurrentSkinName").Value}
					: db.ConfigurationItems.Single(x => x.Name == "Skins:PartialSkinsNames").Value.Split(",")
						.Select(x => x.Trim()).ToList();

			var skinsInfos = new List<SkinInfo>();

			foreach (var skin in skins)
			{
				try
				{
					var jsonInfo = File.ReadAllText(Path.Combine(
						skinType == SkinType.Main ? SkinsPath : PartialSkinsPath, skin,
						"info.json"));
					SkinInfo skinInfo = JsonConvert.DeserializeObject<SkinInfo>(jsonInfo);
					if (selectedSkinsNames.Contains(skinInfo.Name))
						skinInfo.Current = true;

					skinsInfos.Add(skinInfo);
				}
				catch (Exception)
				{
					// ignored
				}
			}

			return skinsInfos;
		}

		public enum SkinType
		{
			Main = 0,
			Partial = 1
		}

		public class SkinInfo
		{
			public string Name { get; set; }
			public string Author { get; set; }
			public string[] Contacts { get; set; }
			public int Version { get; set; }
			public string SourceUrl { get; set; }
			public string Description { get; set; }
			public bool Current { get; set; }
		}
	}
}