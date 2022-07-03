using Prototypes.API.PaymentGateway.Models;

namespace Prototypes.API.PaymentGateway.Services
{
    public interface IDatabaseService
    {
        Task<DatabaseResponse<T>> AddRecord<T>(string tableName, T entity) where T : class;

        Task<T> GetRecord<T>(string tableName, Guid id) where T : class;
    }
}
