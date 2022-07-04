namespace Prototypes.API.PaymentGateway.Middleware
{
    public class ApiBasicAuthMiddleware
    {
        private string APIKeyHeaderName { get { return "Authorization"; } }
        private string APIKey { get { return "dGVzdDp0ZXN0"; } } // TODO - needs to be set client specific
        private readonly RequestDelegate _next;

        public ApiBasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.Keys.Contains(APIKeyHeaderName))
            {
                // The header has not been set correctly. Return 400: Bad request.
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync($"Error 400: Bad request. Please set {APIKeyHeaderName} header and the correct API key.");
                return;
            }

            if (!context.Request.Headers[APIKeyHeaderName].Equals($"Basic {APIKey}"))
            {
                // The api key was incorrect. Return 401: Unauthoried.
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Error 401: Unauthorized. Incorrect or no API key.");
                return;
            }

            // Call the next delegate/middleware in the pipeline
            await this._next(context);
        }
    }

    public static class ApiBasicAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseBasicAuthApiKey(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiBasicAuthMiddleware>();
        }
    }
}
