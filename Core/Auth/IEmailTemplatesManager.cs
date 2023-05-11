
namespace TheProjector.Core.Auth;

public interface IEmailTemplatesManager
{
  string GetAccountCreationTemplate(string temporaryPassword);

  string GetResetPasswordTemplate(string uniqueLink);
}