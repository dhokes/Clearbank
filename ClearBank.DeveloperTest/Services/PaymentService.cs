using ClearBank.DeveloperTest.Config;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAppSettings appSettings;
        private readonly IAccountService accountService;

        /// <summary>
        /// The payment service.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        public PaymentService(IAppSettings appSettings, IAccountService accountService)
        {
            this.appSettings = appSettings;
            this.accountService = accountService;
        }

        /// <inheritdoc />
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var dataStoreType = appSettings.GetValue("DataStoreType");

            var account = accountService.GetAccount(dataStoreType, request.DebtorAccountNumber);

            var result = new MakePaymentResult();

            result.Success = ValidatePayment(request, account);

            if (result.Success)
            {
                accountService.UpdateAccount(request, account, dataStoreType);
            }

            return result;
        }

        // TODO: This could be extracted to its own class.
        /// <inheritdoc />
        public bool ValidatePayment(MakePaymentRequest request, Account account)
        {
            if (account == null)
            {
                return false;
            }

            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);

                case PaymentScheme.FasterPayments:
                    return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) &&
                        account.Balance >= request.Amount;

                case PaymentScheme.Chaps:
                    return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) &&
                        account.Status == AccountStatus.Live;

                default:
                    return false;
            }
        }
    }
}
