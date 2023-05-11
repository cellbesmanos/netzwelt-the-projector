using TheProjector.Core.Users;

namespace TheProjector.Models.Users;

public class EditProfileModel
{
  public Guid Id { get; set; }

  public EditProfileCommand Payload { get; set; }
}