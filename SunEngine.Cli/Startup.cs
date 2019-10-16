using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SunEngine.Admin;
using SunEngine.Core.Configuration.AddServices;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors;
using SunEngine.Core.Security;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;
using SunEngine.Core.Utils.TextProcess;

namespace SunEngine.Cli
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        private IConfiguration Configuration { get; }

        private IHostingEnvironment CurrentEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            if (CurrentEnvironment.IsDevelopment())
            {
                services.AddCors();
            }

            services.AddOptions(Configuration);

            DataBaseFactory dataBaseFactory = services.AddDatabase(Configuration); // TODO make internal def

            services.AddDbOptions(dataBaseFactory);
            
            services.AddCaches(dataBaseFactory);

            services.AddCachePolicy();

            services.AddIdentity(dataBaseFactory);

            AddAuthenticationExtensions.AddAuthentication(services);

            AddAuthorizationExtensions.AddAuthorization(services);

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

            services.AddMvcCore(options =>
                {
                    // Add filters here
                })
                .AddApiExplorer()
                .AddAuthorization()
                .AddJsonFormatters(options =>
                {
                    options.ContractResolver = SunJsonContractResolver.Instance;
                    options.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }


        public void Configure(IApplicationBuilder app)
        {
            if (!CurrentEnvironment.IsDevelopment())
                app.UseHsts();

            // app.UseFileServer();

            app.UseCookiePolicy();

            if (CurrentEnvironment.IsDevelopment())
            {
                app.UseStaticFiles();

                app.UseCors(builder =>
                    builder.WithOrigins("http://localhost:5005")
                        .AllowCredentials().AllowAnyHeader().AllowAnyMethod()
                        .WithExposedHeaders(Headers.TokensHeaderName));
            }

            app.UseAuthentication();
            
            app.UseMiddleware<SunExceptionMiddleware>();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}");
            });
        }


        public static void SetExceptionsMode(IHostingEnvironment env, IConfiguration conf)
        {
            void ShowExceptions()
            {
                Console.WriteLine("ShowExceptions mode");
                SunJsonContractResolver.ShowExceptions = true;
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
