using Prototypes.API.PaymentGateway.Enums;

namespace Prototypes.API.PaymentGateway.Bank
{
    public interface IBankFactory
    {
        IBankService Create(BankType bank);
    }
}
