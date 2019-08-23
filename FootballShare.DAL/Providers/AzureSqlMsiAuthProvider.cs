using Microsoft.Azure.Services.AppAuthentication;

using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Providers
{
    /// <summary>
    /// Retrieves an Access Token for an Azure SQL Managed Service Identity
    /// </summary>
    internal sealed class AzureSqlMsiAuthProvider
    {
        /// <summary>
        /// Azure Active Directory resource name for Azure SQL
        /// </summary>
        private const string AAD_RESOURCE_NAME = "https://database.windows.net/";

        /// <summary>
        /// Retrieves the Access Token
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Access token</returns>
        public static async Task<string> GetTokenAsync(CancellationToken cancellationToken = default)
        {
            AzureServiceTokenProvider tokenProvider = new AzureServiceTokenProvider();
            return await tokenProvider.GetAccessTokenAsync(AAD_RESOURCE_NAME);
        }
    }
}
