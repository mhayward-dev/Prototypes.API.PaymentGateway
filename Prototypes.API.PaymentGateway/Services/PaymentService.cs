using Prototypes.API.PaymentGateway.Bank;
using Prototypes.API.PaymentGateway.Models;

namespace Prototypes.API.PaymentGateway.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBankFactory _bankFactory;
        private readonly ILogger _logger;

        public PaymentService(IBankFactory bankFactory, ILogger logger)
        {
            _bankFactory = bankFactory;
            _logger = logger;
        }

        public async Task<PaymentResponse> Debit(Payment payment)
        {
            try
            {
                var bankService = _bankFactory.Create(payment.CardType);
                var bankResponse = await bankService.MakeDebitRequest(payment);

                return new PaymentResponse
                {
                    CustomerReference = Guid.NewGuid().ToString(), // TODO - create a nice reference for the merchant and shopper
                    IsSuccess = bankResponse.IsSuccess,
                    BankResponseId = bankResponse.BankResponseId,
                    BankResponseCode = bankResponse.ResponseCode,
                };
            }
            catch(Exception e)
            {
                // TODO - implement a better logging service
                _logger.LogError(e.Message, e.InnerException);

                return new PaymentResponse
                {
                    IsSuccess = false,
                };
            }
        }

        public PaymentResponse GetPaymentById(Guid id)
        {
            return new PaymentResponse();
        }
    }
}
