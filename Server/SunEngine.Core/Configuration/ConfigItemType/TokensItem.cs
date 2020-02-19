using System;

namespace SunEngine.Core.Configuration
{
	public class TokensItem : ConfigItem
	{
		public TokensItem(string value = "", bool jsConfig = false) : base(jsConfig)
		{
			_objectValue = _stringValue = value;
		}
	}
}