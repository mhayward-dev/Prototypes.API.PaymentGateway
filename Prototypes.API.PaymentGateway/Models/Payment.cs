namespace Prototypes.API.PaymentGateway.Models
{
    public class Payment
    {
        public string CardType { get; set; } // Are bank and card types cross compatible? May need amending
        public string CardNumber { get; set; }
        public string CardExpiry { get; set; }
        public string CardCvv  { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
