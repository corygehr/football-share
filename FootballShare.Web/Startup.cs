using FootballShare.DAL;
using FootballShare.DAL.Repositories;
using FootballShare.DAL.Services;
using FootballShare.Entities.User;
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
            // Set database connection provider
            services.AddTransient<IDbConnectionFactory>(db => new SqlDbConnectionFactory(
                Configuration.GetConnectionString("DefaultConnection")
            ));

            // Add data repositories
            services.AddTransient<IBettingGroupRepository, SqlBettingGroupRepository>();
            services.AddTransient<IBettingGroupMemberRepository, SqlBettingGroupMemberRepository>();
            services.AddTransient<ISeasonRepository, SqlSeasonRepository>();
            services.AddTransient<ISiteRoleRepository, SqlSiteRoleRepository>();
            services.AddTransient<ISiteUserLoginProviderRepository, SqlSiteUserLoginProviderRepository>();
            services.AddTransient<ISiteUserRepository, SqlSiteUserRepository>();
            services.AddTransient<ISportsLeagueRepository, SqlSportsLeagueRepository>();
            services.AddTransient<IWagerRepository, SqlWagerRepository>();
            services.AddTransient<IWeekEventRepository, SqlWeekEventRepository>();

            // Add identity services
            services.AddTransient<IUserStore<SiteUser>, SiteUserService>();
            services.AddTransient<IRoleStore<SiteRole>, SiteRoleService>();

            // Add data services
            services.AddTransient<IBettingService, BettingService>();
            services.AddTransient<IGroupManagementService, GroupManagementService>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddIdentity<SiteUser, SiteRole>()
                .AddDefaultTokenProviders()
                .AddDefaultUI(UIFramework.Bootstrap4);

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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
