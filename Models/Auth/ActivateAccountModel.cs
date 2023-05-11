using TheProjector.Core.Auth;

namespace TheProjector.Models.Auth;

public class ActivateAccountModel
{
  public Guid Id { get; set; }

  public ChangePasswordCommand Payload { get; set; }
}