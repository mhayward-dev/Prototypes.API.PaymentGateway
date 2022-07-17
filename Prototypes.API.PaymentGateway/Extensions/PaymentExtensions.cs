using Prototypes.API.PaymentGateway.Models;
using System.Text.RegularExpressions;

namespace Prototypes.API.PaymentGateway.Extensions
{
    public static class PaymentExtensions
    {
        public static Payment RedactSensitiveData(this Payment payment)
        {
            payment.CardNumber = payment.CardNumber.Substring(payment.CardNumber.Length - 4).PadLeft(payment.CardNumber.Length, '*');
            payment.CardCvv = Regex.Replace(payment.CardCvv, "[0-9]", "*");

            return payment;
        }
    }
}
