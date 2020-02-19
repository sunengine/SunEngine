namespace SunEngine.Core.Configuration
{
	public class IntegerItem : ConfigItem
	{
		protected int _value;

		public int Value
		{
			get => _value;
			set
			{
				_stringValue = value.ToString();
				_objectValue = value;
			}
		}
		
		public IntegerItem(int value = 0, bool jsConfig = false) : base(jsConfig)
		{
			Value = value;
		}

		public IntegerItem(string value, bool jsConfig = false) : base(jsConfig)
		{
			Value = int.Parse(value);
		}
		
		public override string StringValue
		{
			get => Value.ToString();
			set => Value = int.Parse(value);
		}
	}
}