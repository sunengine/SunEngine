using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LinqToDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SunEngine.Core.Configuration;
using SunEngine.Core.DataBase;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Presenters
{
    public interface IConfigurationPresenter
    {
        Task<IEnumerable<ConfigurationItemView>> LoadConfigurationAsync();
    }


    public class ConfigurationPresenter : DbService, IConfigurationPresenter
    {
        public ConfigurationPresenter(DataBaseConnection db) : base(db)
        {
        }

        public async Task<IEnumerable<ConfigurationItemView>> LoadConfigurationAsync()
        {
            var items = await db.ConfigurationItems.OrderBy(x => x.Name).ToListAsync();
            return items.Select(x => new ConfigurationItemView(x.Name,x.Value)).ToList();
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TypeName
    {
        String,
        Number,
        Boolean,
        Strange
    }

    public class ConfigurationItemView
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public TypeName Type { get; set; }

        public ConfigurationItemView(string name, string value)
        {
            Name = name;

            if (ConfigDefaults.ConfigurationItems.ContainsKey(name))
                Type = GetTypeName(ConfigDefaults.ConfigurationItems[name].GetType().Name);

            Value = GetTypeObject(Type, value);
        }

        protected object GetTypeObject(TypeName typeName, string value)
        {
            switch (typeName)
            {
                case TypeName.Number:
                    return int.Parse(value);
                case TypeName.Boolean:
                    return bool.Parse(value);
                case TypeName.String:
                case TypeName.Strange:
                default:
                    return value;
            }
        }

        protected TypeName GetTypeName(string name)
        {
            var nameLastToken = name.Split(".")[^1];
            switch (nameLastToken)
            {
                case "Int64":
                case "Int32":
                case "int":
                    return TypeName.Number;
                case "Boolean":
                case "bool":
                    return TypeName.Boolean;
                case "String":
                case "string":
                    return TypeName.String;
                default:
                    return TypeName.Strange;
            }
        }
    }
}