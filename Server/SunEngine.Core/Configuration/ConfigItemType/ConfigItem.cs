using System.Reflection;
using LinqToDB.Extensions;

namespace SunEngine.Core.Configuration
{
	public abstract class ConfigItem 
	{
		protected object _objectValue;
		public virtual object ObjectValue => _objectValue;

		public string _stringValue;

		/// <summary>
		/// Use in Dynamic Config
		/// </summary>
		public bool Dynamic = true;

		/// <summary>
		/// Use in JavaScript Config
		/// </summary>
		public bool JsConfig = false;

		public ConfigItem()
		{

		}

		public ConfigItem(bool jsConfig = false)
		{
			JsConfig = jsConfig;
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