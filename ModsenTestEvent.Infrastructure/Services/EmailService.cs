namespace ModsenTestEvent.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IGetRangeByEventIdUseCase _getRangeUseCase; 
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly string _username;
    private readonly string _password;
    private readonly bool _enableSsl;

    public EmailService(IConfiguration configuration, IGetRangeByEventIdUseCase getRangeUseCase)
    {
        _getRangeUseCase = getRangeUseCase;
        _smtpServer = configuration["SmtpSettings:Server"] ?? string.Empty;
        _port = int.Parse(configuration["SmtpSettings:Port"] ?? string.Empty);
        _username = configuration["SmtpSettings:Username"] ?? string.Empty;
        _password = configuration["SmtpSettings:Password"] ?? string.Empty;
        _enableSsl = bool.Parse(configuration["SmtpSettings:EnableSsl"] ?? string.Empty);
    }

    public async Task SendEmailAsync(int id, Event eventItem)
    {
        var participants = await _getRangeUseCase.ExecuteAsync(id);

        var participantDtos = participants.ToList();
        
        if (participants is null || !participants.Any())
        {
            throw new NotFoundException($"No participants found for event with ID {id}");
        }

        foreach (var participant in participantDtos)
        {
            var email = participant.Email;
            var subject = "Event changes";
            var message = $"Dear {participant.FirstName}, Our event:  {eventItem.Name} has been changed. Please look at our new changes \n" +
                          $"New Data: {eventItem.DateTime}\n" +
                          $"New Location: {eventItem.Location}\n" +
                          $"New Category: {eventItem.Category}";

            using (var client = new SmtpClient(_smtpServer, _port))
            {
                client.EnableSsl = _enableSsl;
                client.UseDefaultCredentials = false;

                client.Credentials = new NetworkCredential(_username, _password);

                if (_username != null)
                {
                    var mailMessage = new MailMessage(_username, email, subject, message);
                    await client.SendMailAsync(mailMessage);
                }
            }
        }
    }
}
