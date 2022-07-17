using Azure.Data.Tables;
using Prototypes.API.PaymentGateway.Models;

namespace Prototypes.API.PaymentGateway.Services
{
    public class AzureDatabaseService : IDatabaseService
    {
        private readonly TableServiceClient tableServiceClient;
        private readonly ILogger _logger;

        public AzureDatabaseService(TableServiceClient tableServiceClient, ILogger<AzureDatabaseService> logger)
        {
            this.tableServiceClient = tableServiceClient;
            _logger = logger;
        }

        public async Task<DatabaseResponse<T>> AddRecord<T>(string tableName, T entity) where T : class, ITableEntity
        {
            try
            {
                var tableClient = tableServiceClient.GetTableClient(tableName);
                await tableClient.CreateIfNotExistsAsync();

                var response = await tableClient.AddEntityAsync(entity);

                return new DatabaseResponse<T>
                {
                    Id = entity.RowKey,
                    Result = entity,
                    IsSuccess = response.Status == 200
                };
            }
            catch(Exception e)
            {
                _logger.LogError("AddRecord failed", e.InnerException);
                return new DatabaseResponse<T> { IsSuccess = false };
            }
        }

        public async Task<DatabaseResponse<T>> GetRecordByKey<T>(string tableName, string key, string partitionKey) where T : class, ITableEntity, new()
        {
            try
            {
                var tableClient = tableServiceClient.GetTableClient(tableName);
                var response = await tableClient.GetEntityAsync<T>(partitionKey, key);

                return new DatabaseResponse<T>
                {
                    Id = key,
                    Result = response.Value,
                    IsSuccess = true
                };
            }
            catch (Exception e) 
            {
                _logger.LogError("GetRecordByKey failed", e.InnerException);
                return new DatabaseResponse<T> { IsSuccess = false };
            }
        }
    }
}
