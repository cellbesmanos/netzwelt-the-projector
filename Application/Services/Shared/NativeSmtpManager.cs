using System.Net;
using System.Net.Mail;
using System.Text;
using TheProjector.Core.Shared;

namespace TheProjector.Application.Services.Shared;

public class NativeSmtpManager : ISmtpManager
{
  private readonly SmtpClient _smtpClient;

  public NativeSmtpManager(SmtpClient smtpClient)
  {
    _smtpClient = smtpClient;
  }

  public async Task<CommandResult> SendEmail(SendEmailCommand email)
  {
    using (var mailMessage = new MailMessage(email.From, email.To))
    {
      mailMessage.Subject = email.Subject;
      mailMessage.Body = email.Body;
      mailMessage.BodyEncoding = Encoding.UTF8;
      mailMessage.IsBodyHtml = true;


      await _smtpClient.SendMailAsync(mailMessage);
    }

    return CommandResult.Success();
  }
}