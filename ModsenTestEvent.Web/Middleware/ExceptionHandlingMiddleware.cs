namespace ModsenTestEvent.Web.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        if (exception is ValidationException validationException)
        {
            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            response.StatusCode = (int)HttpStatusCode.BadRequest; 
            var result = JsonSerializer.Serialize(new
            {
                error = errors,
                statusCode = response.StatusCode
            });
            return response.WriteAsync(result);
        }
        
        response.StatusCode = exception switch
        {   
            FileRequiredException => (int)HttpStatusCode.BadRequest,
            EmptyFileException => (int)HttpStatusCode.BadRequest,
            NotFoundException => (int)HttpStatusCode.NotFound,
            DuplicateEventException => (int)HttpStatusCode.BadRequest,
            DuplicateUserException => (int)HttpStatusCode.BadRequest,
            InvalidRefreshTokenException => (int)HttpStatusCode.Unauthorized,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var defaultResult = JsonSerializer.Serialize(new
        {
            error = exception.Message,
            statusCode = response.StatusCode
        });

        return response.WriteAsync(defaultResult);
    }
}