using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SunEngine.Core.Models;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Cache.CacheModels
{
    public class ComponentServerCached
    {
        public int Id { get; }

        public string Name { get; }

        public string Type { get; }

        public object Data { get; }
        
        public bool IsCacheData { get; }
        
        public IReadOnlyDictionary<int, RoleCached> Roles { get; }

        public ComponentServerCached(Component component, Type type, IReadOnlyDictionary<int, RoleCached> roles)
        {
            Id = component.Id;
            Name = component.Name;
            Type = component.Type;
            IsCacheData = component.IsCacheData;
            Roles = roles;
            Data = JsonConvert.DeserializeObject(component.ServerSettingsJson, type);
        }
    }
    

    public class ComponentClientCached
    {
        public int Id { get; }

        public string Name { get; }

        public string Type { get; }

        public JRaw Settings { get; }
        
        [JsonIgnore] 
        public IReadOnlyDictionary<int, RoleCached> Roles { get; }

        public ComponentClientCached(Component component, IReadOnlyDictionary<int, RoleCached> roles)
        {
            Id = component.Id;
            Name = component.Name;
            Type = component.Type;
            Settings = SunJson.MakeJRow(component.ClientSettingsJson);
            Roles = roles;
        }
    }
}
