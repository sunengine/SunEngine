using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Services
{
  public class ConfigurationAdminService
  {
    protected string WwwRootPath { get; }
    protected IConfigurationRoot configurationRoot { get; }

    public ConfigurationAdminService(
      IPathService pathService,
      IConfigurationRoot configurationRoot)
    {
      this.configurationRoot = configurationRoot;
      WwwRootPath = pathService.WwwRootDir;
    }

    public void ReloadConfigurationOptions()
    {
      configurationRoot.Reload();
    }

    public void UpdateClientScripts()
    {
      var itemsToSaveDic = new Dictionary<string, Type>()
      {
        ["Global:SiteName"] = typeof(string),
        ["Global:SiteApi"] = typeof(string),
        ["Global:SiteUrl"] = typeof(string),
        ["Global:UploadImagesUrl"] = typeof(string),
        ["Global:SkinsUrl"] = typeof(string),
        ["Global:PartialSkinsUrl"] = typeof(string),

        ["Client:OpenExternalLinksAtNewTab"] = typeof(bool),
        ["Client:VueDevTools"] = typeof(bool),
        ["Client:VueAppInWindow"] = typeof(bool),
        ["Client:LogInitExtended"] = typeof(bool),
        ["Client:LogRequests"] = typeof(bool),
        ["Client:LogMoveTo"] = typeof(bool),
      };


      var rez = new Dictionary<string, object>();
      foreach (var (key, type) in itemsToSaveDic)
      {
        var value = configurationRoot.GetValue(type, key);

        string[] tokens = key.Split(":");

        Dictionary<string, object> current = rez;
        for (int i = 0; i < tokens.Length - 1; i++)
        {
          if (!current.ContainsKey(tokens[i]))
            current[tokens[i]] = new Dictionary<string, object>();

          current = (Dictionary<string, object>) current[tokens[i]];
        }

        current[tokens[^1]] = value;
      }


      var json = JsonSerializer.Serialize(rez, new JsonSerializerOptions
      {
        WriteIndented = true,
        AllowTrailingCommas = true,
      });

      var configJsPath = Path.Combine(WwwRootPath, "statics", "config.js");

      json = json.Substring(1, json.Length - 2) + ",";

      var configJs = File.ReadAllText(configJsPath);
      configJs = Regex.Replace(configJs, "//( *?)auto-start(.*?)//( *?)auto-end",
        $"// auto-start\n{json}\n // auto-end", RegexOptions.Singleline);
      File.WriteAllText(configJsPath, configJs);
    }

    /*public void UpdateVersion()
    {
      var ran = new Random();

      var indexHtmlPath = Path.Combine(WwwRootPath, "index.html");
      string text = File.ReadAllText(indexHtmlPath);
      Regex reg2 = new Regex("configver=\\d+\"");
      text = reg2.Replace(text, $"configver={ran.Next()}\"");
      File.WriteAllText(indexHtmlPath, text);
    }*/
  }
}
