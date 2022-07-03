using Prototypes.API.PaymentGateway.Enum;

namespace Prototypes.API.PaymentGateway.Models
{
    public class Payment
    {
        public BankType CardType { get; set; } // Are bank and card types cross compatibatible? May need amending
        public string CardNumber { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
    }
}
