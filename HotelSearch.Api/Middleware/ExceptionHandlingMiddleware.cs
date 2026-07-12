namespace HotelSearch.Api.Middleware
{
    /// <summary>
    /// Catches unhandled exceptions from any request and returns a clean error response
    /// instead of letting the default generic 500 error happen.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                // Thrown by our validation code (e.g. GeoLocation, HotelSearchParameters)
                // when input is invalid, so this becomes a 400 instead of a crash.
                context.Response.StatusCode = 400;

                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}