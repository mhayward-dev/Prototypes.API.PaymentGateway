namespace Prototypes.API.PaymentGateway.Models
{
    public class DatabaseResponse<T> where T : class
    {
        public string Id { get; set; } = default!;
        public T? Result { get; set; }
        public IEnumerable<T>? Results { get; set; }
        public bool IsSuccess { get; set; }
    }
}
