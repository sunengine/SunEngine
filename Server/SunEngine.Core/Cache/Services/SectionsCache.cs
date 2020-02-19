using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.Configuration.Sections;
using SunEngine.Core.Controllers;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;

namespace SunEngine.Core.Cache.Services
{
	public interface ISectionsCache
	{
		SectionServerCached GetSectionserverCached(string name, IReadOnlyDictionary<string, RoleCached> roles);
		IEnumerable<SectionClientCached> GetClientSections(IReadOnlyDictionary<string, RoleCached> roles);
		Dictionary<string, Type> SectionserverTypes { get; }
		Dictionary<string, Type> ComponentClientTypes { get; }
		void Initialize();
	}

	public class SectionsCache : ISectionsCache
	{
		private readonly IDataBaseFactory dataBaseFactory;
		private readonly IRolesCache rolesCache;

		public Dictionary<string, Type> SectionserverTypes { get; } = new Dictionary<string, Type>()
		{
			["Posts"] = typeof(PostsServerComponentData),
			["Activities"] = typeof(ActivitiesServerComponentData)
		};

		public Dictionary<string, Type> ComponentClientTypes { get; } = new Dictionary<string, Type>()
		{
			["Posts"] = typeof(PostsClientComponentData)
		};

		public IReadOnlyDictionary<string, SectionServerCached> ServerSections { get; private set; }
		public IReadOnlyList<SectionClientCached> ClientSections { get; private set; }

		public SectionsCache(IDataBaseFactory dataBaseFactory, IRolesCache rolesCache)
		{
			this.rolesCache = rolesCache;
			this.dataBaseFactory = dataBaseFactory;
			Initialize();
		}

		public SectionServerCached GetSectionserverCached(
			string name, IReadOnlyDictionary<string, RoleCached> roles)
		{
			if (ServerSections.TryGetValue(name, out SectionServerCached SectionServerCached))
				if (SectionServerCached.Roles.Any(x =>
					roles.Keys.Any(role => String.Equals(x.Value.Name, role, StringComparison.OrdinalIgnoreCase))))
					return SectionServerCached;

			return null;
		}

		public IEnumerable<SectionClientCached> GetClientSections(IReadOnlyDictionary<string, RoleCached> roles)
		{
			return ClientSections.Where(comp => roles.Values.Any(role => comp.Roles.ContainsKey(role.Id))).ToArray();
		}

		public void Initialize()
		{
			using var db = dataBaseFactory.CreateDb();
			var Sections = db.Sections.ToList();

			var serverSectionsTmp = new Dictionary<string, SectionServerCached>(Sections.Count);

			var clientSectionsTmp = new List<SectionClientCached>();

			foreach (var component in Sections)
				try
				{
					serverSectionsTmp.Add(component.Name,
						new SectionServerCached(component, SectionserverTypes, rolesCache));
					clientSectionsTmp.Add(new SectionClientCached(component, ComponentClientTypes, rolesCache));
				}
				catch
				{
					// ignored
				}

			ServerSections = serverSectionsTmp.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);
			ClientSections = clientSectionsTmp.ToImmutableList();
		}
	}
}