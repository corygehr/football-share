using FootballShare.DAL;
using FootballShare.DAL.Repositories;
using FootballShare.DAL.Services;
using FootballShare.Entities.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FootballShare.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Set ApplicationInsights for logging
            services.AddApplicationInsightsTelemetry();

            // Set database connection provider
            services.AddTransient<IDbConnectionFactory>(db => new SqlDbConnectionFactory(
                Configuration.GetConnectionString("DefaultConnection")
            ));

            // Add data repositories
            services.AddTransient<ILedgerEntryRepository, SqlLedgerEntryRepository>();
            services.AddTransient<IPoolRepository, SqlPoolRepository>();
            services.AddTransient<IPoolMemberRepository, SqlPoolMemberRepository>();
            services.AddTransient<ISeasonRepository, SqlSeasonRepository>();
            services.AddTransient<ISeasonWeekRepository, SqlSeasonWeekRepository>();
            services.AddTransient<ISiteRoleRepository, SqlSiteRoleRepository>();
            services.AddTransient<ISiteUserLoginProviderRepository, SqlSiteUserLoginProviderRepository>();
            services.AddTransient<ISiteUserRepository, SqlSiteUserRepository>();
            services.AddTransient<ISportRepository, SqlSportRepository>();
            services.AddTransient<ISportsLeagueRepository, SqlSportsLeagueRepository>();
            services.AddTransient<ISpreadRepository, SqlSpreadRepository>();
            services.AddTransient<ITeamRepository, SqlTeamRepository>();
            services.AddTransient<ITeamAliasRepository, SqlTeamAliasRepository>();
            services.AddTransient<IWagerRepository, SqlWagerRepository>();
            services.AddTransient<IWeekEventRepository, SqlWeekEventRepository>();

            // Add identity services
            services.AddTransient<IUserStore<SiteUser>, SiteUserService>();
            services.AddTransient<IRoleStore<SiteRole>, SiteRoleService>();

            // Add data services
            services.AddTransient<IBettingService, BettingService>();
            services.AddTransient<IPoolService, PoolService>();
            services.AddTransient<ISportsLeagueService, SportsLeagueService>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddIdentity<SiteUser, SiteRole>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            services.AddAuthentication()
                .AddMicrosoftAccount(msaOptions =>
                {
                    msaOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                    msaOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
                })
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
