using ClearBank.DeveloperTest.Config;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;

namespace ClearBank.DeveloperTest.Tests.Services
{
    [TestClass]
    public class PaymentServiceTests
    {
        [TestMethod]
        public void ValidatePayment_NullAccount_Returns_Invalid()
        {
            // Arrange
            var request = new MakePaymentRequest() { Amount = 10 };

            var mockAccountService = new Mock<IAccountService>();
            var mockAppSettings = new Mock<IAppSettings>();
            var service = new PaymentService(mockAppSettings.Object, mockAccountService.Object);
            
            // Act
            var isValidated = service.ValidatePayment(request, null);
            
            //Assert
            Assert.IsFalse(isValidated);
        }

        [TestMethod]
        public void ValidatePayment_RequestedBacsPayment_AccountAllowsBacsPayment_ReturnsValid()
        {
            // Arrange
            var account = new Account() {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };
            var request = new MakePaymentRequest() { Amount = 10, PaymentScheme = PaymentScheme.Bacs };

            var mockAccountService = new Mock<IAccountService>();
            var mockAppSettings = new Mock<IAppSettings>();
            var service = new PaymentService(mockAppSettings.Object, mockAccountService.Object);
            
            // Act
            var isValidated = service.ValidatePayment(request, account);
            
            //Assert
            Assert.IsTrue(isValidated);
        }

        [TestMethod]
        public void ValidatePayment_RequestedBacsPayment_AccountDoesntAllowsBacsPayment_ReturnsInvalid()
        {
            // Arrange
            var account = new Account();
            var request = new MakePaymentRequest() { Amount = 10, PaymentScheme = PaymentScheme.Bacs };

            var mockAccountService = new Mock<IAccountService>();
            var mockAppSettings = new Mock<IAppSettings>();
            var service = new PaymentService(mockAppSettings.Object, mockAccountService.Object);
            
            // Act
            var isValidated = service.ValidatePayment(request, account);
            
            //Assert
            Assert.IsFalse(isValidated);
        }

        [TestMethod]
        public void ValidatePayment_RequestedFasterPayment_PaymentAmountExceedsBalance_ReturnsInvalid()
        {
            // Arrange
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Balance = 4};
            var request = new MakePaymentRequest() { Amount = 10, PaymentScheme = PaymentScheme.FasterPayments };

            var mockAccountService = new Mock<IAccountService>();
            var mockAppSettings = new Mock<IAppSettings>();
            var service = new PaymentService(mockAppSettings.Object, mockAccountService.Object);
            
            // Act
            var isValidated = service.ValidatePayment(request, account);
            
            //Assert
            Assert.IsFalse(isValidated);
        }

        [TestMethod]
        public void ValidatePayment_RequestedFasterPayment_PaymentAmountBelowAccountBalance_ReturnsValid()
        {
            // Arrange
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Balance = 400};
            var request = new MakePaymentRequest() { Amount = 10, PaymentScheme = PaymentScheme.FasterPayments };

            var mockAccountService = new Mock<IAccountService>();
            var mockAppSettings = new Mock<IAppSettings>();
            var service = new PaymentService(mockAppSettings.Object, mockAccountService.Object);
            
            // Act
            var isValidated = service.ValidatePayment(request, account);
            
            //Assert
            Assert.IsTrue(isValidated);
        }

        [TestMethod]
        public void ValidatePayment_RequestedChapsPayment_AccountStatusIsLive_ReturnsValid()
        {
            // Arrange
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Balance = 400, Status = AccountStatus.Live };
            var request = new MakePaymentRequest() { Amount = 10, PaymentScheme = PaymentScheme.Chaps };

            var mockAccountService = new Mock<IAccountService>();
            var mockAppSettings = new Mock<IAppSettings>();
            var service = new PaymentService(mockAppSettings.Object, mockAccountService.Object);
            
            // Act
            var isValidated = service.ValidatePayment(request, account);
            
            //Assert
            Assert.IsTrue(isValidated);
        }

        [TestMethod]
        public void ValidatePayment_RequestedChapsPayment_AccountDoesntSupportChaps_ReturnsInvalid()
        {
            // Arrange
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs, Balance = 400, Status = AccountStatus.Live };
            var request = new MakePaymentRequest() { Amount = 10, PaymentScheme = PaymentScheme.Chaps };

            var mockAccountService = new Mock<IAccountService>();
            var mockAppSettings = new Mock<IAppSettings>();
            var service = new PaymentService(mockAppSettings.Object, mockAccountService.Object);
            
            // Act
            var isValidated = service.ValidatePayment(request, account);
            
            //Assert
            Assert.IsFalse(isValidated);
        }

        [TestMethod]
        public void ValidatePayment_RequestedChapsPayment_AccountStatusIsDisabled_ReturnsInvalid()
        {
            // Arrange
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Balance = 400, Status = AccountStatus.Disabled };
            var request = new MakePaymentRequest() { Amount = 10, PaymentScheme = PaymentScheme.Chaps };

            var mockAccountService = new Mock<IAccountService>();
            var mockAppSettings = new Mock<IAppSettings>();
            var service = new PaymentService(mockAppSettings.Object, mockAccountService.Object);
            
            // Act
            var isValidated = service.ValidatePayment(request, account);
            
            //Assert
            Assert.IsFalse(isValidated);
        }

        [TestMethod]
        public void MakePayment_Returns_Success_As_True()
        {
            // Arrange
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs, Balance = 400, Status = AccountStatus.Live };

            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<string>())).Returns(account);
            
            var mockAppSettings = new Mock<IAppSettings>();
            mockAppSettings.Setup(x => x.GetValue(It.IsAny<string>())).Returns(It.IsAny<string>);

            var service = new PaymentService(mockAppSettings.Object, mockAccountService.Object);
            
            // Act
            var request = new MakePaymentRequest() { Amount = 10, PaymentScheme = PaymentScheme.Bacs, DebtorAccountNumber= "123456" };
            var result = service.MakePayment(request);
            
            //Assert
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void MakePayment_UpdateAccountMethod_Called_Once()
        {
            // Arrange
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs, Balance = 400, Status = AccountStatus.Live };

            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<string>())).Returns(account);
            mockAccountService.Setup(x => x.UpdateAccount(It.IsAny<MakePaymentRequest>(), It.IsAny<Account>(), It.IsAny<string>())).Verifiable();
            
            var mockAppSettings = new Mock<IAppSettings>();
            mockAppSettings.Setup(x => x.GetValue(It.IsAny<string>())).Returns("Backup");

            var service = new PaymentService(mockAppSettings.Object, mockAccountService.Object);
            
            // Act
            var request = new MakePaymentRequest() { Amount = 10, PaymentScheme = PaymentScheme.Bacs, DebtorAccountNumber= "123456" };
            var result = service.MakePayment(request);
            
            //Assert
            mockAccountService.Verify(x => x.UpdateAccount(request, account, "Backup"), Times.Once);
        }
    }
}
