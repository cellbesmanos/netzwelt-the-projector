namespace TheProjector.Core.Projects;

public class GetProjectNonMemberQuery
{
  public Guid Id { get; init; }

  public string Firstname { get; init; }

  public string Lastname { get; init; }

  public string Fullname => $"{this.Firstname} {this.Lastname}";
}