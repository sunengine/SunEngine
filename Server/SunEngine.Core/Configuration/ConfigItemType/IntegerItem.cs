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

		public IntegerItem()
		{
			Value = 0;
		}

		public IntegerItem(int value)
		{
			Value = value;
		}

		public IntegerItem(string value)
		{
			Value = int.Parse(value);
		}

		public override string StringValue
		{
			get => Value.ToString();
			set => Value = int.Parse(value);
		}

		public static implicit operator IntegerItem(string str)
		{
			return new IntegerItem(int.Parse(str));
		}

		public static explicit operator string(IntegerItem str)
		{
			return str.ToString();
		}
	}
}