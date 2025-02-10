namespace ModsenTestEvent.Infrastructure.Middleware.Exceptions;

public class DuplicateUserException(string message) : Exception(message);
