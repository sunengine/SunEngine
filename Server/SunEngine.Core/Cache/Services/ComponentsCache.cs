using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.Controllers;
using SunEngine.Core.DataBase;

namespace SunEngine.Core.Cache.Services
{
	public interface IComponentsCache
	{
		ComponentServerCached GetComponentServerCached(string name, IReadOnlyDictionary<string, RoleCached> roles);
		IEnumerable<ComponentClientCached> GetClientComponents(IReadOnlyDictionary<string, RoleCached> roles);
		Dictionary<string, Type> ComponentsDataTypes { get; }
		void Initialize();
	}

	public class ComponentsCache : IComponentsCache
	{
		private readonly IDataBaseFactory dataBaseFactory;
		private readonly IRolesCache rolesCache;

		public Dictionary<string, Type> ComponentsDataTypes { get; } = new Dictionary<string, Type>()
		{
			["Posts"] = typeof(PostsComponentData),
			["Activities"] = typeof(ActivitiesComponentData)
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

		public IEnumerable<ComponentClientCached> GetClientComponents(IReadOnlyDictionary<string, RoleCached> Roles)
		{
			return ClientComponents.Where(comp => Roles.Values.Any(role => comp.Roles.ContainsKey(role.Id))).ToArray();
		}

		public void Initialize()
		{
			using var db = dataBaseFactory.CreateDb();
			var components = db.Components.ToList();

			Dictionary<string, ComponentServerCached> serverComponentsTmp =
				new Dictionary<string, ComponentServerCached>(components.Count);

			List<ComponentClientCached> clientComponentsTmp = new List<ComponentClientCached>();

			foreach (var component in components)
			{
				try
				{
					ImmutableDictionary<int, RoleCached> roles;
					if (component.Roles != null)
					{
						roles = component.Roles.Split(',')
							.Select(x => rolesCache.GetRole(x))
							.ToDictionary(x => x.Id, x => x)
							.ToImmutableDictionary();
					}
					else
					{
						roles = new Dictionary<int, RoleCached>().ToImmutableDictionary();
					}

					serverComponentsTmp[component.Name] =
						new ComponentServerCached(component, ComponentsDataTypes[component.Type], roles);

					clientComponentsTmp.Add(new ComponentClientCached(component, roles));
				}
				catch
				{
					// ignored
				}
			}

			ServerComponents = serverComponentsTmp.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);

			ClientComponents = clientComponentsTmp.ToImmutableList();
		}
	}
}