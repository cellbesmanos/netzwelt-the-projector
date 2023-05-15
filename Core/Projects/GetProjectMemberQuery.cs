namespace TheProjector.Core.Projects;

public class GetProjectMemberQuery
{
  public Guid Id { get; init; }

  public string Firstname { get; init; }

  public string Lastname { get; init; }

  public string Fullname { get; init; }

  public string Email { get; init; }
}