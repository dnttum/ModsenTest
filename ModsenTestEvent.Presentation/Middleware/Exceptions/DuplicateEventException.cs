namespace ModsenTestEvent.Presentation.Middleware.Exceptions;

public class DuplicateEventException(string name) : Exception($"Event with name '{name}' already exists.");