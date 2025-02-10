namespace ModsenTestEvent.Infrastructure.Middleware.Exceptions;

public class InvalidRefreshTokenException(string message) : Exception(message);