namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class IntegerItem : ConfigItem<int>
	{
		public IntegerItem(int value = 0) : base(value)
		{
		}

		public IntegerItem(string value) : base(int.Parse(value))
		{
		}
		
		public IntegerItem(int value, bool configJs) : base(value, configJs)
		{
		}

		public IntegerItem(string value, bool configJs) : base(int.Parse(value), configJs)
		{
		}
		
		public override void FromString(string value)
		{
			Value = int.Parse(value);
		}
	}
}