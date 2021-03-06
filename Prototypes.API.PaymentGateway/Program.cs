using Azure.Data.Tables;
using FluentValidation;
using Prototypes.API.PaymentGateway.Bank;
using Prototypes.API.PaymentGateway.Extensions;
using Prototypes.API.PaymentGateway.Middleware;
using Prototypes.API.PaymentGateway.Models;
using Prototypes.API.PaymentGateway.Services;
using Prototypes.API.PaymentGateway.Validation;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();

// Singletons
builder.Services.AddSingleton<IBankFactory, BankFactory>();

builder.Services.RegisterAllTypes<IBankService>(new[] { typeof(IBankService).Assembly });
builder.Services.AddSingleton<IPaymentService, PaymentService>();
builder.Services.AddSingleton(new TableServiceClient(builder.Configuration.GetValue<string>("Azure:DatabaseConnectionString")));
builder.Services.AddSingleton<IDatabaseService, AzureDatabaseService>();

// Scopes
builder.Services.AddScoped<IValidator<Payment>, PaymentValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Check for a valid API key using the API Key Middleware
app.UseBasicAuthApiKey();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
