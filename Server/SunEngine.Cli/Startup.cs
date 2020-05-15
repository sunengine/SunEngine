using System;
using System.Text.Json;
using LinqToDB.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SunEngine.Admin;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.AddServices;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Errors;
using SunEngine.Core.Security;
using SunEngine.Core.Services;
using SunEngine.Materials;

namespace SunEngine.Cli
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			CurrentEnvironment = env;
		}

		private IConfiguration Configuration { get; }

		private IWebHostEnvironment CurrentEnvironment { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			if (Configuration.GetSection("Cors") != null)
				services.AddCors();

			services.AddOptions(Configuration);

			services.AddDatabase(Configuration, out var dataBaseFactory);

			services.AddCaches(dataBaseFactory);

			services.AddCachePolicy();

			services.AddIdentity(Configuration, dataBaseFactory);

			services.AddSunAuthentication();

			services.AddSunAuthorization();

			services.AddManagers();

			services.AddPresenters();

			services.AddMaterials();

			services.AddAdmin();

			services.AddMemoryCache();

			services.AddImagesServices();

			services.AddCiphers(dataBaseFactory);

			services.AddCounters();

			services.AddJobs();

			services.AddSingleton<CaptchaService>();

			services.AddSingleton<SectionTypes>();

			services.AddSanitizer();

			services.AddTransient<IEmailSenderService, EmailSenderService>();

			services.AddSingleton<IDynamicConfigCache, DynamicConfigCache>();

			services.AddSingleton<IPathService, PathService>();

			services.AddSingleton((IConfigurationRoot) Configuration);

			services.AddMvcCore(options =>
				{
					// Add filters here
				})
				.AddApiExplorer()
				.AddAuthorization()
				.AddJsonOptions(options =>
					{
						options.JsonSerializerOptions.WriteIndented = false;
						options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
					}
				);
		}


		public void Configure(IApplicationBuilder app, IOptionsMonitor<ServerOptions> serverOptions)
		{
			if (!CurrentEnvironment.IsDevelopment())
				app.UseHsts();

			if (serverOptions.CurrentValue.FileServer)
				app.UseStaticFiles();

			app.UseCookiePolicy();


			if (Configuration.GetSection("Cors") != null)
			{
				app.UseCors(builder =>
				{
					if (Configuration.GetValue<bool>("Cors:AllowAnyOrigin"))
						builder.AllowAnyOrigin();

					var origins = Configuration.GetValue<string>("Cors:Origins");
					if (!string.IsNullOrEmpty(origins))
						builder.WithOrigins(origins.Split(","));

					if (Configuration.GetValue<bool>("Cors:AllowCredentials"))
						builder.AllowCredentials();

					if (Configuration.GetValue<bool>("Cors:AllowAnyHeader"))
						builder.AllowAnyHeader();

					if (Configuration.GetValue<bool>("Cors:AllowAnyMethod"))
						builder.AllowAnyMethod();
					
					builder.WithExposedHeaders(Headers.TokensHeaderName);
				});
			}

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseMiddleware<SunExceptionMiddleware>();
			app.UseEndpoints(endPoints =>
			{
				endPoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
				endPoints.MapControllerRoute("default", "{controller}/{action}");
			});
		}

		public static void SetExceptionsMode(IWebHostEnvironment env, IConfiguration conf)
		{
			void ShowExceptions()
			{
				Console.WriteLine("Show exceptions mode: True");
				ErrorsMode.ShowErrors = true;
			}

			if (bool.TryParse(conf["Dev:ShowExceptions"], out bool showExceptions) && showExceptions)
				ShowExceptions();

			else if (env.IsDevelopment())
				ShowExceptions();
		}
	}
}