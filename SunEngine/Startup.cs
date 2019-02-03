using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;
using Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SunEngine.Authorization;
using SunEngine.Commons.DataBase;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using SunEngine.Commons.Models;
using SunEngine.Commons.Models.UserGroups;
using SunEngine.Commons.Services;
using SunEngine.Commons.TextProcess;
using SunEngine.Infrastructure;
using SunEngine.Services;
using SunEngine.Stores;
using IAuthorizationService = SunEngine.Commons.Services.IAuthorizationService;

namespace SunEngine
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

            services.AddAllOptions(Configuration);


            // Add DataBase
            LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;

            var dataBaseConfiguration = Configuration.GetSection("DataBaseConnection");
            var providerName = dataBaseConfiguration["Provider"];
            var connectionString = dataBaseConfiguration["ConnectionString"];
            var dataProvider = DataBaseConnection.GetDataProvider(providerName, connectionString);
            MyMappingSchema mappingSchema = new MyMappingSchema();

            services.AddScoped(x => new DataBaseConnection(dataProvider, connectionString, mappingSchema));
            
            var dataBaseFactory = new DataBaseFactory(dataProvider, connectionString, mappingSchema);

            
            services.AddSingleton<IDataBaseFactory>(dataBaseFactory);
            
            // Add Singleton Services
            services.AddSingleton<OperationKeysContainer>();
            
            services.AddSingleton<IAuthorizationService, AuthorizationService>();
            
            services.AddSingleton<Sanitizer>();


            // Add Singleton Stores
            var userGroupStore = new UserGroupStore(dataBaseFactory);

            services.AddSingleton<IUserGroupStore>(userGroupStore);

            services.AddSingleton<ICategoriesStore, CategoriesStore>();

            services.AddSingleton<SpamProtectionStore>();
            

            services.AddSingletonImages();

            
            services.AddIdentity<User, UserGroupDB>(
                    options =>
                    {
                        options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequiredUniqueChars = 2;
                        options.Password.RequiredLength = 6;
                        options.User.RequireUniqueEmail = true;
                        
                        const string engChars = "abcdefghijklmnopqrstuvwxyz";
                        const string rusChars = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
                        const string numbers = "0123456789";
                        const string other = "-";
                        
                        options.User.AllowedUserNameCharacters = engChars + engChars.ToUpper() + rusChars + rusChars.ToUpper() + numbers+other;
                    })
                .AddLinqToDBStores<int>(dataBaseFactory)
                .AddUserManager<MyUserManager>()
                .AddRoleManager<MyRoleManager>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config =>
                {
                    if (CurrentEnvironment.IsDevelopment())
                    {
                        config.RequireHttpsMetadata = false;
                    }
                    
                    //config.SaveToken = true;
                   
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:ShortJwtSecurityKey"]))
                    };
                });
            
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = MyJwt.Scheme;
                    //options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = MyJwt.Scheme;
                })
                .AddScheme<MyJwtOptions, MyJwtHandler>(MyJwt.Scheme, MyJwt.Scheme, options => { });

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScopedEntityServices();

            services.AddScopedControllersAuthorizationServices();
            
            services.AddAdminServices();

            
            
            //services.AddMemoryCache();
            
            services.AddCryptServices();
            
            services.AddSingleton<CaptchaService>();



            services.AddMvcCore(options => { /*options.Filters.Add(new MyAuthUserFilter(userGroupStore));*/ })
                .AddApiExplorer()
                .AddAuthorization()
                .AddJsonFormatters(options =>
                {
                    options.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                })
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
                        .AllowCredentials().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("TOKENS"));
            }


            //app.UseMiddleware<JwtMiddleware>();
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