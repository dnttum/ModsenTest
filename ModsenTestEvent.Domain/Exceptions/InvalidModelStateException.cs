namespace ModsenTestEvent.Domain.Exceptions;

public class InvalidModelStateException(IDictionary<string, string[]> errors) 
    : Exception("Model validation failed")
{
    public IDictionary<string, string[]> Errors { get; } = errors;
}