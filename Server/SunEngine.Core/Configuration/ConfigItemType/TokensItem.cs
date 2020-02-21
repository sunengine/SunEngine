namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class TokensItem : StringItem
	{
		public TokensItem(string value = "") : base(value)
		{
		}

		public TokensItem(string value, bool configJs) : base(value ?? "", configJs)
		{
		}
	}
}