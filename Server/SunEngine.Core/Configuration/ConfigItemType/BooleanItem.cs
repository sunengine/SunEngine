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

		public BooleanItem(bool value = false, bool jsConfig = false) : base(jsConfig)
		{
			Value = value;
		}

		public BooleanItem(string value, bool jsConfig = false) : base(jsConfig)
		{
			Value = bool.Parse(value);
		}

		public override string StringValue
		{
			get => Value.ToString();
			set => Value = bool.Parse(value);
		}
	}
}