using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.Controllers;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;

namespace SunEngine.Core.Cache.Services
{
	public interface IComponentsCache
	{
		ComponentServerCached GetComponentServerCached(string name, IReadOnlyDictionary<string, RoleCached> roles);
		IEnumerable<ComponentClientCached> GetClientComponents(IReadOnlyDictionary<string, RoleCached> roles);
		Dictionary<string, Type> ComponentServerTypes { get; }
		Dictionary<string, Type> ComponentClientTypes { get; }
		void Initialize();
	}

	public class ComponentsCache : IComponentsCache
	{
		private readonly IDataBaseFactory dataBaseFactory;
		private readonly IRolesCache rolesCache;

		public Dictionary<string, Type> ComponentServerTypes { get; } = new Dictionary<string, Type>()
		{
			["Posts"] = typeof(PostsServerComponentData),
			["Activities"] = typeof(ActivitiesServerComponentData)
		};

		public Dictionary<string, Type> ComponentClientTypes { get; } = new Dictionary<string, Type>()
		{
			["Posts"] = typeof(PostsClientComponentData)
		};

		public IReadOnlyDictionary<string, ComponentServerCached> ServerComponents { get; private set; }
		public IReadOnlyList<ComponentClientCached> ClientComponents { get; private set; }

		public ComponentsCache(IDataBaseFactory dataBaseFactory, IRolesCache rolesCache)
		{
			this.rolesCache = rolesCache;
			this.dataBaseFactory = dataBaseFactory;
			Initialize();
		}

		public ComponentServerCached GetComponentServerCached(
			string name, IReadOnlyDictionary<string, RoleCached> roles)
		{
			if (ServerComponents.TryGetValue(name, out ComponentServerCached componentServerCached))
				if (componentServerCached.Roles.Any(x =>
					roles.Keys.Any(role => String.Equals(x.Value.Name, role, StringComparison.OrdinalIgnoreCase))))
					return componentServerCached;

			return null;
		}

		public IEnumerable<ComponentClientCached> GetClientComponents(IReadOnlyDictionary<string, RoleCached> roles)
		{
			return ClientComponents.Where(comp => roles.Values.Any(role => comp.Roles.ContainsKey(role.Id))).ToArray();
		}

		public void Initialize()
		{
			using var db = dataBaseFactory.CreateDb();
			var components = db.Components.ToList();

			var serverComponentsTmp = new Dictionary<string, ComponentServerCached>(components.Count);

			var clientComponentsTmp = new List<ComponentClientCached>();

			foreach (var component in components)
				try
				{
					serverComponentsTmp.Add(component.Name,
						new ComponentServerCached(component, ComponentServerTypes, rolesCache));
					clientComponentsTmp.Add(new ComponentClientCached(component, ComponentClientTypes, rolesCache));
				}
				catch
				{
					// ignored
				}

			ServerComponents = serverComponentsTmp.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);
			ClientComponents = clientComponentsTmp.ToImmutableList();
		}
	}
}