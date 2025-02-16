namespace ModsenTestEvent.Domain.Exceptions;

public class DuplicateEventException(string name) : Exception($"Event with name '{name}' already exists.");