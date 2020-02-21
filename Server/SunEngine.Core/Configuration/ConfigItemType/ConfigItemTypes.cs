using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SunEngine.Core.Configuration.ConfigItemType
{
	public static class ConfigItemTypes
	{
		public readonly static IReadOnlyDictionary<string, object> Types;

		static ConfigItemTypes()
		{
			Dictionary<string, object> types = new Dictionary<string, object>();

			foreach (var domainAssembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var type in domainAssembly.GetTypes()
					.Where(type => type.IsAssignableFrom(typeof(IConfigItem)) && !type.IsAbstract))
				{
					types.Add(type.Name.Split(".")[^1], type);
				}
			}

			Types = types.ToImmutableDictionary();
		}
	}
}