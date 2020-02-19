using System;

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
		
		public override Type ToClientType => typeof(int);

		public IntegerItem(int value = 0, bool configJs = false) : base(configJs)
		{
			Value = value;
		}

		public IntegerItem(string value, bool configJs = false) : base(configJs)
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