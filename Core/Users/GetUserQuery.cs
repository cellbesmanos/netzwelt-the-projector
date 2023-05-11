
namespace TheProjector.Core.Users;

public class GetUserQuery
{
  public Guid Id { get; init; }

  public string Email { get; init; }

  public string Firstname { get; init; }

  public string Lastname { get; init; }

  public string Role { get; init; }

  public string Status { get; init; }

  public string Locale { get; init; }

  public string Fullname
  {
    get => $"{this.Firstname} {this.Lastname}";
  }
}