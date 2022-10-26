using Compass.FileService.Infrastructure;
using Compass.Wasm.Shared.IdentityService;
using MailKit.Net.Smtp;
using MimeKit;

namespace Compass.IdentityService.Infrastructure;

public static class EmailSenderExtensions
{
    public static async Task SendEmailAsync(this EmailSender sender,SmtpOptions options, List<EmailAddress> emails, string subject, MimeEntity entity)
    {
        //构造邮件消息
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(options.Name, options.Address));
        foreach (var email in emails)
        {
            message.To.Add(new MailboxAddress(email.Name,email.Email));
        }
        message.Subject = subject;
        message.Body = entity;
        //发送消息
        using var smtp = new SmtpClient();
        smtp.ServerCertificateValidationCallback = (_, _, _, _) => true;
        await smtp.ConnectAsync(options.Host, options.Port);
        await smtp.AuthenticateAsync(options.Address, options.Password);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}