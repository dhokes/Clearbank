using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;

namespace ClearBank.DeveloperTest.Tests.Services
{
    [TestClass]
    public class AccountServiceTests
    {
        [TestMethod]
        public void GetAccount_Returns_Expected_Balance()
        {
            // Arrange
            var dataStoreType = "Backup";
            var accountNumber = "123456";
            var expectedBalance = 100;

            var mockDataStoreFactory = new Mock<IAccountDataStoreFactory>();
            mockDataStoreFactory.Setup(x => x.GetAccount(accountNumber)).Returns(new Account() { AccountNumber = accountNumber, Balance = expectedBalance });

            var mockAccountDataStoreFactoryService = new Mock<IAccountDataStoreFactoryService>();
            mockAccountDataStoreFactoryService.Setup(x => x.GetAccountDataStoreFactory(dataStoreType)).Returns(mockDataStoreFactory.Object);
            var accountService = new AccountService(mockAccountDataStoreFactoryService.Object);
            
            // Act
            var account = accountService.GetAccount(dataStoreType, accountNumber);
            
            //Assert
            Assert.AreEqual(expectedBalance, account.Balance);
        }

        [TestMethod]
        public void GetAccount_GetAccountDataStoreFactory_ReturnsNull()
        {
            // Arrange
            var dataStoreType = "Backup";
            var accountNumber = "123456";

            var mockDataStoreFactory = new Mock<IAccountDataStoreFactory>();

            var mockAccountDataStoreFactoryService = new Mock<IAccountDataStoreFactoryService>();
            mockAccountDataStoreFactoryService.Setup(x => x.GetAccountDataStoreFactory(dataStoreType)).Returns((IAccountDataStoreFactory)null);
            var accountService = new AccountService(mockAccountDataStoreFactoryService.Object);
            
            // Act
            var account = accountService.GetAccount(dataStoreType, accountNumber);
            
            //Assert
            Assert.IsNull(account);
        }

        [TestMethod]
        public void UpdateAccount_FactoryMethod_Called_Once()
        {
            // Arrange
            var dataStoreType = "Backup";
            var account = new Account() { Balance = 200 };
            var request = new MakePaymentRequest() { Amount = 10 };

            var mockDataStoreFactory = new Mock<IAccountDataStoreFactory>();
            mockDataStoreFactory.Setup(x => x.UpdateAccount(It.IsAny<Account>())).Verifiable();
            var mockAccountDataStoreFactoryService = new Mock<IAccountDataStoreFactoryService>();
            mockAccountDataStoreFactoryService.Setup(x => x.GetAccountDataStoreFactory(dataStoreType)).Returns(mockDataStoreFactory.Object);
            var accountService = new AccountService(mockAccountDataStoreFactoryService.Object);
            
            // Act
            accountService.UpdateAccount(request, account, dataStoreType);
            
            //Assert
            mockDataStoreFactory.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [TestMethod]
        public void UpdateAccount_AccountBalance_CorrectValue()
        {
            // Arrange
            var dataStoreType = "Backup";
            var accountBalance = 200;
            var account = new Account() { Balance = accountBalance };
            var paymentRequestAmount = 10;
            var request = new MakePaymentRequest() { Amount = paymentRequestAmount };

            var mockDataStoreFactory = new Mock<IAccountDataStoreFactory>();
            mockDataStoreFactory.Setup(x => x.UpdateAccount(It.IsAny<Account>())).Verifiable();
            var mockAccountDataStoreFactoryService = new Mock<IAccountDataStoreFactoryService>();
            mockAccountDataStoreFactoryService.Setup(x => x.GetAccountDataStoreFactory(dataStoreType)).Returns(mockDataStoreFactory.Object);
            var accountService = new AccountService(mockAccountDataStoreFactoryService.Object);
            
            // Act
            accountService.UpdateAccount(request, account, dataStoreType);
            
            //Assert
            Assert.AreEqual(accountBalance - paymentRequestAmount, account.Balance);
        }
    }
}
