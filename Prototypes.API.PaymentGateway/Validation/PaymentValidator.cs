using FluentValidation;
using Prototypes.API.PaymentGateway.Enums;
using Prototypes.API.PaymentGateway.Models;

namespace Prototypes.API.PaymentGateway.Validation
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
        // TODO - add more rules as per requirements
        public PaymentValidator()
        {
            RuleFor(p => p.CardType)
                .NotNull()
                .IsEnumName(typeof(BankType))
                .WithMessage("Card type is invalid");

            RuleFor(p => p.CardNumber)
                .NotEmpty()
                .CreditCard()
                .WithMessage("Card number must be 16 digits long");

            RuleFor(p => p.CardExpiry)
                .NotEmpty()
                .Length(5)
                .Matches(@"^(0[1-9]|1[0-2])\/?([0-9]{4}|[0-9]{2})$");

            RuleFor(p => p.CardCvv)
                .NotEmpty()
                .Length(3, 4); // TODO - add validation for specific conditions i.e. if Amex then must have length of 4

            RuleFor(p => p.Amount)
                .NotEmpty()
                .Must(amount => amount > 0 && amount <= 9999);

            RuleFor(p => p.Currency)
                .NotEmpty()
                .Length(3)
                .Must(BeAnIsoCurrency)
                .WithMessage("Currency \"{PropertyValue}\" is not a valid ISO currency");
        }

        private bool BeAnIsoCurrency(string currency)
        {
            // TODO - check against list of ISO currencies

            return true;
        }
    }
}
