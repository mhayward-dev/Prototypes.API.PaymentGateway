using Prototypes.API.PaymentGateway.Enums;
using Prototypes.API.PaymentGateway.Models;

namespace Prototypes.API.PaymentGateway.Bank
{
    public interface IBankService
    {
        bool IsBank(BankType bank);
        Task<BankResponse> MakeDebitRequest(Payment payment);
    }
}
