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
        public void Should_reject_bad_payment_fields()
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

        [Fact]
        public void Should_accept_valid_payment_fields()
        {
            var mockPayment = new Payment
            {
                CardType = "Visa",
                CardNumber = "4111111111111111",
                CardCvv = "123",
                CardExpiry= "01/25",
                Amount = 10.99m,
                Currency = "GBP"
            };

            var result = validator.TestValidate(mockPayment);

            result.ShouldNotHaveValidationErrorFor(p => p.CardType);
            result.ShouldNotHaveValidationErrorFor(p => p.CardNumber);
            result.ShouldNotHaveValidationErrorFor(p => p.CardCvv);
            result.ShouldNotHaveValidationErrorFor(p => p.Amount);
        }
    }
}