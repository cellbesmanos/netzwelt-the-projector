
using TheProjector.Application.Shared;
using TheProjector.Core.Projects;
using TheProjector.Core.Shared;

namespace TheProjector.Models.Projects;

public class ViewProjectMembersControlsViewModel
{
  public Guid ProjectId { get; set; }
  public GetProjectMemberQuery ProjectOwner { get; set; }
  public PaginatedList<GetProjectMemberQuery> Members { get; set; }
  public IEnumerable<GetProjectNonMemberQuery> NonMembers { get; set; }
  public ViewProjectsMembersTableViewModel MembersTableModel { get; set; }
  public CommandResult CommandResult { get; set; }

  private readonly IProjectServices _projectServices;

  public ViewProjectMembersControlsViewModel() { }

  public ViewProjectMembersControlsViewModel(IProjectServices projectServices)
  {
    _projectServices = projectServices;
  }

  public async Task<ViewProjectMembersControlsViewModel> Build(Guid projectId, CommandResult commandResult)
  {
    this.CommandResult = commandResult;
    if (!commandResult.IsSuccessful) return this;

    var projectOwner = await _projectServices.GetProjectOwnerAsync(projectId);
    var members = await _projectServices.GetProjectMembersAsync(projectId, 1, 10);
    var nonMembers = await _projectServices.GetProjectNonMembersAsync(projectId);

    this.ProjectId = projectId;
    this.ProjectOwner = projectOwner;
    this.Members = members;
    this.NonMembers = nonMembers;
    this.MembersTableModel =
    new ViewProjectsMembersTableViewModel
    {
      ProjectId = projectId,
      ProjectOwner = projectOwner,
      Members = members,
    };

    return this;
  }
}