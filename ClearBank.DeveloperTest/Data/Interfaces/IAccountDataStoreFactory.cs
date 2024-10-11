using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data.Interfaces
{
    public interface IAccountDataStoreFactory
    {
        /// <summary>
        /// Get an account.
        /// </summary>
        /// <param name="accountNumber">The account number.</param>
        /// <returns>The account.</returns>
        Account GetAccount(string accountNumber);
        
        /// <summary>
        /// Update an account.
        /// </summary>
        /// <param name="account">The account.</param>
        void UpdateAccount(Account account);
    }
}
