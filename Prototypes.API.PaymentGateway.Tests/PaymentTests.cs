using Prototypes.API.PaymentGateway.Models;
using FluentValidation;
using FluentValidation.TestHelper;
using Prototypes.API.PaymentGateway.Validation;

namespace Prototypes.API.PaymentGateway.Tests
{
    public class PaymentTests
    {
        private PaymentValidator validator;

        public PaymentTests()
        {
            validator = new PaymentValidator();
        }

        [Fact]
        public void Should_validate_all_payment_fields()
        {
            // TODO - each validation for each property should be tested, but single cases are tested below.
            var mockPayment = new Payment
            {
                CardType = "Maestro", // not supported
                CardNumber = "4111111", // invalid length
                CardCvv = "44444", // too long
                CardExpiry = "01/20", // in the past
                Amount = 0, // can't be zero
                Currency = "ZZZZ" // invalid length
            };

            var result = validator.TestValidate(mockPayment);

            result.ShouldHaveValidationErrorFor(p => p.CardType);
            result.ShouldHaveValidationErrorFor(p => p.CardNumber);
            result.ShouldHaveValidationErrorFor(p => p.CardCvv);
            result.ShouldHaveValidationErrorFor(p => p.CardExpiry);
            result.ShouldHaveValidationErrorFor(p => p.Amount);
            result.ShouldHaveValidationErrorFor(p => p.Currency);
        }
    }
}