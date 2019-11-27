using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Cache.Services.Counters;

namespace SunEngine.Core.Configuration.AddServices
{
    public static class AddCountersExtensions
    {
        public static void AddCounters(this IServiceCollection services)
        {
            services.AddSingleton<IMaterialsVisitsCounterCache, MaterialsVisitsCounterCache>();
            services.AddSingleton<IProfilesVisitsCounterService, ProfilesVisitsCounterService>();
        }
    }
}
