using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using SunEngine.Admin.Presenters;
using SunEngine.Core.Configuration;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Services
{
    public class ConfigurationAdminService : DbService
    {
        protected string WwwRootPath { get; }
        protected IHostingEnvironment env { get; }

        public ConfigurationAdminService(
            DataBaseConnection db,
            IPathService pathService,
            IHostingEnvironment env) : base(db)
        {
            this.env = env;
            WwwRootPath = pathService.WwwRootDir;
        }

        public void UpdateClientScripts()
        {
            var list = new List<string>()
            {
                "Global:SiteName",
                "Comments:TimeToOwnEditInMinutes",
                "Comments:TimeToOwnDeleteInMinutes",
                "Materials:CommentsPageSize",
                "Materials:TimeToOwnEditInMinutes",
                "Materials:TimeToOwnDeleteInMinutes",
            };

            var values = db.ConfigurationItems.Where(x => list.Contains(x.Name))
                .Select(x => new ConfigurationConfigItemView(x));

            var rez = new Dictionary<string, object>();
            foreach (var itemView in values)
            {
                string[] tokens = itemView.Name.Split(":");

                Dictionary<string, object> current = rez;
                for (int i = 0; i < tokens.Length - 1; i++)
                {
                    if (!current.ContainsKey(tokens[i]))
                        current[tokens[i]] = new Dictionary<string, object>();

                    current = (Dictionary<string, object>) current[tokens[i]];
                }

                current[tokens[^1]] = itemView.Value;
            }


            var json = JsonSerializer.Serialize(rez, new JsonSerializerOptions
            {
                WriteIndented = true,
                AllowTrailingCommas = true,
            });

            var variablesJsPath = Path.Combine(WwwRootPath, "variables.js");

            json = json.Substring(1, json.Length - 2) + ",";

            var variablesJs = File.ReadAllText(variablesJsPath);
            variablesJs = Regex.Replace(variablesJs, "//( *?)auto-start(.*?)//( *?)auto-end",
                $"// auto-start\n{json}\n // auto-end", RegexOptions.Singleline);
            File.WriteAllText(variablesJsPath, variablesJs);

            UpdateVersion();
        }

        public void UpdateVersion()
        {
            if (env.IsProduction())
            {
                var ran = new Random();
                
                var indexHtmlPath = Path.Combine(WwwRootPath, "index.html");
                string text = File.ReadAllText(indexHtmlPath);
                Regex reg2 = new Regex("configver=\\d+\"");
                text = reg2.Replace(text, $"configver={ran.Next()}\"");
                Regex reg3 = new Regex("variablesver=\\d+\"");
                text = reg3.Replace(text, $"variablesver={ran.Next()}\"");
                File.WriteAllText(indexHtmlPath, text);
            }
        }
    }

    public class ConfigurationConfigItemView
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public ConfigurationConfigItemView(string name, string value)
        {
            Name = name;

            if (!ConfigDefaults.ConfigurationItems.ContainsKey(name))
                return;

            var type = GetTypeName(ConfigDefaults.ConfigurationItems[name].GetType(), out string enumName);

            Value = GetTypeObject(type, name, value);
        }

        public ConfigurationConfigItemView(ConfigurationItem ci)
        {
            Name = ci.Name;

            if (!ConfigDefaults.ConfigurationItems.ContainsKey(Name))
                return;

            var type = GetTypeName(ConfigDefaults.ConfigurationItems[Name].GetType(), out string enumName);

            Value = GetTypeObject(type, Name, ci.Value);
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
                { } => TypeName.Strange
            };
        }
    }
}