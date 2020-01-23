using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace SunEngine.Core.Cache.Services
{
	public interface IDynamicConfigCache
	{
		void Initialize();
		string DynamicConfigCacheJson { get; }
	}

	public class DynamicConfigCache : IDynamicConfigCache
	{
		protected IConfigurationRoot configurationRoot { get; }

		public string DynamicConfigCacheJson { get; private set; }

		public DynamicConfigCache(IConfigurationRoot configurationRoot)
		{
			this.configurationRoot = configurationRoot;
			Initialize();
		}

		public void Initialize()
		{
			var itemsToSaveDic = new Dictionary<string, Type>()
			{
				["Global:SiteName"] = typeof(string),
				["Global:SiteTitle"] = typeof(string),
				["Global:SiteSubTitle"] = typeof(string),
				["Global:PageTitleTemplate"] = typeof(string),
				["Global:Locale"] = typeof(string),
				["Global:UpdateClientScriptsOnConfigChanges"] = typeof(string),
				["Global:OpenExternalLinksAtNewTab"] = typeof(bool),
				["Global:IconsSet"] = typeof(string),

				["Dev:ShowExceptions"] = typeof(bool),
				["Dev:VueDevTools"] = typeof(bool),
				["Dev:VueAppInWindow"] = typeof(bool),
				["Dev:LogInitExtended"] = typeof(bool),
				["Dev:LogRequests"] = typeof(bool),
				["Dev:LogMoveTo"] = typeof(bool),

				["Comments:TimeToOwnEditInMinutes"] = typeof(int),
				["Comments:TimeToOwnDeleteInMinutes"] = typeof(int),

				["Materials:CommentsPageSize"] = typeof(int),
				["Materials:TimeToOwnEditInMinutes"] = typeof(int),
				["Materials:TimeToOwnDeleteInMinutes"] = typeof(int),

				["Skins:CurrentSkinName"] = typeof(string),
				["Skins:PartialSkinsNames"] = typeof(string),

				["Editor:MaterialToolbar"] = typeof(string),
				["Editor:CommentToolbar"] = typeof(string),
				["Editor:UserInformationToolbar"] = typeof(string),
				["Editor:SendPrivateMessageToolbar"] = typeof(string),

				["Images:ImageRequestSizeLimitBytes"] = typeof(int)
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

			DynamicConfigCacheJson = JsonSerializer.Serialize(rez, new JsonSerializerOptions
			{
				WriteIndented = true,
				AllowTrailingCommas = true,
			});
		}
	}
}