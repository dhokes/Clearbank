using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    /// <summary>
    /// The account service.
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly IAccountDataStoreFactoryService accountDataStoreFactoryService;

        /// <summary>
        /// The account service.
        /// </summary>
        /// <param name="accountDataStoreFactoryService">The account data store factory service.</param>
        public AccountService(IAccountDataStoreFactoryService accountDataStoreFactoryService)
        {
            this.accountDataStoreFactoryService = accountDataStoreFactoryService;
        }

        /// <inheritdoc />
        public Account GetAccount(string dataStoreType, string accountNumber)
        {
            var accountDataStore = accountDataStoreFactoryService.GetAccountDataStoreFactory(dataStoreType);
            return accountDataStore?.GetAccount(accountNumber);
        }

        /// <inheritdoc />
        public void UpdateAccount(MakePaymentRequest request, Account account, string dataStoreType)
        {
            account.Balance -= request.Amount;

            var accountDataStore = accountDataStoreFactoryService.GetAccountDataStoreFactory(dataStoreType);
            accountDataStore?.UpdateAccount(account);
        }
    }
}
