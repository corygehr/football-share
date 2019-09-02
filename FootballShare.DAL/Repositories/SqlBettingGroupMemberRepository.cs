using Dapper;
using FootballShare.Entities.Group;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// SQL Server implementation of <see cref="IBettingGroupMemberRepository"/>
    /// </summary>
    public class SqlBettingGroupMemberRepository : IBettingGroupMemberRepository
    {
        /// <summary>
        /// Database connection factory
        /// </summary>
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlBettingGroupMemberRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Database connection factory</param>
        public SqlBettingGroupMemberRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public async Task<BettingGroupMember> CreateAsync(BettingGroupMember entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"INSERT INTO [dbo].[BettingGroupMembers] (
                                [SiteUserId],
                                [BettingGroupId],
                                [IsAdmin],
                                [WhenCreated]
                              )
                              VALUES (
                                @{nameof(BettingGroupMember.SiteUserId)},
                                @{nameof(BettingGroupMember.BettingGroupId)},
                                @{nameof(BettingGroupMember.IsAdmin)},
                                CURRENT_TIMESTAMP
                              );
                              SELECT TOP 1 *
                              FROM [dbo].[BettingGroupMembers]
                              WHERE [SiteUserId] = @{nameof(BettingGroupMember.SiteUserId)}
                              AND [BettingGroupId] = @{nameof(BettingGroupMember.BettingGroupId)};";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<BettingGroupMember>(query, entity);
            }
        }

        public async Task DeleteAsync(BettingGroupMember entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"DELETE FROM [dbo].[BettingGroupMembers]
                              WHERE [SiteUserId] = @{nameof(BettingGroupMember.SiteUserId)}
                              AND [BettingGroupId] = @{nameof(BettingGroupMember.BettingGroupId)}";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, entity);
            }
        }

        public Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BettingGroupMember>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<BettingGroupMember> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<BettingGroupMember> GetAsync(BettingGroupMember entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[BettingGroupMembers]
                              WHERE [SiteUserId] = @{nameof(BettingGroupMember.SiteUserId)}
                              AND [BettingGroupId] = @{nameof(BettingGroupMember.BettingGroupId)}";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<BettingGroupMember>(query, entity);
            }
        }

        public Task<BettingGroupMember> UpdateAsync(BettingGroupMember entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
