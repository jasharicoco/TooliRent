using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TooliRent.Services.Services.Interfaces;

public class SmtpEmailService : IEmailService
{
    private readonly string _smtpHost = "smtp.gmail.com";
    private readonly int _smtpPort = 587;
    private readonly string _smtpUser = "kallblodstravarnaresort@gmail.com";
    private readonly string _smtpPass = "udwdbnjtnofxoefo";

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient(_smtpHost, _smtpPort)
        {
            Credentials = new NetworkCredential(_smtpUser, _smtpPass),
            EnableSsl = true
        };

        var mail = new MailMessage(_smtpUser, to, subject, body);
        await client.SendMailAsync(mail);
    }
}
