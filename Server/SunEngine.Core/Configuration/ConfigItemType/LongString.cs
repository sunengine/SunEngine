namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class LongString : StringItem
	{
		public LongString(string value = "") : base(value)
		{
		}

		public LongString(string value, bool configJs) : base(value ?? "", configJs)
		{
		}
	}
}