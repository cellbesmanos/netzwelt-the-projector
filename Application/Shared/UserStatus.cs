
namespace TheProjector.Application.Shared;

public static class UserStatus
{
  public static Guid Active => new Guid("ca7a477d-ab7a-41e1-8d0c-86a91efd219d");

  public static Guid Pending => new Guid("95130666-173e-41d2-bfa1-6faae47355f8");

  public static Guid Disabled => new Guid("4ece0109-f9b9-4f55-822b-1524508e27f6");
}