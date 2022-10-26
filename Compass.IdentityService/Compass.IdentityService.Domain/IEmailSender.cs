using Compass.Wasm.Shared.IdentityService;

namespace Compass.IdentityService.Domain;
//发邮件，防腐接口（学习下MailKit）
public interface IEmailSender
{
    public Task SendAsync(string toName,string toEmail, string subject, string body);
    public Task MassSendAsync(List<EmailAddress> emails, string subject, string body);
}