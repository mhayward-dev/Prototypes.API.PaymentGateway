﻿namespace Prototypes.API.PaymentGateway.Models
{
    public class BankResponse
    {
        // TODO - scope out required response fields from banks
        public string BankResponseId { get; set; } = default!;
        public bool IsSuccess { get; set; }
        public string ResponseCode { get; set; } = default!;
    }
}
