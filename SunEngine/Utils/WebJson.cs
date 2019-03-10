using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SunEngine.Utils
{
    public static class WebJson
    {
        private static JsonSerializerSettings jsonSettings { get; }

        static WebJson()
        {
            jsonSettings = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                MissingMemberHandling = MissingMemberHandling.Ignore,
                MaxDepth = 32,
                TypeNameHandling = TypeNameHandling.None
            };
        }

        public static string Serialize<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                using (TextWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    using (JsonTextWriter jsonTextWriter = new JsonTextWriter(writer))
                    {
                        jsonTextWriter.CloseOutput = false;
                        jsonTextWriter.AutoCompleteOnClose = false;
                        JsonSerializer.Create(jsonSettings).Serialize(jsonTextWriter, obj);
                    }

                    writer.Flush();
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}