using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SunEngine.Core.Models;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Cache.CacheModels
{
    public class ComponentServerCached
    {
        public string Name { get; }

        public string Type { get; }

        public object Data { get; }
        
        public bool IsCacheData { get; }

        public ComponentServerCached(Component component, Type type)
        {
            Name = component.Name;
            Type = component.Type;
            IsCacheData = component.IsCacheData;
            Data = JsonConvert.DeserializeObject(component.ServerSettingsJson, type);
        }
    }

    public class ComponentClientCached
    {
        public string Name { get; }

        public string Type { get; }

        public JRaw Settings { get; }

        public ComponentClientCached(Component component)
        {
            Name = component.Name;
            Type = component.Type;
            Settings = SunJson.MakeJRow(component.ClientSettingsJson);
        }
    }
}
