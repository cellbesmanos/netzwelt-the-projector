using TheProjector.Core.Auth;

namespace TheProjector.Models.Auth;

public class LoginModel
{
  public string Message { get; set; }

  public LoginUserCommand Payload { get; set; }
}