namespace ModsenTestEvent.Presentation.Middleware.Exceptions;

public class DuplicateUserException(string message) : Exception(message);
