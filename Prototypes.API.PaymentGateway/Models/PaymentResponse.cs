﻿namespace Prototypes.API.PaymentGateway.Models
{
    public class PaymentResponse
    {
        public string Id { get; set; }
        public string CustomerReference { get; set; }
        public bool IsSuccess { get; set; }
        public string BankResponseCode { get; set; }
        public DateTime DateCreated { get; set; }
        public Payment Payment { get; set; }
    }
}