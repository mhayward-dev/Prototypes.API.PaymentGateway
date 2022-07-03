namespace Prototypes.API.PaymentGateway.Models
{
    public class MerchantResponse
    {
        public string CustomerReference { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
