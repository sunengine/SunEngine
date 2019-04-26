using System;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SunEngine.Commons.Misc;

namespace SunEngine.Commons.Utils
{
    public static class WebJson
    {
        public static JsonSerializerSettings jsonSettings { get; } = new JsonSerializerSettings
        {
            ContractResolver = SunJsonContractResolver.Instance,
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

                if (string.Equals(property.PropertyName, "IsSoft", StringComparison.OrdinalIgnoreCase))
                    property.ShouldSerialize = instance => (instance as ErrorObject).IsSoft;
            }


            return property;
        }
    }
}