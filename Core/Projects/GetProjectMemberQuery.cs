namespace TheProjector.Core.Projects;

public class GetProjectMemberQuery
{
  public Guid Id { get; init; }

  public string Firstname { get; init; }

  public string Lastname { get; init; }

  public string Fullname => $"{this.Firstname} {this.Lastname}";
}