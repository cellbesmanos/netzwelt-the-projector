using TheProjector.Core.Auth;

namespace TheProjector.Models.Auth;

public class RequestResetPasswordModel
{
  public string Message { get; set; }

  public string Email { get; set; }

  public RequestResetPasswordCommand Payload { get; set; }
}