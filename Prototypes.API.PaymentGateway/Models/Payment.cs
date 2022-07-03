namespace Prototypes.API.PaymentGateway.Models
{
    public class Payment
    {
        public string CardType { get; set; } // Are bank and card types cross compatibatible? May need amending
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
