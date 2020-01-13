using System.Collections.Generic;
using System.Linq;

namespace SunEngine.Core.Cache.Services
{
	public class CacheKeyGenerator
	{
		public string ContentGenerateKey(
			string controllerName, string actionName, IEnumerable<int> categoriesIds, int? page)
		{
			if (categoriesIds == null || !categoriesIds.Any())
				return null;

			return
				$"{controllerName}-{actionName}-{page}:,{string.Join(',', categoriesIds.OrderBy(x => x).Distinct())},";
		}

		public string ContentGenerateKey(string controllerName, string actionName, int? page, int id)
		{
			return $"{controllerName}-{actionName}-{page}:,{id},";
		}

		public string ContentGenerateKey(string componentName, int? page)
		{
			return $"{componentName}-{page}";
		}

		public string ContentGenerateKey(string componentName, IEnumerable<int> categoriesIds, int? page)
		{
			if (categoriesIds == null || !categoriesIds.Any())
				return null;

			return $"{componentName}-{page}:,{string.Join(',', categoriesIds.OrderBy(x => x).Distinct())},";
		}
	}
}