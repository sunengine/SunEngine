using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Errors;

namespace SunEngine.Core.Services
{
    public class SanitizerOptionsService
    {
        private const string DefaultCategory = "Default";

        private readonly IConfiguration configuration;
        private readonly Dictionary<string, SanitizerOptions> optionCategories
            = new Dictionary<string, SanitizerOptions>();

        public SanitizerOptionsService(IConfiguration configuration)
        {
            this.configuration = configuration;
            LoadOptions();
        }

        public SanitizerOptions GetOptions(string categoryName = DefaultCategory)
        {
            if (optionCategories.ContainsKey(categoryName))
                return optionCategories[categoryName];
            
            throw new SunException($"Not found sanitizer options with {nameof(categoryName)}={categoryName}");
        }

        private void LoadOptions()
        {
            var sections = configuration.GetSection("Sanitizer").GetChildren();
            foreach (var section in sections)
            {
                var key = section.Key;
                var value = section.Get<SanitizerOptions>();
                optionCategories.Add(key, value);
            }
        }
    }
}