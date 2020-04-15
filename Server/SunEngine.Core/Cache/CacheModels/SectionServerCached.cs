using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Errors.Exceptions;
using SunEngine.Core.Models;

namespace SunEngine.Core.Cache.CacheModels
{
	public class SectionServerCached
	{
		public int Id { get; }

		public string Name { get; }

		public Object Data { get; }

		public T GetData<T>() where T : class
		{
			return Data as T;
		}

		public bool IsCacheData { get; }

		public IReadOnlyDictionary<int, RoleCached> Roles { get; }

		public SectionServerCached(Section section, Dictionary<string, Type> serverTypes, IRolesCache rolesCache)
		{
			Id = section.Id;
			Name = section.Name;
			IsCacheData = section.IsCacheData;
			if (section.Roles != null)
			{
				Roles = section.Roles.Split(',')
					.Select(x => rolesCache.GetRole(x))
					.ToDictionary(x => x.Id, x => x)
					.ToImmutableDictionary();
			}
			else
				Roles = new Dictionary<int, RoleCached>().ToImmutableDictionary();

			if (!serverTypes.TryGetValue(section.Type, out Type type))
				throw new SunException("No component type found: " + section.Type);

			Data = JsonSerializer.Deserialize(section.Options, type);
		}
	}
}