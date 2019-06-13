using System.Collections.Generic;
using Newtonsoft.Json;
using SunEngine.Core.Models;

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
        public object RouteParamsJson { get; }
        public bool Exact { get; }
        public object SettingsJson { get; }
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
            
            try
            {
                RouteParamsJson = JsonConvert.DeserializeObject<object>(menuItem.RouteParamsJson);
            }
            catch
            {
            }
            try
            {
                SettingsJson = JsonConvert.DeserializeObject<object>(menuItem.SettingsJson);
            }
            catch
            {
            }
            
            Roles = roles;
        }
    }
}
