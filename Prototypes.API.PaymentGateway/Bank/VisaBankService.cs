using Prototypes.API.PaymentGateway.Enums;
using Prototypes.API.PaymentGateway.Models;

namespace Prototypes.API.PaymentGateway.Bank
{
    public class VisaBankService : IBankService
    {
        public bool IsBank(BankType bank) => bank == BankType.Visa;

        public Task<BankResponse> MakeDebitRequest(Payment payment)
        {
            return Task.FromResult(new BankResponse { 
                BankResponseId = "TEST12345",
                IsSuccess = true,
                ResponseCode = "Accepted"
            });
        }
    }
}
