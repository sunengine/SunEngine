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
	public class ComponentServerCached
	{
		public int Id { get; }

		public string Name { get; }

		public Object Data { get; }

		public bool IsCacheData { get; }

		public IReadOnlyDictionary<int, RoleCached> Roles { get; }

		public ComponentServerCached(Component component, Dictionary<string, Type> serverTypes, IRolesCache rolesCache)
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
				Roles = new Dictionary<int, RoleCached>().ToImmutableDictionary();

			if(!serverTypes.TryGetValue(component.Type,out Type type))
				throw new SunException("No component type found: "+component.Type);
			
			Data = JsonSerializer.Deserialize(component.Options,type);
		}
	}
}