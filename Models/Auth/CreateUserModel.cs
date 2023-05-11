using TheProjector.Core.Auth;
using TheProjector.Core.Users;

namespace TheProjector.Models.Auth;

public class CreateUserModel
{
  public string Message { get; set; }

  public SignupCommand Payload { get; set; }

  public IEnumerable<GetRolesQuery> Roles { get; set; }
}