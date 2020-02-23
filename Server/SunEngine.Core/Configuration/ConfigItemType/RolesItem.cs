namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class RolesItem : StringItem
	{
		public RolesItem(string value = "") : base(value)
		{
		}

		public RolesItem(string value, bool configJs) : base(value ?? "", configJs)
		{
		}
	}
}