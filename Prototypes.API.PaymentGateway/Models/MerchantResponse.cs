using Prototypes.API.PaymentGateway.Extensions;

namespace Prototypes.API.PaymentGateway.Models
{
    public class MerchantResponse
    {
        public MerchantResponse(Payment payment)
        {
            Payment = payment.RedactSensitiveData();
        }

        public string? TransactionId { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string> Errors { get; set; } = default!;
        public Payment? Payment { get; private set; }
    }
}
