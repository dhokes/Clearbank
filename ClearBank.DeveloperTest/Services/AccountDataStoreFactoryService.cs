using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Data.Interfaces;

namespace ClearBank.DeveloperTest.Services
{
    /// <summary>
    /// Account data store factory service.
    /// </summary>
    public class AccountDataStoreFactoryService : IAccountDataStoreFactoryService
    {
        /// <inheritdoc />
        public IAccountDataStoreFactory GetAccountDataStoreFactory(string dataStoreType)
        {
            return dataStoreType == "Backup" ? new BackupAccountDataStore() : new AccountDataStore();
        }
    }
}
