using Azure.Data.Tables;
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
                {"Azure:DatabaseEndpoint", "DefaultEndpointsProtocol=https;AccountName=prototypes-instance;AccountKey=UcO3Tq5AaFkMygdM2GFokFgQ4HHEgHxbrdAkxznw95WeAf8MFWbYAoUqgdBAdLsMEmPwYTzPtWXf5LRIbdI9jA==;TableEndpoint=https://prototypes-instance.table.cosmos.azure.com:443/;"}
             };

            var config = new ConfigurationBuilder().AddInMemoryCollection(configDict).Build();

            _bankFactory = new BankFactory(services);
            _databaseService = new AzureDatabaseService(new TableServiceClient(configDict.GetValueOrDefault("Azure:DatabaseEndpoint")), new MockLogService<AzureDatabaseService>());
            _paymentService = new PaymentService(_bankFactory, _databaseService, new MockLogService<PaymentService>());
        }

        [Fact]
        public void Should_mask_payment_details()
        {
            var mockPayment = new Payment
            {
                CardType = "Visa",
                CardNumber = "4111111111111111",
                CardCvv = "123",
                CardExpiry = "01/25",
                Amount = 10.99,
                Currency = "GBP"
            };

            var masked = new PaymentResponse(mockPayment);

            Assert.Equal("************1111", masked.Payment.CardNumber);
            Assert.Equal("***", masked.Payment.CardCvv);
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
                Amount = 10.99,
                Currency = "GBP"
            };

            var mockResponse = new PaymentResponse(mockPayment)
            {
                IsSuccess = true,
                BankResponseCode = "Test123",
                Message = "Accepted",
                DateCreated = DateTime.UtcNow
            };

            var response = await _paymentService.AddPayment(mockResponse);

            Assert.NotNull(response);
            Assert.True(response.Length > 1);
        }

        [Fact]
        public async void Should_retrieve_payment_from_datastore()
        {
            var mockClientName = "ClientName";
            var mockPayment = new Payment
            {
                CardType = "Visa",
                CardNumber = "4111111111111111",
                CardCvv = "123",
                CardExpiry = "01/25",
                Amount = 10.99,
                Currency = "GBP"
            };

            var mockResponse = new PaymentResponse(mockPayment)
            {
                IsSuccess = true,
                BankResponseCode = "Test123",
                Message = "Accepted",
                DateCreated = DateTime.UtcNow
            };

            var id = await _paymentService.AddPayment(mockResponse);
            var record = await _paymentService.GetPaymentById(id, "ClientName");

            Assert.NotNull(record);

            Assert.True(record.IsSuccess);
            Assert.Equal("Test123", record.BankResponseCode);
            Assert.Equal("Accepted", record.Message);
            Assert.Equal(mockResponse.DateCreated, record.DateCreated);
            Assert.Equal("Visa", record.Payment.CardType);
            Assert.Equal("************1111", record.Payment.CardNumber);
            Assert.Equal("***", record.Payment.CardCvv);
            Assert.Equal("01/25", record.Payment.CardExpiry);
            Assert.Equal(10.99, record.Payment.Amount);
            Assert.Equal("GBP", record.Payment.Currency);
        }
    }
}
