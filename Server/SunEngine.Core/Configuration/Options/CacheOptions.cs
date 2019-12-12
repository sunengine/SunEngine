using System;
using SunEngine.Core.Models;

namespace SunEngine.Core.Configuration.Options
{
    public class CacheOptions
    {
        public CachePolicy CurrentCachePolicy { get; private set; }
        public int? InvalidateCacheTime { get; private set; }

        public CacheOptions()
        {
        }

        public CacheOptions(CacheSettings settings) => UpdateOptions(settings);

        public void UpdateOptions(CacheSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings), "Cache settings can`t be null");

            CurrentCachePolicy = settings.CachePolicy;
            InvalidateCacheTime = settings.InvalidateCacheTime;
        }
    }
}
