using ClearBank.DeveloperTest.Data.Interfaces;

namespace ClearBank.DeveloperTest.Services
{
    /// <summary>
    /// Interface for the account data store factory service.
    /// </summary>
    public interface IAccountDataStoreFactoryService
    {
        /// <summary>
        /// Get the account data store factory.
        /// </summary>
        /// <param name="dataStoreType">The data store type.</param>
        /// <returns>The account data store factory.</returns>
        IAccountDataStoreFactory GetAccountDataStoreFactory(string dataStoreType);
    }
}