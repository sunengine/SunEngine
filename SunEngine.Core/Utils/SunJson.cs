using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SunEngine.Core.Errors;

namespace SunEngine.Core.Utils
{
    public static class SunJson
    {
        public static JsonSerializerSettings jsonSettings { get; } = new JsonSerializerSettings
        {
            ContractResolver = SunJsonContractResolver.Instance,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            MaxDepth = 32,
            TypeNameHandling = TypeNameHandling.None,
            NullValueHandling = NullValueHandling.Ignore, 
        };

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, jsonSettings);
        }
        
        public static JRaw MakeJRow(string json)
        {
            if (json == null)
                return null;

            try
            {
                return new JRaw(json);
            }
            catch
            {
                return null;
            }
        }
    }

    public class SunJsonContractResolver : CamelCasePropertyNamesContractResolver
    {
        public static readonly SunJsonContractResolver Instance = new SunJsonContractResolver();

        public static bool ShowExceptions = false;

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == typeof(ErrorObject))
            {
                if (string.Equals(property.PropertyName, "Message", StringComparison.OrdinalIgnoreCase))
                    property.ShouldSerialize =
                        instance => ShowExceptions || (instance as ErrorObject).StackTrace == null;

                if (string.Equals(property.PropertyName, "StackTrace", StringComparison.OrdinalIgnoreCase))
                    property.ShouldSerialize = instance => ShowExceptions;
            }

            return property;
        }
    }
}
