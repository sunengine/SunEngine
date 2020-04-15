using System;
using System.Collections.Generic;
using SunEngine.Core.Sections;

namespace SunEngine.Core.Services
{
	public class SectionTypes
	{
		public Dictionary<string, Type> SectionServerTypes { get; } = new Dictionary<string, Type>
		{
			["Posts"] = typeof(PostsServerSection),
			["Activities"] = typeof(ActivitiesServerSection),
			["Articles"] = typeof(MaterialsServerSection)
		};

		public Dictionary<string, Type> SectionClientTypes { get; } = new Dictionary<string, Type>
		{
			["Posts"] = typeof(PostsClientSection),
			["Activities"] = typeof(ActivitiesClientSection),
			["Articles"] = typeof(MaterialsClientSection)
		};
	}
}