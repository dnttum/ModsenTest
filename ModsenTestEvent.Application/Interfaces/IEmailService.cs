namespace ModsenTestEvent.Application.Interfaces;

public interface IEmailService
{
    public Task SendEmailAsync(int id, Event eventItem);
}