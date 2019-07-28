using Newtonsoft.Json.Linq;

namespace SunEngine.Core.Cache.CacheModels
{
    public class ComponentServerCached
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public object Data { get; set; }
    }
    
    public class ComponentClientCached
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public JRaw Settings { get; set; }
    }
}
