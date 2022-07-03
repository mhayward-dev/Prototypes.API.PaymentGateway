using Prototypes.API.PaymentGateway.Enum;

namespace Prototypes.API.PaymentGateway.Bank
{
    public interface IBankFactory
    {
        IBankService Create(BankType bank);
    }
}
