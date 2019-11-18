using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SunEngine.Admin;
using SunEngine.Core.Configuration.AddServices;
using SunEngine.Core.Errors;
using SunEngine.Core.Security;
using SunEngine.Core.Services;

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

            var origins = Configuration.GetValue<string>("Cors:Origins");
            if (!string.IsNullOrEmpty(origins))
                services.AddCors();

            services.AddOptions(Configuration);

            services.AddDatabase(Configuration, out var dataBaseFactory);

            services.AddDbOptions(dataBaseFactory);

            services.AddCaches(dataBaseFactory);

            services.AddCachePolicy();

            services.AddIdentity(dataBaseFactory);

            services.AddSunAuthentication();

            services.AddSunAuthorization();

            services.AddManagers();

            services.AddPresenters();

            services.AddAdmin();

            services.AddMemoryCache();

            services.AddImagesServices();

            services.AddCiphers(dataBaseFactory);

            services.AddCounters();

            services.AddJobs();

            services.AddSingleton<CaptchaService>();
            services.AddSanitizer();

            services.AddTransient<IEmailSenderService, EmailSenderService>();

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

            /*.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = SunJsonContractResolver.Instance;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            });*/
        }


        public void Configure(IApplicationBuilder app)
        {
            if (!CurrentEnvironment.IsDevelopment())
                app.UseHsts();
            
            app.UseFileServer();

            app.UseCookiePolicy();

            var origins = Configuration.GetValue<string>("Cors:Origins").Split(",");

            if (Configuration.GetValue<bool>("FileServer"))
                app.UseStaticFiles();

            if (origins != null && origins.Length >= 1)
            {
                app.UseCors(builder =>
                    builder.WithOrigins(origins)
                        .AllowCredentials().AllowAnyHeader().AllowAnyMethod()
                        .WithExposedHeaders(Headers.TokensHeaderName));
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
                //Console.WriteLine("ShowExceptions mode");
                //SunJsonContractResolver.ShowExceptions = true;
            }

            if (bool.TryParse(conf["Dev:ShowExceptions"], out bool showExceptions))
            {
                if (showExceptions)
                    ShowExceptions();
            }
            else if (env.IsDevelopment())
            {
                ShowExceptions();
            }
        }
    }
}