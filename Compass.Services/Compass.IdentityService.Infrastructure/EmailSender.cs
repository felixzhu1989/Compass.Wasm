using System.Text;
using Compass.IdentityService.Domain;
using Compass.IdentityService.Infrastructure;
using Compass.Wasm.Shared.Identities;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Compass.FileService.Infrastructure;
//MailKit
//Install-Package MailKit
public class EmailSender : IEmailSender
{
    private readonly IOptionsSnapshot<SmtpOptions> _smtp;//使用依赖注入获取中心服务器中的smtp设置
    public EmailSender(IOptionsSnapshot<SmtpOptions> smtp)
    {
        _smtp = smtp;
    }
    /// <summary>
    /// 单发邮件
    /// </summary>
    public Task SendAsync(string toName,string toEmail, string subject, string body)
    {
        var email = new EmailAddress(toName, toEmail);//单个收件人
        //简单消息
        //var mineEntity= new TextPart(TextFormat.Plain) { Text = body };
        //HTML消息，Using a BodyBuilder
        StringBuilder message = new StringBuilder();
        message.Append("<p>来自Compass的消息：</p>");
        message.Append($"<p style=\"color:navy\"><b>{body}</b></p>");
        message.Append("<a href=\"http://10.9.18.31\">Compass</a>");
        var builder = new BodyBuilder
        {
            // Set the html version of the message text
            HtmlBody = message.ToString()
        };
        var mineEntity = builder.ToMessageBody();
        return this.SendEmailAsync(_smtp.Value, new List<EmailAddress> { email }, subject, mineEntity);
    }
    /// <summary>
    /// 群发邮件
    /// </summary>
    public Task MassSendAsync(List<EmailAddress> emails, string subject, string body)
    {
        //HTML消息，Using a BodyBuilder
        StringBuilder message=new StringBuilder();
        message.Append("<p>来自Compass的消息：</p>");
        message.Append($"<p style=\"color:navy\"><b>{body}</b></p>");
        message.Append("<a href=\"http://10.9.18.31\">Compass</a>");
        var builder = new BodyBuilder
        {
            // Set the html version of the message text
            HtmlBody = message.ToString()
        };
        var mineEntity = builder.ToMessageBody();
        return this.SendEmailAsync(_smtp.Value, emails, subject, mineEntity);
    }
}