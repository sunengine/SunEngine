using System;
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

        public ComponentServerCached(Component component, Type type)
        {
            Id = component.Id;
            Name = component.Name;
            Type = component.Type;
            Data = JsonConvert.DeserializeObject(component.ServerSettingsJson, type);
        }
    }

    public class ComponentClientCached
    {
        public int Id { get; }

        public string Name { get; }

        public string Type { get; }

        public JRaw Settings { get; }

        public ComponentClientCached(Component component)
        {
            Id = component.Id;
            Name = component.Name;
            Type = component.Type;
            Settings = SunJson.MakeJRow(component.ClientSettingsJson);
        }
    }
}
