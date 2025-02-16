namespace ModsenTestEvent.Domain.Exceptions;

public class DuplicateUserException(string message) : Exception(message);
