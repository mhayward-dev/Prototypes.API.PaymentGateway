namespace Prototypes.API.PaymentGateway.Models
{
    public class Payment
    {
        public string CardType { get; set; } = default!; // Are bank and card types cross compatible? May need amending
        public string CardNumber { get; set; } = default!;
        public string CardExpiry { get; set; } = default!;
        public string CardCvv  { get; set; } = default!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = default!;
    }
}
