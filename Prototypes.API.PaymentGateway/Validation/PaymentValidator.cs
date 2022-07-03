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
                .WithMessage("Card type is mandatory");

            RuleFor(p => p.CardNumber)
                .NotEmpty()
                .CreditCard()
                .WithMessage("Card number must be 16 digits long");

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
