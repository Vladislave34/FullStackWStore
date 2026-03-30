using Core.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Core.Services;

public class EmailSender(IConfiguration _config)  : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("WStore Support", _config["EmailSettings:From"]));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        var body = new BodyBuilder()
        {
            HtmlBody = message
        };
        emailMessage.Body = body.ToMessageBody();
        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(_config["EmailSettings:SmtpServer"],
                    int.Parse(_config["EmailSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_config["EmailSettings:From"], _config["EmailSettings:Password"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
    }
}