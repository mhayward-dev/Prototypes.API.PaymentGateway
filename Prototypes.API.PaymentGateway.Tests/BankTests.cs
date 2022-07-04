using Prototypes.API.PaymentGateway.Bank;
using Prototypes.API.PaymentGateway.Enums;
using Prototypes.API.PaymentGateway.Models;
using Prototypes.API.PaymentGateway.Tests.MockServices;

namespace Prototypes.API.PaymentGateway.Tests
{
    public class BankTests
    {
        private readonly IBankFactory _bankFactory;
        private readonly MockVisaBankService mockVisaBankService;

        public BankTests()
        {
            var services = new List<IBankService>() { 
                new MockVisaBankService() 
            };

            _bankFactory = new BankFactory(services);
            mockVisaBankService = new MockVisaBankService();
        }

        [Fact]
        public void Should_create_visa_service()
        {
            var mockPayment = new Payment
            {
                CardType = "Visa",
                CardNumber = "4111111111111111",
                CardCvv = "123",
                CardExpiry = "01/25",
                Amount = 10.99m,
                Currency = "GBP"
            };

            var bankType = Enum.Parse<BankType>(mockPayment.CardType);
            var bankService = _bankFactory.Create(bankType);

            Assert.True(bankType == BankType.Visa);
            Assert.NotNull(bankService);
        }

        [Fact]
        public void Should_make_request_to_visa_bank()
        {
            var mockPayment = new Payment
            {
                CardType = "Visa",
                CardNumber = "4111111111111111",
                CardCvv = "123",
                CardExpiry = "01/25",
                Amount = 10.99m,
                Currency = "GBP"
            };

            Assert.NotNull(mockVisaBankService.MakeDebitRequest(mockPayment));
        }
    }
}
