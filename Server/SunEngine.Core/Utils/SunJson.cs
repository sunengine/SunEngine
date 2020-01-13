using System.Text.Json;

namespace SunEngine.Core.Utils
{
	public static class SunJson
	{
		public static JsonSerializerOptions DefaultJsonSerializerOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};

		public static string Serialize(object obj)
		{
			return JsonSerializer.Serialize(obj, DefaultJsonSerializerOptions);
		}

		public static JsonElement? MakeJElement(string str)
		{
			if (str != null)
				return JsonDocument.Parse(str).RootElement;
			return null;
		}
	}
}