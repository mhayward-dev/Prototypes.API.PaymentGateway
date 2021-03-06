using Prototypes.API.PaymentGateway.Enums;
using Prototypes.API.PaymentGateway.Models;

namespace Prototypes.API.PaymentGateway.Bank
{
    public class AmericanExpressBankService : IBankService
    {
        public bool IsBank(BankType bank) => bank == BankType.AmericanExpress;

        public Task<BankResponse> MakeDebitRequest(Payment payment)
        {
            // TODO - implement bank logic

            throw new NotImplementedException();
        }
    }
}
