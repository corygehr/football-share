using FootballShare.DAL.Repositories;
using FootballShare.Entities.Pools;
using FootballShare.Entities.Users;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Services
{
    /// <summary>
    /// <see cref="Pool"/> management service implementation
    /// </summary>
    public class PoolService : IPoolService
    {
        /// <summary>
        /// <see cref="LedgerEntry"/> repository
        /// </summary>
        private readonly ILedgerEntryRepository _ledgerRepo;
        /// <summary>
        /// <see cref="PoolMember"/> repository
        /// </summary>
        private readonly IPoolMemberRepository _poolMemberRepo;
        /// <summary>
        /// <see cref="Pool"/> repository
        /// </summary>
        private readonly IPoolRepository _poolRepo;
        /// <summary>
        /// <see cref="SiteUser"/> repository
        /// </summary>
        private readonly ISiteUserRepository _userRepo;

        /// <summary>
        /// Creates a new <see cref="PoolService"/> instance
        /// </summary>
        /// <param name="ledgerRepo"><see cref="LedgerEntry"/> repository</param>
        /// <param name="poolMemberRepo"><see cref="PoolMember"/> repository</param>
        /// <param name="poolRepo"><see cref="Pool"/> repository</param>
        /// <param name="userRepo"><see cref="SiteUser"/> repository</param>
        public PoolService(ILedgerEntryRepository ledgerRepo, IPoolRepository poolRepo, IPoolMemberRepository poolMemberRepo, ISiteUserRepository userRepo)
        {
            this._ledgerRepo = ledgerRepo;
            this._poolMemberRepo = poolMemberRepo;
            this._poolRepo = poolRepo;
            this._userRepo = userRepo;
        }

        public async Task AddPoolMemberAsync(SiteUser user, Pool pool, bool asAdmin = false, CancellationToken cancellationToken = default)
        {
            await this._poolMemberRepo.CreateAsync(
                new PoolMember
                {
                    Balance = pool.StartingBalance,
                    PoolId = pool.Id,
                    IsAdmin = asAdmin,
                    SiteUserId = user.Id
                },
                cancellationToken);
        }

        public async Task<Pool> CreatePoolAsync(Pool pool, Guid userId, CancellationToken cancellationToken = default)
        {
            SiteUser adminUser = await this._userRepo.GetAsync(userId.ToString(), cancellationToken);
            Pool newPool = await this._poolRepo.CreateAsync(pool, cancellationToken);
            // Add creating user as an administrator
            await this.AddPoolMemberAsync(adminUser, newPool, true, cancellationToken);

            return newPool;
        }

        public async Task DeletePoolAsync(int poolId, CancellationToken cancellationToken = default)
        {
            await this._poolRepo.DeleteAsync(poolId.ToString(), cancellationToken);
        }

        public async Task<IEnumerable<PoolMember>> GetMembersAsync(int poolId, CancellationToken cancellationToken = default)
        {
            return await this._poolMemberRepo.GetPoolMembersAsync(poolId, cancellationToken);
        }

        public async Task<IEnumerable<LedgerEntry>> GetPoolLedgerAsync(int poolId, CancellationToken cancellationToken = default)
        {
            return await this._ledgerRepo.GetEntriesForPoolAsync(poolId, cancellationToken);
        }

        public async Task<PoolMember> GetPoolMemberAsync(int poolMemberId, CancellationToken cancellationToken = default)
        {
            return await this._poolMemberRepo.GetAsync(poolMemberId.ToString(), cancellationToken);
        }

        public async Task<PoolMember> GetPoolMemberAsync(int poolId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await this._poolMemberRepo.GetMembershipAsync(userId, poolId, cancellationToken);
        }

        public async Task<Pool> GetPoolAsync(int poolId, CancellationToken cancellationToken = default)
        {
            return await this._poolRepo.GetAsync(poolId.ToString(), cancellationToken);
        }

        public async Task<IEnumerable<Pool>> GetPublicPoolsAsync(CancellationToken cancellationToken = default)
        {
            return await this._poolRepo.GetAllPublicAsync(cancellationToken);
        }

        public async Task<IEnumerable<Pool>> GetPublicPoolsNotJoinedAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            // Get all public pools
            IEnumerable<Pool> publicPools = await this.GetPublicPoolsAsync(cancellationToken);
            // Get user pools
            IEnumerable<PoolMember> userPools = await this._poolMemberRepo.GetUserMembershipsAsync(userId, cancellationToken);

            // Get pools which the user is not a member of
            return publicPools.Where(p => userPools.Where(up => up.PoolId == p.Id).Count() == 0);
        }

        public async Task<IEnumerable<PoolMember>> GetUserMembershipsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await this._poolMemberRepo.GetUserMembershipsAsync(userId, cancellationToken);
        }

        public async Task<PoolMember> GetUserPoolProfileAsync(Guid userId, int poolId, CancellationToken cancellationToken = default)
        {
            return await this._poolMemberRepo.GetMembershipAsync(userId, poolId, cancellationToken);
        }

        public async Task RemovePoolMemberAsync(int poolMemberId, CancellationToken cancellationToken = default)
        {
            await this._poolMemberRepo.DeleteAsync(poolMemberId.ToString(), cancellationToken);
        }

        public async Task<Pool> UpdatePoolAsync(Pool pool, CancellationToken cancellationToken = default)
        {
            return await this._poolRepo.UpdateAsync(pool, cancellationToken);
        }
    }
}
