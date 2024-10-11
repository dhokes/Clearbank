using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    /// <summary>
    /// Interface for the account service.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Get an account.
        /// </summary>
        /// <param name="dataStoreType">The data store type.</param>
        /// <param name="accountNumber">The account number.</param>
        /// <returns>The account.</returns>
        public Account GetAccount(string dataStoreType, string accountNumber);
        
        /// <summary>
        /// Update an account.
        /// </summary>
        /// <param name="request">The payment request.</param>
        /// <param name="account">The account.</param>
        /// <param name="dataStoreType">The data store type.</param>
        void UpdateAccount(MakePaymentRequest request, Account account, string dataStoreType);
    }
}
