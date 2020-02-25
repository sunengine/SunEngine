using System;
using System.Collections.Generic;
using SunEngine.Core.SectionsData;

namespace SunEngine.Core.Cache.Services
{
	public class SectionTypes
	{
		public Dictionary<string, Type> SectionServerTypes { get; } = new Dictionary<string, Type>()
		{
			["Posts"] = typeof(PostsServerSectionData),
			["Activities"] = typeof(ActivitiesServerSectionData)
		};

		public Dictionary<string, Type> SectionClientTypes { get; } = new Dictionary<string, Type>()
		{
			["Posts"] = typeof(PostsClientSectionData),
			["Activities"] = typeof(ActivitiesClientSectionData)
		};
	}
}