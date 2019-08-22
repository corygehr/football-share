using FootballShare.Entities.Models.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.Entities.Repositories
{
    /// <summary>
    /// <see cref="User"/> repository interface
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Gets a <see cref="User"/> by unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <param name=""></param>
        /// <returns></returns>
        Task<User> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}
