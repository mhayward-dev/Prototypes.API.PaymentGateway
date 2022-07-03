namespace Prototypes.API.PaymentGateway.Models
{
    public class DatabaseResponse<T> where T : class
    {
        public string? Id { get; set; }
        public bool IsSuccess { get; set; }

        public T? Entity { get; set; }
    }
}
