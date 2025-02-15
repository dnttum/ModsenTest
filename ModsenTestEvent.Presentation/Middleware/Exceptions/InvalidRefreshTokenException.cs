namespace ModsenTestEvent.Presentation.Middleware.Exceptions;

public class InvalidRefreshTokenException(string message) : Exception(message);