
using TheProjector.Application.Shared;
using TheProjector.Models.Projects;
using TheProjector.Core.Users;
using TheProjector.Core.Projects;
using TheProjector.Core.Shared;

public class ProjectDetailsViewModel
{
  private readonly IUserServices _userServices;
  private readonly IProjectServices _projectServices;
  private readonly Guid _currentUserID;

  public ProjectDetailsViewModel(IUserServices userServices, IProjectServices projectServices, Guid currentUserID)
  {
    _userServices = userServices;
    _projectServices = projectServices;
    _currentUserID = currentUserID;
  }

  public async Task<ProjectMembersModel> CreateView(CommandResult commandResult, AddProjectMemberCommand payload)
  {
    var projectOwner = await _projectServices.GetProjectOwnerAsync(payload.ProjectId);
    var currentUserIsOwner = _currentUserID.Equals(projectOwner.Id);
    var members = (await _projectServices
      .GetProjectMembersAsync(payload.ProjectId))
      .Where(user => !user.Id.Equals(projectOwner.Id));
    var membersPagedList = PagedList<GetProjectMemberQuery>
      .ToPagedList(members, 1, 10);

    var nonMembers = await _projectServices.GetProjectNonMembersAsync(payload.ProjectId);

    return new ProjectMembersModel
    {
      // optionally add an error
      ProjectId = payload.ProjectId,
      CurrentUserIsOwner = currentUserIsOwner,
      ProjectNonMembers = nonMembers,
      ProjectMembers = membersPagedList
    };
  }

  public async Task<ProjectMembersModel> CreateView(CommandResult commandResult, RemoveProjectMemberCommand payload)
  {
    var projectOwner = await _projectServices.GetProjectOwnerAsync(payload.ProjectId);
    var currentUserIsOwner = _currentUserID.Equals(projectOwner.Id);
    var members = (await _projectServices
      .GetProjectMembersAsync(payload.ProjectId))
      .Where(user => !user.Id.Equals(projectOwner.Id));
    var membersPagedList = PagedList<GetProjectMemberQuery>
      .ToPagedList(members, 1, 10);

    var nonMembers = await _projectServices.GetProjectNonMembersAsync(payload.ProjectId);

    return new ProjectMembersModel
    {
      ProjectId = payload.ProjectId,
      CurrentUserIsOwner = currentUserIsOwner,
      ProjectNonMembers = nonMembers,
      ProjectMembers = membersPagedList
    };
  }
}