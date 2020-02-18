namespace SunEngine.Core.Configuration
{
	public abstract class ConfigItem
	{
		protected string _value { get; set; }

		public string Value
		{
			get { return _value; }
			set { _value = value ?? ""; }
		}

		public virtual bool Validate()
		{
			return true;
		}
		
		public virtual void Prepare()
		{
		}
	}
}