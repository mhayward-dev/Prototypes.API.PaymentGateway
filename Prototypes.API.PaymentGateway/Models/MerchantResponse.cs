namespace Prototypes.API.PaymentGateway.Models
{
    public class MerchantResponse
    {
        public string TransactionId { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public Payment Payment { get; set; }
    }
}
