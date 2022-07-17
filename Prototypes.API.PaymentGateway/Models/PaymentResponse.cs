using Prototypes.API.PaymentGateway.Extensions;

namespace Prototypes.API.PaymentGateway.Models
{
    public class PaymentResponse
    {
        public PaymentResponse()
        {
        }

        public PaymentResponse(Payment payment)
        {
            Payment = payment.RedactSensitiveData();
        }

        public string Id { get; set; } = default!;
        public bool IsSuccess { get; set; }
        public string BankResponseCode { get; set; } = default!;
        public string Message { get; set; } = default!;
        public DateTime? DateCreated { get; set; }

        public Payment? Payment { get; private set; }
    }
}
