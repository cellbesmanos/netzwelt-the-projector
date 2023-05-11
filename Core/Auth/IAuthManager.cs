using TheProjector.Core.Shared;

namespace TheProjector.Core.Auth;

public interface IAuthManager
{
  Task<CommandResult> SignUpAsync(SignupCommand payload);

  Task<CommandResult> LoginAsync(LoginUserCommand payload);

  Task<CommandResult> ActivateAccountAsync(Guid id, ChangePasswordCommand changePass);

  Task<CommandResult> DisableAccount(Guid id);

  Task<CommandResult> RequestResetPasswordAsync(RequestResetPasswordCommand payload);

  Task<CommandResult> CheckPasswordResetTokenValidityAsync(string token);

  Task<CommandResult> ChangePasswordAsync(string token, ChangePasswordCommand payload);
}