using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Models;

namespace SunEngine.Core.Cache.CacheModels
{
	public class ComponentServerCached<T> where T: class
	{
		public int Id { get; }

		public string Name { get; }

		public T Data { get; }

		public bool IsCacheData { get; }

		public IReadOnlyDictionary<int, RoleCached> Roles { get; }

		public ComponentServerCached(Component component, IRolesCache rolesCache)
		{
			Id = component.Id;
			Name = component.Name;
			IsCacheData = component.IsCacheData;
			if (component.Roles != null)
			{
				Roles = component.Roles.Split(',')
					.Select(x => rolesCache.GetRole(x))
					.ToDictionary(x => x.Id, x => x)
					.ToImmutableDictionary();
			}
			else
			{
				Roles = new Dictionary<int, RoleCached>().ToImmutableDictionary();
			}
			Data = JsonSerializer.Deserialize(component.JsonOptions, type);
		}
	}
}