using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using SunEngine.Core.Models;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Cache.CacheModels
{
	public class MenuItemCached
	{
		public int Id { get; }

		public MenuItemCached Parent { get; }
		public int? ParentId { get; }
		public string Name { get; }
		public string Title { get; }
		public string SubTitle { get; }
		public string RouteName { get; }
		public JsonElement? RouteParamsJson { get; }
		public bool Exact { get; }
		public JsonElement? SettingsJson { get; }
		public string CssClass { get; }
		public string ExternalUrl { get; }
		public bool IsSeparator { get; }
		public int SortNumber { get; }
		public string Icon { get; }
		public string CustomIcon { get; }
		public bool IsHidden { get; }
		[JsonIgnore] public IReadOnlyDictionary<int, RoleCached> Roles { get; }


		public MenuItemCached(MenuItem menuItem, IReadOnlyDictionary<int, RoleCached> roles)
		{
			Id = menuItem.Id;
			ParentId = menuItem.ParentId;
			Name = menuItem.Name;
			Title = menuItem.Title;
			SubTitle = menuItem.SubTitle;
			RouteName = menuItem.RouteName;
			Exact = menuItem.Exact;
			CssClass = menuItem.CssClass;
			ExternalUrl = menuItem.ExternalUrl;
			IsSeparator = menuItem.IsSeparator;
			SortNumber = menuItem.SortNumber;
			Icon = menuItem.Icon;
			IsHidden = menuItem.IsHidden;

			RouteParamsJson = SunJson.MakeJElement(menuItem.RouteParamsJson);
			SettingsJson = SunJson.MakeJElement(menuItem.SettingsJson);

			Roles = roles;
		}
	}
}