using Microsoft.AspNetCore.Mvc;
using Prototypes.API.PaymentGateway.Models;
using Prototypes.API.PaymentGateway.Services;

namespace Prototypes.API.PaymentGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IDatabaseService _databaseService;

        public PaymentController(IPaymentService paymentService, IDatabaseService databaseService)
        {
            _paymentService = paymentService;
            _databaseService = databaseService;
        }

        [HttpPost("debit")]
        public async Task<IActionResult> Debit(Payment payment)
        {
            var paymentResponse = await _paymentService.Debit(payment);

            var paymentRecord = await _databaseService.AddRecord("Payments", paymentResponse);

            if (!paymentResponse.IsSuccess)
                return new BadRequestResult();

            return new CreatedResult(
                paymentResponse.CustomerReference,
                new MerchantResponse
                {
                    IsSuccess = paymentResponse.IsSuccess,
                    CustomerReference = paymentResponse.CustomerReference,
                    Message = paymentResponse.BankResponseCode // TODO - format appriopriate messaging to merchant
                });
        }

        [HttpGet]
        public async Task<IActionResult> GetPayment()
        {


            return new OkResult();
        }
    }
}
