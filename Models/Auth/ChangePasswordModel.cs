using TheProjector.Core.Auth;

namespace TheProjector.Models.Auth;

public class ChangePasswordModel
{
  public string Token { get; set; }

  public ChangePasswordCommand Payload { get; set; }
}