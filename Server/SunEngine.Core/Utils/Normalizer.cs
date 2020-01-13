using Microsoft.AspNetCore.Identity;

namespace SunEngine.Core.Utils
{
	public static class Normalizer
	{
		public static string Normalize(string key)
		{
			return key.ToLower();
		}
	}

	public class NormalizerLookup : ILookupNormalizer
	{
		public static NormalizerLookup Instance = new NormalizerLookup();

		public string NormalizeEmail(string email)
		{
			return Normalizer.Normalize(email);
		}

		public string NormalizeName(string name)
		{
			return Normalizer.Normalize(name);
		}
	}
}