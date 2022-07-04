using Prototypes.API.PaymentGateway.Bank;
using Prototypes.API.PaymentGateway.Enums;
using Prototypes.API.PaymentGateway.Extensions;
using Prototypes.API.PaymentGateway.Models;

namespace Prototypes.API.PaymentGateway.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBankFactory _bankFactory;
        private readonly IDatabaseService _databaseService;
        private readonly ILogger _logger;

        public PaymentService(IBankFactory bankFactory, IDatabaseService databaseService, ILogger<PaymentService> logger)
        {
            _bankFactory = bankFactory;
            _databaseService = databaseService;
            _logger = logger;
        }

        public async Task<PaymentResponse> Debit(Payment payment)
        {
            try
            {
                var bankService = _bankFactory.Create(Enum.Parse<BankType>(payment.CardType));
                var bankResponse = await bankService.MakeDebitRequest(payment);

                var paymentResponse = new PaymentResponse
                {
                    IsSuccess = bankResponse.IsSuccess,
                    BankResponseCode = bankResponse.BankResponseId,
                    Message = bankResponse.ResponseCode,
                    DateCreated = DateTime.Now,
                    Payment = payment.ToMerchantResponse() // Remove very sensitive data, do we store credit card numbers?
                };

                return paymentResponse;
            }
            catch (Exception e)
            {
                // TODO - implement a better logging service
                _logger.LogError(e.Message, e.InnerException);

                return new PaymentResponse
                {
                    IsSuccess = false,
                    Message = "An error occured processing the payment"
                };
            }
        }

        public async Task<string?> AddPayment(PaymentResponse payment)
        {
            var dbResponse = await _databaseService.AddRecord("Payments", payment);

            return dbResponse.Id;
        }

        public async Task<PaymentResponse?> GetPaymentById(string id)
        {
            var dbResponse = await _databaseService.GetRecordByKey<PaymentResponse>("Payments", id);

            if (dbResponse.IsSuccess)
                dbResponse.Result.Id = id;

            return dbResponse?.Result;
        }
    }
}
