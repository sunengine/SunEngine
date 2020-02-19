using System;
using System.Reflection;
using LinqToDB.Extensions;

namespace SunEngine.Core.Configuration
{
	public abstract class ConfigItem 
	{
		protected object _objectValue;
		public virtual object ObjectValue => _objectValue;

		protected string _stringValue;

		/// <summary>
		/// Use in JavaScript Config
		/// </summary>
		public bool ConfigJs = false;
		
		/// <summary>
		/// Use in Dynamic Config
		/// </summary>
		public bool Dynamic => !ConfigJs;

		public virtual Type ToClientType => typeof(string);

		public ConfigItem(bool configJs = false)
		{
			ConfigJs = configJs;
		}
		
		public ConfigItem ShallowCopy()
		{
			return (ConfigItem)MemberwiseClone();
		}

		public string GetTypeName()
		{
			return _objectValue.GetType().Name.Split(".")[^1];
		}

		public virtual string StringValue
		{
			get => _stringValue;
			set {
				_objectValue = _stringValue = value; 
			}
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