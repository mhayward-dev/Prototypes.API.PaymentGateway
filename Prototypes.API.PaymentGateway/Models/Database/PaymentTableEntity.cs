using Azure;
using Azure.Data.Tables;

namespace Prototypes.API.PaymentGateway.Models.Database
{
    public class PaymentTableEntity : ITableEntity
    {
        public PaymentTableEntity() { }
        public PaymentTableEntity(PaymentResponse response)
        {
            if (response.Payment != null)
            {
                Amount = response.Payment.Amount;
                Currency = response.Payment.Currency;
                CardType = response.Payment.CardType;
                CardNumber = response.Payment.CardNumber;
                CardExpiry = response.Payment.CardExpiry;
                CardCvv = response.Payment.CardCvv;
            }

            IsSuccess = response.IsSuccess;
            BankResponseCode = response.BankResponseCode;
            Message = response.Message;
            DateCreated = response.DateCreated;
        }

        public string RowKey { get; set; } = default!;
        public string PartitionKey { get; set; } = default!;
        public string Name { get; init; } = default!;
        public int Quantity { get; init; }
        public bool Sale { get; init; }
        public ETag ETag { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;

        public string CardType { get; set; } = default!;
        public string CardNumber { get; set; } = default!;
        public string CardExpiry { get; set; } = default!;
        public string CardCvv { get; set; } = default!;
        public double Amount { get; set; }
        public string Currency { get; set; } = default!;

        public bool IsSuccess { get; set; }
        public string BankResponseCode { get; set; } = default!;
        public string Message { get; set; } = default!;
        public DateTime? DateCreated { get; set; }
    }
}
