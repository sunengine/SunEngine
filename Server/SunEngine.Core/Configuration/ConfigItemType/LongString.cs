using System;

namespace SunEngine.Core.Configuration
{
	public class LongString : ConfigItem
	{
		public LongString(string value = "", bool configJs = false) : base(configJs)
		{
			StringValue = value;
		}
	}
}