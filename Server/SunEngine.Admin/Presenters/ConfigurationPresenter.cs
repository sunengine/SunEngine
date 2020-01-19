using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.Configuration;
using SunEngine.Core.DataBase;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Presenters
{
	public interface IConfigurationPresenter
	{
		Task<IEnumerable<ConfigurationItemView>> LoadConfigurationAsync();
		Dictionary<string, IEnumerable<string>> GetEnums();
	}


	public class ConfigurationPresenter : DbService, IConfigurationPresenter
	{
		public ConfigurationPresenter(DataBaseConnection db) : base(db)
		{
		}

		public async Task<IEnumerable<ConfigurationItemView>> LoadConfigurationAsync()
		{
			var items = await db.ConfigurationItems.OrderBy(x => x.Name).ToDictionaryAsync(x => x.Name, x => x.Value);
			var order = ConfigDefaults.ConfigurationItems.Select(item => item.Key).ToList();

			var rez = new List<ConfigurationItemView>();

			foreach (var name in order)
				if (items.TryGetValue(name, out string value))
					rez.Add(new ConfigurationItemView(name, value));

			return rez;
		}

		public Dictionary<string, IEnumerable<string>> GetEnums()
		{
			var enums = new Dictionary<string, Type>();

			foreach (var configurationItem in ConfigDefaults.ConfigurationItems)
			{
				var type = configurationItem.Value.GetType();
				if (type.IsEnum && !enums.ContainsKey(type.Name))
					enums[type.Name] = type;
			}

			var rez = new Dictionary<string, IEnumerable<string>>();
			foreach (var (name, enum1) in enums)
			{
				rez.Add(name, enum1.GetEnumNames());
			}

			return rez;
		}
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum TypeName
	{
		String,
		LongString,
		JsonString,
		Number,
		Boolean,
		Enum,
		Strange
	}

	public class ConfigurationItemView
	{
		public string Name { get; set; }
		public object Value { get; set; }
		public TypeName Type { get; set; }
		public string EnumName { get; set; }

		public ConfigurationItemView(string name, string value)
		{
			Name = name;

			if (!ConfigDefaults.ConfigurationItems.ContainsKey(name))
				return;

			Type = GetTypeName(ConfigDefaults.ConfigurationItems[name].GetType(), out string enumName);
			this.EnumName = enumName;

			Value = GetTypeObject(Type, name, value);
		}

		protected object GetTypeObject(TypeName typeName, string name, string value)
		{
			switch (typeName)
			{
				case TypeName.Number:
					return int.Parse(value);
				case TypeName.Boolean:
					return bool.Parse(value);
				case TypeName.Enum:
					var type = ConfigDefaults.ConfigurationItems[name].GetType();
					var obj = Enum.Parse(type, value);
					return Enum.GetName(type, obj);
				case TypeName.String:
				case TypeName.LongString:
				case TypeName.JsonString:
				case TypeName.Strange:
				default:
					return value;
			}
		}

		protected TypeName GetTypeName(Type type, out string enumName)
		{
			var nameLastToken = type.Name.Split(".")[^1];

			if (type.IsEnum)
			{
				enumName = nameLastToken;
				return TypeName.Enum;
			}

			enumName = null;

			return nameLastToken switch
			{
				{ } x when new[] {"Int64", "Int32", "int"}.Contains(x) => TypeName.Number,
				{ } x when new[] {"Boolean", "bool"}.Contains(x) => TypeName.Boolean,
				{ } x when new[] {"String", "string"}.Contains(x) => TypeName.String,
				{ } x when x == "LongString" => TypeName.LongString,
				{ } x when x == "JsonString" => TypeName.JsonString,
				{ } => TypeName.Strange
			};
		}
	}
}