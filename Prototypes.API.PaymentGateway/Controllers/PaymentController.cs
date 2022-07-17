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
                return new BadRequestObjectResult(new MerchantResponse(payment)
                {
                    IsSuccess = false,
                    Message = "Invalid Request",
                    Errors = validCheck.Errors.Select(e => e.ErrorMessage)
                });
            }

            var paymentResponse = await _paymentService.Debit(payment);
            var Id = await _paymentService.AddPayment(paymentResponse);

            if (Id == null)
                return new BadRequestResult();

            var merchantResponse = new MerchantResponse(payment)
            {
                TransactionId = Id,
                IsSuccess = paymentResponse.IsSuccess,
                Message = paymentResponse.Message
            };

            if (!paymentResponse.IsSuccess)
                return new BadRequestObjectResult(merchantResponse);

            return new CreatedResult(Id, merchantResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment([FromRoute] string id)
        {
            var clientName = "ClientName"; // TODO - get client name via auth key
            var payment = await _paymentService.GetPaymentById(id, clientName);

            if (payment == null)
                return new BadRequestObjectResult("Record not found");

            return new OkObjectResult(payment);
        }
    }
}
