﻿using FootballShare.Entities.Pools;
using FootballShare.Entities.Users;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Services
{
    /// <summary>
    /// <see cref="Pool"/> management service
    /// </summary>
    public interface IPoolService : IService
    {
        /// <summary>
        /// Adds a <see cref="SiteUser"/> to a <see cref="Pool"/>
        /// </summary>
        /// <param name="user"><see cref="SiteUser"/> to add</param>
        /// <param name="pool">Target <see cref="Pool""/></param>
        /// <param name="asAdmin">Add <see cref="SiteUser"/> as Administrator</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task AddPoolMemberAsync(SiteUser user, Pool pool, bool asAdmin = false, CancellationToken cancellationToken = default);
        /// <summary>
        /// Creates a new <see cref="Pool"/>
        /// </summary>
        /// <param name="pool">New <see cref="Pool"/></param>
        /// <param name="userId">Creating <see cref="SiteUser"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created <see cref="BettingGroup"/></returns>
        Task<Pool> CreatePoolAsync(Pool pool, Guid userId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Deletes the specified <see cref="Pool"/>
        /// </summary>
        /// <param name="poolId"><see cref="Pool"/> unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeletePoolAsync(int poolId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets all <see cref="LedgerEntry"/> objects for the specified <see cref="Pool"/>
        /// </summary>
        /// <param name="poolId"><see cref="Pool"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="LedgerEntry"/> objects</returns>
        Task<IEnumerable<LedgerEntry>> GetPoolLedgerAsync(int poolId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves all members of the specified <see cref="Pool"/>
        /// </summary>
        /// <param name="poolId"><see cref="Pool"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="PoolMember"/> objects</returns>
        Task<IEnumerable<PoolMember>> GetMembersAsync(int poolId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a specific <see cref="Pool"/>
        /// </summary>
        /// <param name="pool"><see cref="Pool"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Matching <see cref="Pool"/></returns>
        Task<Pool> GetPoolAsync(int poolId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a specific <see cref="PoolMember"/>
        /// </summary>
        /// <param name="poolMemberId"><see cref="PoolMember"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="PoolMember"/> matching ID</returns>
        Task<PoolMember> GetPoolMemberAsync(int poolMemberId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a specific <see cref="PoolMember"/>
        /// </summary>
        /// <param name="poolId"><see cref="Pool"/> ID</param>
        /// <param name="userId"><see cref="SiteUser"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="PoolMember"/> matching ID</returns>
        Task<PoolMember> GetPoolMemberAsync(int poolId, Guid userId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets all publicly joinable <see cref="Pool"/> objects
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Pool"/> objects</returns>
        Task<IEnumerable<Pool>> GetPublicPoolsAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets all <see cref="Pool"/> objects the user is not joined to
        /// </summary>
        /// <param name="userId"><see cref="SiteUser"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Pool"/> objects</returns>
        Task<IEnumerable<Pool>> GetPublicPoolsNotJoinedAsync(Guid userId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves all <see cref="PoolMember"/> objects for a specific <see cref="SiteUser"/>
        /// </summary>
        /// <param name="userId"><see cref="SiteUser"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="PoolMember"/> objects</returns>
        Task<IEnumerable<PoolMember>> GetUserMembershipsAsync(Guid userId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves details about a <see cref="SiteUser"/> membership in a <see cref="Pool"/>
        /// </summary>
        /// <param name="userId"><see cref="SiteUser"/> ID</param>
        /// <param name="poolId"><see cref="Pool"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="PoolMember"/></returns>
        Task<PoolMember> GetUserPoolProfileAsync(Guid userId, int poolId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Removes a <see cref="PoolMember"/>
        /// </summary>
        /// <param name="poolMemberId"><see cref="PoolMember"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task RemovePoolMemberAsync(int poolMemberId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates the provided <see cref="Pool"/>
        /// </summary>
        /// <param name="pool"><see cref="Pool"/> to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated <see cref="Pool"/></returns>
        Task<Pool> UpdatePoolAsync(Pool pool, CancellationToken cancellationToken = default);
    }
}
