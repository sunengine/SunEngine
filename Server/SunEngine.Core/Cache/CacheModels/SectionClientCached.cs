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
	public class SectionClientCached
	{
		public int Id { get; }

		public string Name { get; }

		public string Type { get; }

		public JsonElement? Options { get; }

		[JsonIgnore] public IReadOnlyDictionary<int, RoleCached> Roles { get; }

		public SectionClientCached(Section section, Type clientSectionType,
			IRolesCache rolesCache)
		{
			Id = section.Id;
			Name = section.Name;
			Type = section.Type;
			if (section.Roles != null)
			{
				Roles = section.Roles.Split(',')
					.Select(x => rolesCache.GetRole(x))
					.ToDictionary(x => x.Id, x => x)
					.ToImmutableDictionary();
			}
			else
				Roles = new Dictionary<int, RoleCached>().ToImmutableDictionary();

			object clientData = JsonSerializer.Deserialize(section.Options, clientSectionType);
			string clientJson = JsonSerializer.Serialize(clientData);
			Options = SunJson.MakeJElement(clientJson);
		}
	}
}