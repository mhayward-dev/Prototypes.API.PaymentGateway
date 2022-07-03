using Prototypes.API.PaymentGateway.Enum;
using Prototypes.API.PaymentGateway.Models;

namespace Prototypes.API.PaymentGateway.Bank
{
    public class VisaBankService : IBankService
    {
        public bool IsBank(BankType bank) => bank == BankType.Visa;

        public Task<BankResponse> MakeDebitRequest(Payment payment)
        {
            throw new NotImplementedException();
        }
    }
}
