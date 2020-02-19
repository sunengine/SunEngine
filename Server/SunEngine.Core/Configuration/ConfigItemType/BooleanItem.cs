namespace SunEngine.Core.Configuration
{
	public class BooleanItem : ConfigItem
	{
		protected bool _value;

		public bool Value
		{
			get => _value;
			set
			{
				_stringValue = value.ToString();
				_objectValue = value;
			}
		}

		public BooleanItem()
		{
			Value = false;
		}
		
		public BooleanItem(bool value)
		{
			Value = value;
		}
		
		public BooleanItem(string value)
		{
			Value = bool.Parse(value);
		}

		public override string StringValue
		{
			get => Value.ToString();
			set => Value = bool.Parse(value);
		}
		
		public static implicit operator BooleanItem(string str)
		{
			return new BooleanItem(bool.Parse(str));
		}

		public static explicit operator string(BooleanItem str)
		{
			return str.ToString();
		}
	}
}