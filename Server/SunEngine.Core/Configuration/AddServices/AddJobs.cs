using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Services;

namespace SunEngine.Core.Configuration.AddServices
{
	public static class AddJobsExtensions
	{
		public static void AddJobs(this IServiceCollection services)
		{
			services.AddHostedService<CleanCacheJobsService>();
		}
	}
}