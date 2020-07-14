using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.DataBase;
using SunEngine.Core.Services;

namespace SunEngine.Core.Cache.Services
{
	public interface ISectionsCache
	{
		SectionServerCached GetSectionServerCached(string name, IReadOnlyDictionary<string, RoleCached> roles);
		T GetServerSectionData<T>(string name, IReadOnlyDictionary<string, RoleCached> roles) where T : class;
		IEnumerable<SectionClientCached> GetClientSections(IReadOnlyDictionary<string, RoleCached> roles);
		void Initialize();
	}

	public class SectionsCache : ISectionsCache
	{
		private readonly IDataBaseFactory dataBaseFactory;
		private readonly IRolesCache rolesCache;
		private readonly SectionTypes sectionTypes;

		public IReadOnlyDictionary<string, SectionServerCached> ServerSections { get; private set; } 
		public IReadOnlyList<SectionClientCached> ClientSections { get; private set; }

		public SectionsCache(
			IDataBaseFactory dataBaseFactory,
			IRolesCache rolesCache,
			SectionTypes sectionTypes)
		{
			this.rolesCache = rolesCache;
			this.dataBaseFactory = dataBaseFactory;
			this.sectionTypes = sectionTypes;
			Initialize();
		}

		public SectionServerCached GetSectionServerCached(
			string name, IReadOnlyDictionary<string, RoleCached> roles)
		{
			if (ServerSections.TryGetValue(name, out SectionServerCached SectionServerCached))
				if (SectionServerCached.Roles.Any(x =>
					roles.Keys.Any(role => String.Equals(x.Value.Name, role, StringComparison.OrdinalIgnoreCase))))
					return SectionServerCached;

			return null;
		}

		public T GetServerSectionData<T>(string name, IReadOnlyDictionary<string, RoleCached> roles) where T: class
		{
			var section = GetSectionServerCached(name, roles);
			return section?.Data as T;
		}

		public IEnumerable<SectionClientCached> GetClientSections(IReadOnlyDictionary<string, RoleCached> roles)
		{
			return ClientSections.Where(comp => roles.Values.Any(role => comp.Roles.ContainsKey(role.Id))).ToArray();
		}

		public void Initialize()
		{
			using var db = dataBaseFactory.CreateDb();
			var sections = db.Sections.ToList();

			var serverSectionsTmp = new Dictionary<string, SectionServerCached>(sections.Count);

			var clientSectionsTmp = new List<SectionClientCached>();

			foreach (var section in sections)
				try
				{
					serverSectionsTmp.Add(section.Name,
						new SectionServerCached(section, sectionTypes.Sections[section.Name].ServerSectionType, rolesCache));
					clientSectionsTmp.Add(new SectionClientCached(section, sectionTypes.Sections[section.Name].ClientSectionType, rolesCache));
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