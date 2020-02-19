using System;

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
		
		public override Type ToClientType => typeof(bool);

		public BooleanItem(bool value = false, bool configJs = false) : base(configJs)
		{
			Value = value;
		}

		public BooleanItem(string value, bool configJs = false) : base(configJs)
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