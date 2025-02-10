namespace ModsenTestEvent.Infrastructure.Services;
[AutoInterface]

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly DataContext _context;

    public EmailService(IConfiguration configuration, DataContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public async Task SendEmailAsync(int id, Event eventItem)
    {
        var smtpServer = _configuration["SmtpSettings:Server"];
        var port = int.Parse(_configuration["SmtpSettings:Port"] ?? string.Empty);
        var username = _configuration["SmtpSettings:Username"];
        var password = _configuration["SmtpSettings:Password"];
        var enableSsl = bool.Parse(_configuration["SmtpSettings:EnableSsl"] ?? string.Empty);

        var participants = await _context.Participants
            .Where(p => p.EventId == id)
            .ToListAsync();

        if (!participants.Any()) return;

        foreach (var participant in participants)
        {
            var email = participant.Email;
            var subject = "Event changes";
            var message = $"Dear {participant.FirstName}, Our event:  {eventItem.Name} has been changed. Please look at our new changes \n" +
                          $"New Data: {eventItem.DateTime}\n" +
                          $"New Location: {eventItem.Location}\n" +
                          $"New Category: {eventItem.Category}";

            using (var client = new SmtpClient(smtpServer, port))
            {
                client.EnableSsl = enableSsl;
                client.UseDefaultCredentials = false;

                client.Credentials = new NetworkCredential(username, password);

                if (username != null)
                {
                    var mailMessage = new MailMessage(username, email, subject, message);
                    await client.SendMailAsync(mailMessage);
                }
            }
        }
    }
}
