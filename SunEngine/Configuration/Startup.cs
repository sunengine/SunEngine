using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SunEngine.Configuration.AddServices;
using SunEngine.DataBase;
using SunEngine.Security;
using SunEngine.Security.Authentication;
using SunEngine.Services;
using SunEngine.Utils.TextProcess;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace SunEngine.Configuration
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

            DataBaseFactory dataBaseFactory = services.AddDatabase(Configuration);

            services.AddStores(dataBaseFactory);

            services.AddIdentity(dataBaseFactory);

            AddAuthenticationExtensions.AddAuthentication(services);

            AddAuthorizationExtensions.AddAuthorization(services);

            services.AddManagers();

            services.AddPresenters();

            services.AddAdminServices();

            //services.AddMemoryCache();

            services.AddCryptServices();

            services.AddImagesServices();

            services.AddUrlHelper();

            services.AddScoped<IAccountService, AccountService>();
            services.AddSingleton<CaptchaService>();
            services.AddSingleton<Sanitizer>();
            services.AddTransient<IEmailSender, EmailSender>();


            services.AddMvcCore(options =>
                {
                    // Add filters here
                })
                .AddApiExplorer()
                .AddAuthorization()
                .AddJsonFormatters(options => { options.DateTimeZoneHandling = DateTimeZoneHandling.Utc; })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }


        public void Configure(IApplicationBuilder app)
        {
            if (CurrentEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }


            //app.UseHttpsRedirection();
            //app.UseFileServer();

            app.UseCookiePolicy();

            if (CurrentEnvironment.IsDevelopment())
            {
                app.UseStaticFiles();

                app.UseCors(builder =>
                    builder.WithOrigins("http://localhost:5005")
                        .AllowCredentials().AllowAnyHeader().AllowAnyMethod()
                        .WithExposedHeaders("TOKENS", "TOKENSEXPIRE"));
            }


            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}");
            });
        }
    }
}