using FootballShare.Automation.Functions;
using FootballShare.Automation.Parsers;
using FootballShare.DAL.Repositories;
using FootballShare.DAL.Services;
using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;
using FootballShare.Entities.Pools;
using FootballShare.Entities.Users;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.Automation.Test.Functions
{
    /// <summary>
    /// Unit tests against the <see cref="UpdatePayoutsActivity"/> Function.
    /// </summary>
    [TestClass]
    public class UpdatePayoutsActivityShould
    {
        #region Define repository mocks
        /// <summary>
        /// <see cref="ISportsLeagueRepository"/> mock.
        /// </summary>
        private readonly Mock<ISportsLeagueRepository> _leagueRepo;
        /// <summary>
        /// <see cref="ILedgerEntryRepository"/> mock.
        /// </summary>
        private readonly Mock<ILedgerEntryRepository> _ledgerRepo;
        /// <summary>
        /// <see cref="IPoolMemberRepository"/> mock.
        /// </summary>
        private readonly Mock<IPoolMemberRepository> _poolMemberRepo;
        /// <summary>
        /// <see cref="IPoolRepository"/> mock.
        /// </summary>
        private readonly Mock<IPoolRepository> _poolRepo;
        /// <summary>
        /// <see cref="ISeasonRepository"/> mock.
        /// </summary>
        private readonly Mock<ISeasonRepository> _seasonRepo;
        /// <summary>
        /// <see cref="ISeasonWeekRepository"/> mock.
        /// </summary>
        private readonly Mock<ISeasonWeekRepository> _seasonWeekRepo;
        /// <summary>
        /// <see cref="ISpreadRepository"/> mock.
        /// </summary>
        private readonly Mock<ISpreadRepository> _spreadRepo;
        /// <summary>
        /// <see cref="ITeamRepository"/> mock.
        /// </summary>
        private readonly Mock<ITeamRepository> _teamRepo;
        /// <summary>
        /// <see cref="ISiteUserRepository"/> mock.
        /// </summary>
        private readonly Mock<ISiteUserRepository> _userRepo;
        /// <summary>
        /// <see cref="IWagerRepository"/> mock.
        /// </summary>
        private readonly Mock<IWagerRepository> _wagerRepo;
        /// <summary>
        /// <see cref="IWeekEventRepository"/> mock.
        /// </summary>
        private readonly Mock<IWeekEventRepository> _weekEventRepo;
        #endregion
        #region Define test assets
        /// <summary>
        /// <see cref="IBettingService"/> instance.
        /// </summary>
        private readonly IBettingService _bettingService;
        /// <summary>
        /// <see cref="ISportsLeagueService"/> instance.
        /// </summary>
        private readonly ISportsLeagueService _leagueService;
        /// <summary>
        /// <see cref="ILogger"/> mock.
        /// </summary>
        private readonly Mock<ILogger> _logMock;
        /// <summary>
        /// <see cref="UpdatePayoutsActivity"/> instance to test.
        /// </summary>
        private readonly UpdatePayoutsActivity _targetFunction;
        #endregion
        #region Define default objects
        /// <summary>
        /// Default <see cref="Sport"/> object.
        /// </summary>
        private static Sport SPORT = new Sport
        {
            Id = "test-sport",
            Name = "Test Sport",
            ShortName = "Test",
            WhenCreated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero)
        };
        /// <summary>
        /// Default <see cref="SportsLeague"/> object.
        /// </summary>
        private static SportsLeague LEAGUE = new SportsLeague
        {
            Id = "test-league",
            Name = "Test Sports League",
            ShortName = "Test",
            Sport = SPORT
        };
        /// <summary>
        /// Default <see cref="Season"/> object.
        /// </summary>
        private static Season SEASON = new Season
        {
            EndDate = new DateTimeOffset(2020, 2, 1, 0, 0, 0, TimeSpan.Zero),
            Id = "test-2019",
            League = LEAGUE,
            Name = "Test 2019 Season",
            SportsLeagueId = LEAGUE.Id,
            StartDate = new DateTimeOffset(2019, 8, 1, 0, 0, 0, TimeSpan.Zero),
            WhenCreated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero)
        };
        /// <summary>
        /// Default <see cref="SeasonWeek"/> object collection.
        /// </summary>
        private static IEnumerable<SeasonWeek> SEASON_WEEKS = new List<SeasonWeek>
        {
            new SeasonWeek
            {
                EndDate = new DateTimeOffset(2019, 9, 2, 0, 0, 0, TimeSpan.Zero),
                Id = "test-week-1",
                IsChampionship = false,
                IsPlayoff = false,
                IsPreseason = false,
                Name = "Test Week 1",
                Season = SEASON,
                SeasonId = SEASON.Id,
                Sequence = 1,
                StartDate = new DateTimeOffset(2019, 9, 1, 0, 0, 0, TimeSpan.Zero),
                WhenCreated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero)
            }
        };
        /// <summary>
        /// Default <see cref="Pool"/> object.
        /// </summary>
        private static Pool POOL = new Pool
        {
            Id = 1,
            IsPublic = false,
            Name = "Test Pool",
            Season = SEASON,
            SeasonId = SEASON.Id,
            StartingBalance = 10000,
            WagersPerWeek = 4,
            WhenCreated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero),
            WhenUpdated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero)
        };
        private static IEnumerable<Team> TEAMS = new List<Team>
        {
            new Team
            {
                Id = "test-team-1",
                League = LEAGUE,
                LeagueId = LEAGUE.Id,
                Name = "Test Team 1",
                ShortName = "Team 1",
                WhenCreated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero),
                WhenUpdated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero)
            },
            new Team
            {
                Id = "test-team-2",
                League = LEAGUE,
                LeagueId = LEAGUE.Id,
                Name = "Test Team 2",
                ShortName = "Team 2",
                WhenCreated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero),
                WhenUpdated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero)
            }
        };

        /// <summary>
        /// Default <see cref="SiteUser"/> object collection.
        /// </summary>
        private static IEnumerable<SiteUser> USERS = new List<SiteUser>
        {
            new SiteUser
            {
                DisplayName = "Test1",
                Email = "test_1@nomail.com",
                FullName = "Test User 1",
                Id = new Guid("00000000000000000000000000000001"),
                UserName = "test_1@nomail.com",
                WhenCreated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero),
                WhenUpdated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero)
            }
        };
        /// <summary>
        /// Default <see cref="PoolMember"/> object collection.
        /// </summary>
        private static IEnumerable<PoolMember> POOL_MEMBERS = new List<PoolMember>
            {
                new PoolMember
                {
                    Balance = 2500,
                    Id = 1,
                    IsAdmin = false,
                    Pool = POOL,
                    PoolId = POOL.Id,
                    SiteUserId = USERS.ElementAt(0).Id,
                    WhenCreated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero),
                    WhenUpdated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero)
                }
            };
        #endregion

        /// <summary>
        /// Configures objects required for testing.
        /// </summary>
        public UpdatePayoutsActivityShould()
        {
            // Configure repositories
            this._leagueRepo = new Mock<ISportsLeagueRepository>();
            this._ledgerRepo = new Mock<ILedgerEntryRepository>();
            this._poolMemberRepo = new Mock<IPoolMemberRepository>();
            this._poolRepo = new Mock<IPoolRepository>();
            this._seasonRepo = new Mock<ISeasonRepository>();
            this._seasonWeekRepo = new Mock<ISeasonWeekRepository>();
            this._spreadRepo = new Mock<ISpreadRepository>();
            this._teamRepo = new Mock<ITeamRepository>();
            this._userRepo = new Mock<ISiteUserRepository>();
            this._wagerRepo = new Mock<IWagerRepository>();
            this._weekEventRepo = new Mock<IWeekEventRepository>();

            // Define return objects
            this._leagueRepo
                .Setup(l => l.GetAllAsync(CancellationToken.None))
                .Returns(Task.FromResult((IEnumerable<SportsLeague>)new List<SportsLeague> { LEAGUE }));
            this._leagueRepo
                .Setup(l => l.GetAsync(LEAGUE.Id, CancellationToken.None))
                .Returns(Task.FromResult(LEAGUE));
            this._poolRepo
                .Setup(p => p.GetAsync(POOL.Id.ToString(), CancellationToken.None))
                .Returns(Task.FromResult(POOL));
            this._poolMemberRepo
                .Setup(m => m.GetAllAsync(CancellationToken.None))
                .Returns(Task.FromResult(POOL_MEMBERS));
            this._poolMemberRepo
                .Setup(m => m.GetMembershipAsync(USERS.ElementAt(0).Id, POOL.Id, CancellationToken.None))
                .Returns(Task.FromResult(POOL_MEMBERS.ElementAt(0)));
            this._poolMemberRepo
                .Setup(m => m.GetPoolMembersAsync(POOL.Id, CancellationToken.None))
                .Returns(Task.FromResult(POOL_MEMBERS));
            this._seasonRepo
                .Setup(s => s.GetAllAsync(CancellationToken.None))
                .Returns(Task.FromResult((IEnumerable<Season>)new List<Season> { SEASON }));
            this._seasonRepo
                .Setup(s => s.GetAllCurrentSeasonsAsync(CancellationToken.None))
                .Returns(Task.FromResult((IEnumerable<Season>)new List<Season> { SEASON }));
            this._seasonRepo
                .Setup(s => s.GetAsync(SEASON.Id, CancellationToken.None))
                .Returns(Task.FromResult(SEASON));
            this._seasonRepo
                .Setup(s => s.GetCurrentLeagueSeasonAsync(LEAGUE.Id, CancellationToken.None))
                .Returns(Task.FromResult(SEASON));
            this._seasonWeekRepo
                .Setup(w => w.GetAllAsync(CancellationToken.None))
                .Returns(Task.FromResult(SEASON_WEEKS));
            this._seasonWeekRepo
                .Setup(w => w.GetAllForSeasonAsync(SEASON.Id, CancellationToken.None))
                .Returns(Task.FromResult(SEASON_WEEKS));
            this._seasonWeekRepo
                .Setup(w => w.GetAsync(SEASON_WEEKS.ElementAt(0).Id, CancellationToken.None))
                .Returns(Task.FromResult(SEASON_WEEKS.ElementAt(0)));
            this._seasonWeekRepo
                .Setup(w => w.GetCurrentSeasonWeekAsync(SEASON.Id, CancellationToken.None))
                .Returns(Task.FromResult(SEASON_WEEKS.ElementAt(0)));
            this._teamRepo
                .Setup(t => t.GetAllAsync(CancellationToken.None))
                .Returns(Task.FromResult(TEAMS));
            this._teamRepo
                .Setup(t => t.GetAsync(TEAMS.ElementAt(0).Id, CancellationToken.None))
                .Returns(Task.FromResult(TEAMS.ElementAt(0)));
            this._teamRepo
                .Setup(t => t.GetByNameAsync(TEAMS.ElementAt(0).Name, CancellationToken.None))
                .Returns(Task.FromResult(TEAMS.ElementAt(0)));
            this._teamRepo
                .Setup(t => t.GetAsync(TEAMS.ElementAt(1).Id, CancellationToken.None))
                .Returns(Task.FromResult(TEAMS.ElementAt(1)));
            this._teamRepo
                .Setup(t => t.GetByNameAsync(TEAMS.ElementAt(1).Name, CancellationToken.None))
                .Returns(Task.FromResult(TEAMS.ElementAt(1)));

            // Configure services
            this._bettingService = new BettingService(
                this._ledgerRepo.Object,
                this._poolRepo.Object,
                this._poolMemberRepo.Object,
                this._seasonRepo.Object,
                this._seasonWeekRepo.Object,
                this._spreadRepo.Object,
                this._teamRepo.Object,
                this._userRepo.Object,
                this._wagerRepo.Object,
                this._weekEventRepo.Object
                );

            this._leagueService = new SportsLeagueService(
                this._leagueRepo.Object,
                this._seasonRepo.Object,
                this._teamRepo.Object,
                this._weekEventRepo.Object
                );

            // Configure target function
            this._targetFunction = new UpdatePayoutsActivity(
                this._bettingService,
                this._leagueService
                );

            this._logMock = new Mock<ILogger>();
        }

        /// <summary>
        /// Should not payout when the selected team fails to cover the spread. For example, 
        /// BUF@CLE w/ BUF -7 and CLE 7; if CLE scores 14 and BUF scores 16, BUG does not pay.
        /// </summary>
        [TestMethod]
        public void NotPayoutForSpreadMiss()
        {
            // Arrange

            // Create WeekEvent
            WeekEvent weekEvent = new WeekEvent
            {
                AwayScore = 17,
                AwayTeam = TEAMS.ElementAt(0),
                AwayTeamId = TEAMS.ElementAt(0).Id,
                HomeScore = 27,
                HomeTeam = TEAMS.ElementAt(1),
                HomeTeamId = TEAMS.ElementAt(1).Id,
                Id = 1,
                SeasonWeekId = SEASON_WEEKS.ElementAt(0).Id,
                Time = new DateTimeOffset(2019, 9, 1, 12, 0, 0, TimeSpan.Zero),
                Week = SEASON_WEEKS.ElementAt(0),
                WhenCreated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero),
                WhenUpdated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero)
            };

            // Create Spread
            Spread spread = new Spread
            {
                AwaySpread = 6.5,
                Event = weekEvent,
                HomeSpread = -6.5,
                Id = 1,
                WeekEventId = weekEvent.Id,
                WhenCreated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero)
            };

            // Create user wager
            Wager wager = new Wager
            {
                Amount = 500,
                Event = weekEvent,
                Id = 1,
                Pool = POOL,
                PoolId = POOL.Id,
                SelectedTeam = weekEvent.AwayTeam,
                SelectedTeamId = weekEvent.AwayTeamId,
                SelectedTeamSpread = spread.AwaySpread,
                SiteUserId = USERS.ElementAt(0).Id,
                User = USERS.ElementAt(0),
                WeekEventId = weekEvent.Id,
                WhenCreated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero)
            };

            // Attach objects to repositories
            this._weekEventRepo
                .Setup(w => w.GetAsync(weekEvent.Id.ToString(), CancellationToken.None))
                .Returns(Task.FromResult(weekEvent));
            this._spreadRepo
                .Setup(s => s.GetByWeekEventAsync(weekEvent.Id, CancellationToken.None))
                .Returns(Task.FromResult(spread));
            this._wagerRepo
                .Setup(w => w.GetUnresolvedWagersAsync(CancellationToken.None))
                .Returns(Task.FromResult((IEnumerable<Wager>)new List<Wager> { wager }));

            // Act
            _targetFunction.Run(null, this._logMock.Object, CancellationToken.None);

            // Assert
            Assert.AreEqual(WagerResult.Loss, wager.Result);
        }

        /// <summary>
        /// Should payout when a team covers the spread via handicap. For example, 
        /// BUF@CLE w/ BUF -7 and CLE 7; if CLE scores 14 and BUF scores 16, CLE pays.
        /// </summary>
        [TestMethod]
        public void PayoutForPositiveSpreadWins()
        {
            // Arrange

            // Create WeekEvent
            WeekEvent weekEvent = new WeekEvent
            {
                AwayScore = 17,
                AwayTeam = TEAMS.ElementAt(0),
                AwayTeamId = TEAMS.ElementAt(0).Id,
                HomeScore = 27,
                HomeTeam = TEAMS.ElementAt(1),
                HomeTeamId = TEAMS.ElementAt(1).Id,
                Id = 1,
                SeasonWeekId = SEASON_WEEKS.ElementAt(0).Id,
                Time = new DateTimeOffset(2019, 9, 1, 12, 0, 0, TimeSpan.Zero),
                Week = SEASON_WEEKS.ElementAt(0),
                WhenCreated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero),
                WhenUpdated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero)
            };

            // Create Spread
            Spread spread = new Spread
            {
                AwaySpread = 6.5,
                Event = weekEvent,
                HomeSpread = -6.5,
                Id = 1,
                WeekEventId = weekEvent.Id,
                WhenCreated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero)
            };

            // Create user wager
            Wager wager = new Wager
            {
                Amount = 500,
                Event = weekEvent,
                Id = 1,
                Pool = POOL,
                PoolId = POOL.Id,
                SelectedTeam = weekEvent.HomeTeam,
                SelectedTeamId = weekEvent.HomeTeamId,
                SelectedTeamSpread = spread.HomeSpread,
                SiteUserId = USERS.ElementAt(0).Id,
                User = USERS.ElementAt(0),
                WeekEventId = weekEvent.Id,
                WhenCreated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero)
            };

            // Attach objects to repositories
            this._weekEventRepo
                .Setup(w => w.GetAsync(weekEvent.Id.ToString(), CancellationToken.None))
                .Returns(Task.FromResult(weekEvent));
            this._spreadRepo
                .Setup(s => s.GetByWeekEventAsync(weekEvent.Id, CancellationToken.None))
                .Returns(Task.FromResult(spread));
            this._wagerRepo
                .Setup(w => w.GetUnresolvedWagersAsync(CancellationToken.None))
                .Returns(Task.FromResult((IEnumerable<Wager>)new List<Wager> { wager }));

            // Act
            _targetFunction.Run(null, this._logMock.Object, CancellationToken.None);

            // Assert
            Assert.AreEqual(WagerResult.Win, wager.Result);
        }

        /// <summary>
        /// Should push when the chosen team's score with spread matches the opponent. For example, 
        /// BUF@CLE w/ BUF -7 and CLE 7; if CLE scores 10 and BUF scores 17, original bet amount 
        /// is returned to the betting user.
        /// </summary>
        [TestMethod]
        public void PushOnEvenScoreSpread()
        {
            // Arrange

            // Create WeekEvent
            WeekEvent weekEvent = new WeekEvent
            {
                AwayScore = 10,
                AwayTeam = TEAMS.ElementAt(0),
                AwayTeamId = TEAMS.ElementAt(0).Id,
                HomeScore = 17,
                HomeTeam = TEAMS.ElementAt(1),
                HomeTeamId = TEAMS.ElementAt(1).Id,
                Id = 1,
                SeasonWeekId = SEASON_WEEKS.ElementAt(0).Id,
                Time = new DateTimeOffset(2019, 9, 1, 12, 0, 0, TimeSpan.Zero),
                Week = SEASON_WEEKS.ElementAt(0),
                WhenCreated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero),
                WhenUpdated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero)
            };

            // Create Spread
            Spread spread = new Spread
            {
                AwaySpread = 7,
                Event = weekEvent,
                HomeSpread = -7,
                Id = 1,
                WeekEventId = weekEvent.Id,
                WhenCreated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero)
            };

            // Create user wager
            Wager wager = new Wager
            {
                Amount = 500,
                Event = weekEvent,
                Id = 1,
                Pool = POOL,
                PoolId = POOL.Id,
                SelectedTeam = weekEvent.AwayTeam,
                SelectedTeamId = weekEvent.AwayTeamId,
                SelectedTeamSpread = spread.AwaySpread,
                SiteUserId = USERS.ElementAt(0).Id,
                User = USERS.ElementAt(0),
                WeekEventId = weekEvent.Id,
                WhenCreated = new DateTimeOffset(2019, 12, 25, 0, 0, 0, TimeSpan.Zero)
            };

            // Attach objects to repositories
            this._weekEventRepo
                .Setup(w => w.GetAsync(weekEvent.Id.ToString(), CancellationToken.None))
                .Returns(Task.FromResult(weekEvent));
            this._spreadRepo
                .Setup(s => s.GetByWeekEventAsync(weekEvent.Id, CancellationToken.None))
                .Returns(Task.FromResult(spread));
            this._wagerRepo
                .Setup(w => w.GetUnresolvedWagersAsync(CancellationToken.None))
                .Returns(Task.FromResult((IEnumerable<Wager>)new List<Wager> { wager }));

            // Act
            _targetFunction.Run(null, this._logMock.Object, CancellationToken.None);

            // Assert
            Assert.AreEqual(WagerResult.Push, wager.Result);
        }
    }
}
