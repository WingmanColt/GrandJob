using Ganss.XSS;
using HireMe.Data;
using HireMe.Data.Repository;
using HireMe.Data.Repository.Interfaces;
using HireMe.Entities.Models;
using HireMe.Firewall.Checker.Core;
using HireMe.Firewall.Checker.Core.Interface;
using HireMe.Mapping.Utility;
using HireMe.Services;
using HireMe.Services.Core;
using HireMe.Services.Core.Interfaces;
using HireMe.Services.Interfaces;
using HireMe.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using NToastNotify;
using System;
using System.Globalization;
using System.IO.Compression;
using System.Reflection;
using WebMarkupMin.AspNetCore3;

namespace HireMe
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            
            Configuration = builder.Build();
         //   GC.Collect();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Connection string
            var connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<BaseDbContext>(options => options
            .UseSqlServer(connectionString), ServiceLifetime.Transient);
            services.AddDbContext<FeaturesDbContext>(options => options
            .UseSqlServer(connectionString), ServiceLifetime.Transient);
            services.AddDatabaseDeveloperPageExceptionFilter();

            // Identity
            IdentityConfig(services);

            // Services
            LoadServices(services);


            // Configure
            services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
            {
                ProgressBar = true,
                CloseButton = true,
                PreventDuplicates = true,
                PositionClass = ToastPositions.BottomRight,
                ShowDuration = 300,
                HideDuration = 1000,
                TimeOut = 3000,

                ShowMethod = "fadeIn",
                HideMethod = "fadeOut",
                ShowEasing = "swing",
                HideEasing = "linear",
            });


           /* services.AddRouting(option =>
            {
                option.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
                option.LowercaseUrls = true;
            });*/

            // Cookies
            CookiesConfig(services);
           
            
            // Cache 1
            //   services.AddMemoryCache();
            //  services.AddEFSecondLevelCache(options =>
            // options.UseMemoryCacheProvider().DisableLogging(true));


            // Cache 2
            services.AddResponseCaching();

            // Compression
            services.AddResponseCompression();
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });


            // .NET 3.1 with cache profiles
            services.AddControllersWithViews(options =>
            {
                options.CacheProfiles.Add("Weekly", new Microsoft.AspNetCore.Mvc.CacheProfile
                {
                    Duration = 60 * 60 * 24 * 7, // 7 days
                    Location = Microsoft.AspNetCore.Mvc.ResponseCacheLocation.Any,
                    NoStore = false
                });
                options.CacheProfiles.Add("Hourly", new Microsoft.AspNetCore.Mvc.CacheProfile
                {
                    VaryByHeader = "User-Agent",
                    Duration = 60 * 60, // 1 hour
                    Location = Microsoft.AspNetCore.Mvc.ResponseCacheLocation.Client,
                    NoStore = false
                });
            });

            services.AddRazorPages().AddRazorOptions(options =>
            {
                options.PageViewLocationFormats.Add("/Pages/Partials/{0}.cshtml");
            }); 

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole",
                     policy => policy.RequireRole("Admin"));
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
               options.SlidingExpiration = true;
               options.Cookie.MaxAge = new TimeSpan(72, 0, 0);
            });



            // Extensions
            LoadExtensions(services);                     
        }





        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();

                //app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

            }

            var supportedCultures = new[]
            {
              new CultureInfo("bg-BG"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("bg-BG"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseWebMarkupMin();
            app.UseResponseCompression();
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            // Toast notifications
            app.UseNToastNotify();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            // Cache
            app.UseResponseCaching();

            /*app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = (context) =>
                {
                    var headers = context.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromDays(365)
                    };
                }
            });*/

            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromDays(10)
                    };
                context.Response.Headers[HeaderNames.Vary] = new string[] { "Accept-Encoding" };
                await next();
            });


        }

        // Identity configuration
        private void IdentityConfig(IServiceCollection services)
        {
            // Identity configuration
            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequiredLength = 10;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 0;
                options.Lockout.DefaultLockoutTimeSpan = new TimeSpan(1, 0, 0);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
           "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            });

            services.AddAuthentication()
             .AddFacebook(facebookOptions =>
             {
                 facebookOptions.AppId = Configuration["FacebookConf:ID"];
                 facebookOptions.AppSecret = Configuration["FacebookConf:Secret"];
             })
             .AddGoogle(options =>
             {
                 options.ClientId = Configuration["GoogleConf:ID"];
                 options.ClientSecret = Configuration["GoogleConf:Secret"];

             })
             .AddLinkedIn(options =>
             {
                 options.ClientId = Configuration["LinkdinConf:ID"];
                 options.ClientSecret = Configuration["LinkdinConf:Secret"];

                 options.UserInformationEndpoint = "https://api.linkedin.com/v2/me?projection=(id,firstName,lastName,profilePicture(displayImage~:playableStreams))";
                 options.Scope.Add("r_liteprofile");
             });


            services.AddIdentity<User, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<BaseDbContext>();

        }



        // Services initializations
        private static void LoadServices(IServiceCollection services)
        {
            // Repositories
            services.AddSingleton(typeof(IRepository<>), typeof(DbBaseRepository<>));
            services.AddTransient(typeof(IRepository<>), typeof(DbFeaturesRepository<>));

            // Admin Services
            services.AddScoped<ILogService, LogService>();

            // App Services
            services.AddTransient<IAccountsService, AccountsService>();
            services.AddTransient<IBaseService, BaseService>();

            services.AddTransient<IScanner, WindowsDefenderScanner>();
            services.AddTransient<ICipherService, CipherService>();
            services.AddTransient<ISenderService, SenderService>();

            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IJobsService, JobsService>();
            services.AddTransient<IContestantsService, ContestantsService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IResumeService, ResumeService>();

            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<ISkillsService, SkillsService>();
            services.AddTransient<ILanguageService, LanguageService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IPromotionService, PromotionService>();
            services.AddTransient<IFavoritesService, FavoritesService>();

            IHtmlSanitizer sanitizer = new HtmlSanitizer();
            services.AddSingleton<IHtmlSanitizer>(sanitizer);

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // Cookies config
        private static void CookiesConfig(IServiceCollection services)
        {
            // Cookies
            services.Configure<CookieTempDataProviderOptions>(options => options.Cookie.Name = "TempDataCookie");

            services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, o =>
            {
                o.ExpireTimeSpan = TimeSpan.FromDays(3);
                o.SlidingExpiration = true;
            });

            services.ConfigureExternalCookie(config =>
            {
                config.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                config.Cookie.MaxAge = new TimeSpan(72, 0, 0);
            });
            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
               // options.Cookie.Name = $"{Configuration["GrandJob"]}.Session";
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            /*
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.Cookie.Name = "YourAppCookieName";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.LoginPath = "/Identity/Account/Login";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });
            */
        }

        // Load Extensions
        private static void LoadExtensions(IServiceCollection services)
        {
            services.AddWebMarkupMin(options =>
            {
                options.AllowMinificationInDevelopmentEnvironment = true;
                options.AllowCompressionInDevelopmentEnvironment = true;
            })

              .AddHtmlMinification(options =>
              {
                  options.MinificationSettings.RemoveRedundantAttributes = true;
                  options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
                  options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
              }).AddHttpCompression();
        }
    }
}

