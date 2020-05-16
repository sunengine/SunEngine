using System;
using System.Collections.Generic;
using System.Linq;
using SunEngine.Core.Models;
using SunEngine.Core.Sections;

namespace SunEngine.Core.Services
{
	public class SectionTypes
	{
		public readonly Dictionary<string, SectionDescriptor> Sections = new List<SectionDescriptor>
		{
			new SectionDescriptor("Posts", typeof(PostsServerSection), typeof(PostsClientSection),
				SectionsGroupsNames.Pages),
			new SectionDescriptor("Activities", typeof(ActivitiesServerSection),
				typeof(ActivitiesClientSection), SectionsGroupsNames.Pages),
			new SectionDescriptor("Articles", typeof(MaterialsServerSection),
				typeof(MaterialsClientSection), SectionsGroupsNames.Pages),
		}.ToDictionary(x => x.Name, x => x);
	}

	public class SectionDescriptor
	{
		public string Name { get; }
		public Type ClientSectionType { get; }
		public Type ServerSectionType { get; }
		public string SectionGroup { get; }

		public SectionDescriptor(string name, Type serverSectionType, Type clientSectionType, string sectionGroup)
		{
			Name = name;
			ClientSectionType = clientSectionType;
			ServerSectionType = serverSectionType;
			SectionGroup = sectionGroup;
		}
	}
}