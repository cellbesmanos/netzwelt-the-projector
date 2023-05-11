
namespace TheProjector.Application.Persistence;

public class ProjectAssignment
{
  public Guid UserId { get; set; }
  public User User { get; set; } = null!;

  public Guid ProjectId { get; set; }
  public Project Project { get; set; } = null!;

  public bool IsOwner { get; set; }
}