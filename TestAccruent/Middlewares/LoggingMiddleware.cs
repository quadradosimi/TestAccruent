namespace TestAccruent.Middlewares
{
    // Middleware for logging incoming requests and outgoing responses
    public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<LoggingMiddleware> _logger = logger;

        // Method invoked by the middleware pipeline to handle the request
        public async Task InvokeAsync(HttpContext context)
        {
            var controllerName = context?.GetRouteValue("controller")?.ToString();
            var actionName = context?.GetRouteValue("action")?.ToString();
            var parameters = context?.Request.Query.Select(p => $"{p.Key}={p.Value}").ToList();

            // Logging the start of method execution
            _logger.LogInformation($"{controllerName}: {actionName}: {string.Join(", ", parameters)} execution started.");

            await _next(context);

            // Logging the end of method execution
            _logger.LogInformation($"{controllerName}: {actionName}: {string.Join(", ", parameters)} execution done.");
        }
    }
}
