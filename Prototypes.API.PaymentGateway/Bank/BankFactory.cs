using Prototypes.API.PaymentGateway.Enum;

namespace Prototypes.API.PaymentGateway.Bank
{
    public class BankFactory : IBankFactory
    {
        private readonly IEnumerable<IBankService> _bankServices;

        public BankFactory(IEnumerable<IBankService> bankServices)
        {
            _bankServices = bankServices;
        }

        public IBankService Create(BankType bank)
        {
            return _bankServices.Single(b => b.IsBank(bank));
        }
    }
}
