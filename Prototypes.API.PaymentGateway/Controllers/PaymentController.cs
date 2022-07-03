using FluentValidation;
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
        private readonly IValidator<Payment> _paymentValidator;

        public PaymentController(IPaymentService paymentService, 
            IDatabaseService databaseService, 
            IValidator<Payment> paymentValidator)
        {
            _paymentService = paymentService;
            _databaseService = databaseService;
            _paymentValidator = paymentValidator;
        }

        [HttpPost("debit")]
        public async Task<IActionResult> Debit(Payment payment)
        {
            var validCheck = await _paymentValidator.ValidateAsync(payment);

            if (!validCheck.IsValid)
            {
                return new BadRequestObjectResult(new MerchantResponse
                {
                    IsSuccess = false,
                    Message = "Invalid Request",
                    Errors = validCheck.Errors.Select(e => e.ErrorMessage)
                });
            }

            var paymentResponse = await _paymentService.Debit(payment);

            await _databaseService.AddRecord("Payments", paymentResponse);

            var merchantResponse = new MerchantResponse
            {
                IsSuccess = paymentResponse.IsSuccess,
                CustomerReference = paymentResponse.CustomerReference,
                Message = paymentResponse.Message // TODO - format appriopriate messaging to merchant
            };

            if (!paymentResponse.IsSuccess)
                return new BadRequestObjectResult(merchantResponse);

            return new CreatedResult(
                paymentResponse.CustomerReference,
                merchantResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetPayment()
        {
            return new OkResult();
        }
    }
}
