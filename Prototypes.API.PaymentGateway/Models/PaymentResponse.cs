namespace Prototypes.API.PaymentGateway.Models
{
    public class PaymentResponse
    {
        public string CustomerReference { get; set; }
        public bool IsSuccess { get; set; }
        public string BankResponseId { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
        public Payment Payment { get; set; }
    }
}
