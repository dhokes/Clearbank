using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;

namespace ClearBank.DeveloperTest.Tests.Services
{
    [TestClass]
    public class AccountDataStoreFactoryServiceTests
    {
        [TestMethod]
        public void GetAccountDataStoreFactory_Returns_BackupAccountDataStore()
        {
            // Arrange
            var dataStoreType = "Backup";
            var service = new AccountDataStoreFactoryService();

            // Act
            var factory = service.GetAccountDataStoreFactory(dataStoreType);

            //Assert
            Assert.AreEqual(typeof(BackupAccountDataStore), factory.GetType());
        }

        [TestMethod]
        public void GetAccountDataStoreFactory_Returns_BAccountDataStore()
        {
            // Arrange
            var dataStoreType = "Test";
            var service = new AccountDataStoreFactoryService();

            // Act
            var factory = service.GetAccountDataStoreFactory(dataStoreType);

            //Assert
            Assert.AreEqual(typeof(AccountDataStore), factory.GetType());
        }
    }
}
