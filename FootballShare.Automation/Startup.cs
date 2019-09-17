using FootballShare.Automation.Parsers;
using FootballShare.DAL;
using FootballShare.DAL.Repositories;
using FootballShare.DAL.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Linq;

[assembly: FunctionsStartup(typeof(FootballShare.Automation.Startup))]

namespace FootballShare.Automation
{
    /// <summary>
    /// Function App service configuration
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// Application configuration
        /// </summary>
        public IConfiguration Configuration;

        /// <summary>
        /// Configures services for use in the Function app
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Get configuration
            this.Configuration = builder.Services
                .Where(s => s.ServiceType == typeof(IConfiguration)).First()
                .ImplementationInstance as IConfiguration;

            builder.Services.AddHttpClient();

            // Add database connections
            builder.Services.AddSingleton<IDbConnectionFactory>(db => new SqlDbConnectionFactory(
                Configuration.GetConnectionString("DefaultConnection")    
            ));

            // Add data repositories
            builder.Services.AddTransient<ILedgerEntryRepository, SqlLedgerEntryRepository>();
            builder.Services.AddTransient<IPoolRepository, SqlPoolRepository>();
            builder.Services.AddTransient<IPoolMemberRepository, SqlPoolMemberRepository>();
            builder.Services.AddTransient<ISeasonRepository, SqlSeasonRepository>();
            builder.Services.AddTransient<ISeasonWeekRepository, SqlSeasonWeekRepository>();
            builder.Services.AddTransient<ISiteUserRepository, SqlSiteUserRepository>();
            builder.Services.AddTransient<ISportRepository, SqlSportRepository>();
            builder.Services.AddTransient<ISportsLeagueRepository, SqlSportsLeagueRepository>();
            builder.Services.AddTransient<ISpreadRepository, SqlSpreadRepository>();
            builder.Services.AddTransient<ITeamRepository, SqlTeamRepository>();
            builder.Services.AddTransient<IWagerRepository, SqlWagerRepository>();
            builder.Services.AddTransient<IWeekEventRepository, SqlWeekEventRepository>();

            // Add services
            builder.Services.AddTransient<IBettingService, BettingService>();

            // Add parsers
            builder.Services.AddTransient<WeekEventsParser>();
            builder.Services.AddTransient<SpreadsParser>();
        }
    }
}
