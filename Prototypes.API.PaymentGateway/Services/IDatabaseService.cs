using Azure.Data.Tables;
using Prototypes.API.PaymentGateway.Models;

namespace Prototypes.API.PaymentGateway.Services
{
    public interface IDatabaseService
    {
        Task<DatabaseResponse<T>> AddRecord<T>(string tableName, T entity) where T : class, ITableEntity;
        Task<DatabaseResponse<T>> GetRecordByKey<T>(string tableName, string key, string partitionKey) where T : class, ITableEntity, new();
    }
}
