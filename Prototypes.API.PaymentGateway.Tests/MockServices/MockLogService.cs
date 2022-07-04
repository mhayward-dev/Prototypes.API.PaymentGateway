using Microsoft.Extensions.Logging;

namespace Prototypes.API.PaymentGateway.Tests.MockServices
{
    public class MockLogService<T> : ILogger<T>, IDisposable
    {
        public MockLogService()
        {
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}
