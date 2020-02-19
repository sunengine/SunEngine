using System;

namespace SunEngine.Core.Configuration
{
	public class StringItem : ConfigItem
	{
		public StringItem(string value = "", bool jsConfig = false) : base(jsConfig)
		{
			_objectValue = _stringValue = value;
		}
	}
}