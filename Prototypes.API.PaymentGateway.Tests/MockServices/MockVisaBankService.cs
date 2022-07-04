using Prototypes.API.PaymentGateway.Bank;
using Prototypes.API.PaymentGateway.Enums;
using Prototypes.API.PaymentGateway.Models;

namespace Prototypes.API.PaymentGateway.Tests.MockServices
{
    public class MockVisaBankService : IBankService
    {
       public bool IsBank(BankType bank) => bank == BankType.Visa;

        public Task<BankResponse> MakeDebitRequest(Payment payment)
        {
            // request mocks here

            return Task.FromResult(new BankResponse
            {
                BankResponseId = "TEST12345",
                IsSuccess = true,
                ResponseCode = "Accepted"
            });
        }
    }
}
