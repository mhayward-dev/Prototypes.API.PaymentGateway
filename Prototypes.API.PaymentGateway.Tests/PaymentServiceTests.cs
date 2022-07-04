using Microsoft.Extensions.Configuration;
using Prototypes.API.PaymentGateway.Bank;
using Prototypes.API.PaymentGateway.Extensions;
using Prototypes.API.PaymentGateway.Models;
using Prototypes.API.PaymentGateway.Services;
using Prototypes.API.PaymentGateway.Tests.MockServices;

namespace Prototypes.API.PaymentGateway.Tests
{
    public class PaymentServiceTests
    {
        private readonly IPaymentService _paymentService;
        private readonly IBankFactory _bankFactory;
        private readonly IDatabaseService _databaseService;

        public PaymentServiceTests()
        {
            var services = new List<IBankService>() {
                new MockVisaBankService()
            };

            var configDict = new Dictionary<string, string>
             {
                {"Firebase:DatabaseEndpoint", "https://prototypes-ecb2c-default-rtdb.europe-west1.firebasedatabase.app/"}
             };

            var config = new ConfigurationBuilder().AddInMemoryCollection(configDict).Build();

            _bankFactory = new BankFactory(services);
            _databaseService = new FirebaseDatabaseService(config, new MockLogService<FirebaseDatabaseService>());
            _paymentService = new PaymentService(_bankFactory, _databaseService, new MockLogService<PaymentService>());
        }

        [Fact]
        public async void Should_mask_payment_details()
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

            var masked = mockPayment.ToMerchantResponse();

            Assert.Equal("************1111", masked.CardNumber);
            Assert.Equal("***", masked.CardCvv);
        }

        [Fact]
        public async void Should_add_payment_to_datastore()
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

            var mockResponse = new PaymentResponse
            {
                IsSuccess = true,
                BankResponseCode = "Test123",
                Message = "Accepted",
                DateCreated = DateTime.Now,
                Payment = mockPayment.ToMerchantResponse()
            };

            var response = await _paymentService.AddPayment(mockResponse);

            Assert.NotNull(response);
            Assert.True(response.Length > 1);
        }

        [Fact]
        public async void Should_retrieve_payment_from_datastore()
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

            var mockResponse = new PaymentResponse
            {
                IsSuccess = true,
                BankResponseCode = "Test123",
                Message = "Accepted",
                DateCreated = DateTime.Now,
                Payment = mockPayment.ToMerchantResponse()
            };

            var id = await _paymentService.AddPayment(mockResponse);
            var record = await _paymentService.GetPaymentById(id);

            Assert.NotNull(record);

            Assert.True(record.IsSuccess);
            Assert.Equal("Test123", record.BankResponseCode);
            Assert.Equal("Accepted", record.Message);
            Assert.Equal(mockResponse.DateCreated, record.DateCreated);
            Assert.Equal("Visa", record.Payment.CardType);
            Assert.Equal("************1111", record.Payment.CardNumber);
            Assert.Equal("***", record.Payment.CardCvv);
            Assert.Equal("01/25", record.Payment.CardExpiry);
            Assert.Equal(10.99m, record.Payment.Amount);
            Assert.Equal("GBP", record.Payment.Currency);
        }
    }
}
