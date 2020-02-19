using System;

namespace SunEngine.Core.Configuration
{
	public class LongString : ConfigItem
	{
		public LongString(string value = "", bool jsConfig = false) : base(jsConfig)
		{
			StringValue = value;
		}
	}
}