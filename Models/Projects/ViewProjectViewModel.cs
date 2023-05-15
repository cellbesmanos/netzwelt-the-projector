
using TheProjector.Application.Shared;
using TheProjector.Core.Projects;

namespace TheProjector.Models.Projects;

public class ViewProjectViewModel
{
  public GetProjectQuery Project { get; set; }
  public GetProjectMemberQuery ProjectOwner { get; set; }
  public PaginatedList<GetProjectMemberQuery> Members { get; set; }
  public IEnumerable<GetProjectNonMemberQuery> NonMembers { get; set; }
  public ViewProjectMembersControlsViewModel ProjectMembersControlsModel { get; set; }

  private readonly IProjectServices _projectServices;

  public ViewProjectViewModel(IProjectServices projectServices)
  {
    _projectServices = projectServices;
  }

  public async Task<ViewProjectViewModel> Build(GetProjectQuery project)
  {

    var projectOwner = await _projectServices.GetProjectOwnerAsync(project.Id);
    var members = await _projectServices.GetProjectMembersAsync(project.Id, 1, 10);
    var nonMembers = await _projectServices.GetProjectNonMembersAsync(project.Id);

    this.Project = project;
    this.ProjectOwner = projectOwner;
    this.Members = members;
    this.NonMembers = nonMembers;
    this.ProjectMembersControlsModel =
     new ViewProjectMembersControlsViewModel
     {
       ProjectId = project.Id,
       ProjectOwner = projectOwner,
       Members = members,
       NonMembers = nonMembers,
       MembersTableModel =
      new ViewProjectsMembersTableViewModel
      {
        ProjectId = project.Id,
        ProjectOwner = projectOwner,
        Members = members,
      }
     };

    return this;
  }
}