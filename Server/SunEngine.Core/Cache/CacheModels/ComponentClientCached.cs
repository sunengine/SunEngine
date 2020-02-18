using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Models;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Cache.CacheModels
{
	public class ComponentClientCached
	{
		public int Id { get; }

		public string Name { get; }

		public string Type { get; }

		public JsonElement? Settings { get; }

		[System.Text.Json.Serialization.JsonIgnore]
		public IReadOnlyDictionary<int, RoleCached> Roles { get; }

		public ComponentClientCached(Component component, Type type,  IRolesCache rolesCache)
		{
			Id = component.Id;
			Name = component.Name;
			Type = component.Type;
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
			object clientData = JsonSerializer.Deserialize(component.JsonOptions, type);
			string clientJson = JsonSerializer.Serialize(clientData);
			Settings = SunJson.MakeJElement(clientJson);
		}
	}
}