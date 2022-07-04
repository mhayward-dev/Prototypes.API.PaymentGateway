using Prototypes.API.PaymentGateway.Models;

namespace Prototypes.API.PaymentGateway.Services
{
    public interface IDatabaseService
    {
        Task<DatabaseResponse<T>> AddRecord<T>(string tableName, T entity) where T : class;
        Task<DatabaseResponse<T>> GetRecordByKey<T>(string tableName, string key) where T : class;
    }
}
