using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
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

		public JsonElement? Options { get; }

		[JsonIgnore] public IReadOnlyDictionary<int, RoleCached> Roles { get; }

		public ComponentClientCached(Component component, Dictionary<string, Type> clientComponentTypes,
			IRolesCache rolesCache)
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
				Roles = new Dictionary<int, RoleCached>().ToImmutableDictionary();

			if (clientComponentTypes.TryGetValue(Type, out Type type))
			{
				object clientData = JsonSerializer.Deserialize(component.Options, type);
				string clientJson = JsonSerializer.Serialize(clientData);
				Options = SunJson.MakeJElement(clientJson);
			}
		}
	}
}