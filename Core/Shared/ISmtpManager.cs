
namespace TheProjector.Core.Shared;

public interface ISmtpManager
{
  Task<CommandResult> SendEmail(SendEmailCommand sendEmail);
}