using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SunEngine.Commons.Utils
{
    public static class WebJson
    {
        public static JsonSerializerSettings jsonSettings { get; } = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            MissingMemberHandling = MissingMemberHandling.Ignore,
            MaxDepth = 32,
            TypeNameHandling = TypeNameHandling.None,
            NullValueHandling = NullValueHandling.Ignore
        };

        public static string Serialize(object obj)
        {
           return JsonConvert.SerializeObject(obj, jsonSettings);
        }
    }
}