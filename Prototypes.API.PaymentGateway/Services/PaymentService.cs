using Prototypes.API.PaymentGateway.Bank;
using Prototypes.API.PaymentGateway.Enums;
using Prototypes.API.PaymentGateway.Extensions;
using Prototypes.API.PaymentGateway.Models;
using Prototypes.API.PaymentGateway.Models.Database;

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

                var paymentResponse = new PaymentResponse(payment)
                {
                    IsSuccess = bankResponse.IsSuccess,
                    BankResponseCode = bankResponse.BankResponseId,
                    Message = bankResponse.ResponseCode,
                    DateCreated = DateTime.UtcNow,
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

        public async Task<string?> AddPayment(PaymentResponse response)
        {
            var dbEntity = new PaymentTableEntity(response)
            {
                RowKey = Guid.NewGuid().ToString(),
                PartitionKey = "ClientName",
                Timestamp = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)
            };

            var dbResponse = await _databaseService.AddRecord("Payments", dbEntity);
            return dbResponse.Id;
        }

        public async Task<PaymentResponse?> GetPaymentById(string id, string clientName)
        {
            var dbResponse = await _databaseService.GetRecordByKey<PaymentTableEntity>("Payments", id, clientName);

            if (dbResponse.Result != null)
            {
                var result = dbResponse.Result;
                var payment = dbResponse.Result as Payment;

                return new PaymentResponse(payment)
                {
                    Id = result.RowKey,
                    IsSuccess = result.IsSuccess,
                    BankResponseCode = result.BankResponseCode,
                    Message = result.Message,
                    DateCreated = result.DateCreated
                };
            }

            return null;
        }
    }
}
