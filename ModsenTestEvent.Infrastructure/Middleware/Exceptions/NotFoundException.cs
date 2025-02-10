namespace ModsenTestEvent.Infrastructure.Middleware.Exceptions;

public class NotFoundException(string message) : Exception(message);