using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using LinqToDB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors;
using SunEngine.Core.Errors.Exceptions;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace SunEngine.Admin.Services
{
  public class SkinsAdminService : DbService
  {
    public readonly string WwwRootPath;
    public readonly string SkinsPath;

    private readonly int MaxArchiveSize;
    private readonly int MaxExtractArchiveSize;

    protected readonly ConfigurationAdminService configurationAdminService;

    private readonly List<string> requiredFiles = new List<string>()
    {
      "styles.css",
      "preview.png",
      "readme.md"
    };

    private readonly List<string> allowedExtensions = new List<string>()
    {
      ".scss", ".sass", ".css", ".map", ".png", ".jpg", ".jpeg", ".gif",
      ".svg", ".woff", ".woff2", ".ttf", ".otf", ".json", ".md"
    };

    public SkinsAdminService(
      IPathService pathService,
      IOptionsMonitor<FileLoadingOptions> fileLoadingOptions,
      ConfigurationAdminService configurationAdminService,
      DataBaseConnection db) : base(db)
    {
      this.configurationAdminService = configurationAdminService;
      SkinsPath = pathService.GetPath(PathNames.SkinsDirName);
      MaxArchiveSize = fileLoadingOptions.CurrentValue.MaxArchiveSize * 1024;
      MaxExtractArchiveSize = fileLoadingOptions.CurrentValue.MaxExtractArchiveSize * 1024;
      WwwRootPath = pathService.WwwRootDir;
    }

    public void UploadSkin(IFormFile file)
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
      var missingFiles = requiredFiles.Where(x => !fileNames.Contains(x)).ToList();
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
      var skinDirPath = Path.Combine(SkinsPath, PathUtils.ClearPathToken(skinInfo.Name));

      if (Directory.Exists(skinDirPath))
        Directory.Delete(skinDirPath, true);
      Directory.CreateDirectory(skinDirPath);

      zipArchive.ExtractToDirectory(skinDirPath, true);
    }

    public void DeleteSkin(string name)
    {
      var secureSkinName = PathUtils.ClearPathToken(name);
      var pathToDelete = Path.Combine(SkinsPath, secureSkinName);
      Directory.Delete(pathToDelete, true);
    }

    public void ChangeSkin(string name)
    {
      var secureSkinName = PathUtils.ClearPathToken(name);

      var selectedSkinPath = Path.Combine(SkinsPath, secureSkinName);

      if (!Directory.Exists(selectedSkinPath))
        throw new SunException($"No skin {secureSkinName} in skins directory");

      db.ConfigurationItems.Where(x => x.Name == "Skins:CurrentSkinName").Set(x => x.Value, secureSkinName).Update();

      configurationAdminService.ReloadConfigurationOptions();
      configurationAdminService.UpdateClientScripts();
    }

    public List<SkinInfo> GetAllSkins()
    {
      var skinsPaths = Directory.GetDirectories(SkinsPath);
      var skins = skinsPaths.Select(Path.GetFileName).OrderBy(x => x).ToArray();

      string currentSkinName = db.ConfigurationItems.FirstOrDefault(x => x.Name == "Skins:CurrentSkinName")?.Value;

      var skinsInfos = new List<SkinInfo>();

      foreach (var skin in skins)
      {
        try
        {
          var jsonInfo = File.ReadAllText(Path.Combine(SkinsPath, skin, "info.json"));
          SkinInfo skinInfo = JsonConvert.DeserializeObject<SkinInfo>(jsonInfo);
          if (skinInfo.Name == currentSkinName)
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
