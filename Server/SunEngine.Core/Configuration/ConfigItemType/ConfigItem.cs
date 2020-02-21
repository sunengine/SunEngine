using System;

namespace SunEngine.Core.Configuration.ConfigItemType
{
	public interface IConfigItem 
	{
		/// <summary>
		/// Use in JavaScript Config
		/// </summary>
		bool ConfigJs { get; }

		/// <summary>
		/// Use in Dynamic Config
		/// </summary>
		bool Dynamic => !ConfigJs;

		Type ToClientType();

		object ToClientObject();

		IConfigItem ShallowCopy();
	
		void FromString(string value); 
		
		//public abstract string GetTypeName();

		public bool Validate()
		{
			return true;
		}

		public void Prepare()
		{
		}
	}

	public abstract class ConfigItem<T> : IConfigItem
	{
		public T Value { get; set; }
		public bool ConfigJs  { get; set; }
		
		public virtual Type ToClientType()
		{
			return typeof(T);
		}

		public virtual object ToClientObject()
		{
			return Value;
		}

		public abstract void FromString(string value);

		public ConfigItem(bool configJs = false)
		{
			ConfigJs = configJs;
		}
		
		public ConfigItem(T value)
		{
			Value = value;
			ConfigJs = false;
		}
		
		public ConfigItem(T value, bool configJs)
		{
			Value = value;
			ConfigJs = configJs;
		}
		
		
		public IConfigItem ShallowCopy()
		{
			return (IConfigItem)MemberwiseClone();
		}
	}
}