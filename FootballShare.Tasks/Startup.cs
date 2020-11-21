using FootballShare.DAL;
using FootballShare.DAL.Repositories;
using FootballShare.DAL.Services;
using FootballShare.Tasks.Parsers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

[assembly: FunctionsStartup(typeof(FootballShare.Tasks.Startup))]
namespace FootballShare.Tasks
{
    /// <summary>
    /// Function App service configuration
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// Configures services for use in the Function app
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Get HostBuilder context data for configuration
            FunctionsHostBuilderContext context = builder.GetContext();

            // Allow HTTP Client requests
            builder.Services.AddHttpClient();

            // Add database connections
            builder.Services.AddSingleton<IDbConnectionFactory>(db => new SqlDbConnectionFactory(
                context.Configuration.GetConnectionString("DefaultConnection")
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
            builder.Services.AddTransient<ITeamAliasRepository, SqlTeamAliasRepository>();
            builder.Services.AddTransient<IWagerRepository, SqlWagerRepository>();
            builder.Services.AddTransient<IWeekEventRepository, SqlWeekEventRepository>();

            // Add services
            builder.Services.AddTransient<IBettingService, BettingService>();
            builder.Services.AddTransient<IPoolService, PoolService>();
            builder.Services.AddTransient<ISportsLeagueService, SportsLeagueService>();

            // Add parsers
            builder.Services.AddTransient<ScoresParser>();
            builder.Services.AddTransient<SpreadsParser>();
            builder.Services.AddTransient<WeekEventsParser>();
        }
    }
}
