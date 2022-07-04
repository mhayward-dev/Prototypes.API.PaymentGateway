using Prototypes.API.PaymentGateway.Models;

namespace Prototypes.API.PaymentGateway.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponse> Debit(Payment payment);
        Task<string?> AddPayment(PaymentResponse payment);
        Task<PaymentResponse?> GetPaymentById(string id);
    }
}
