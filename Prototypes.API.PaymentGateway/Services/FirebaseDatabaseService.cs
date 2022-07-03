using Firebase.Database;
using Firebase.Database.Query;
using Prototypes.API.PaymentGateway.Models;

namespace Prototypes.API.PaymentGateway.Services
{
    public class FirebaseDatabaseService : IDatabaseService
    {
        private readonly IConfiguration _config;
        private readonly FirebaseClient _firebase;
        private readonly ILogger _logger;

        public FirebaseDatabaseService(IConfiguration config, ILogger logger)
        {
            _firebase = new FirebaseClient(_config.GetValue<string>("Firebase:DatabaseEndpoint"));
            _config = config;
            _logger = logger;
        }

        public async Task<DatabaseResponse<T>> AddRecord<T>(string tableName, T entity) where T : class
        {
            try
            {
                var result = await _firebase.Child(tableName).PostAsync(entity);

                return new DatabaseResponse<T>
                {
                    Id = result.Key,
                    Entity = result.Object,
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e.InnerException);

                return new DatabaseResponse<T>
                {
                    IsSuccess = false,
                };
            }
        }

        public async Task<T> GetRecord<T>(string tableName, Guid id) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
