﻿using FootballShare.Entities.Group;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// Base repository interface for the <see cref="BettingGroup"/> class
    /// </summary>
    public interface IBettingGroupRepository : IRepository<BettingGroup>
    {
        /// <summary>
        /// Finds a specific <see cref="BettingGroup"/> by its unique Identifier
        /// </summary>
        /// <param name="newEntity"><see cref="BettingGroup"/> to store</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created <see cref="BettingGroup"/></returns>
        Task<BettingGroup> CreateAsync(BettingGroup newEntity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a specific <see cref="BettingGroup"/>
        /// </summary>
        /// <param name="entity"><see cref="BettingGroup"/> to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteAsync(BettingGroup entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns all public <see cref="BettingGroup"/> objects
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="BettingGroup"/> objects</returns>
        Task<IEnumerable<BettingGroup>> FindAllPublicAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns all <see cref="BettingGroupMember"/> objects for the specified <see cref="BettingGroup"/> ID
        /// </summary>
        /// <param name="groupId"><see cref="BettingGroup"/> unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="BettingGroupMember"/> objects</returns>
        Task<IEnumerable<BettingGroupMember>> GetBettingGroupMembersAsync(string groupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns all <see cref="BettingGroupPool"/> objects for the specified <see cref="BettingGroup"/> ID
        /// </summary>
        /// <param name="groupId"><see cref="BettingGroup"/> unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="BettingGroupPool"/> objects</returns>
        Task<IEnumerable<BettingGroupPool>> GetBettingGroupPoolsAsync(string groupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns all <see cref="BettingGroup"/> objects to which a specific member belongs
        /// </summary>
        /// <param name="userId">Unique <see cref="SiteUser"/> identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="BettingGroup"/> objects</returns>
        Task<IEnumerable<BettingGroup>> SearchByMemberUserIdAsync(string userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns all <see cref="BettingGroup"/> objects matching the provided name
        /// </summary>
        /// <param name="name">Name to locate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="BettingGroup"/> objects</returns>
        Task<IEnumerable<BettingGroup>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the provided <see cref="BettingGroup"/>
        /// </summary>
        /// <param name="entity"><see cref="BettingGroup"/> to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task UpdateAsync(BettingGroup entity, CancellationToken cancellationToken = default);
    }
}