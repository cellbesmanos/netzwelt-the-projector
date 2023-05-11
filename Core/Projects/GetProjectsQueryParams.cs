
namespace TheProjector.Core.Projects;

public class GetProjectsQueryParams
{
  private int _maxPageSize = 15;

  public int TotalPages { get; set; } = 1;

  public string Search { get; init; } = string.Empty;

  public string Sort { get; init; } = "asc";

  public int PageNumber { get; set; }

  private int _pageSize;
  public int PageSize
  {
    get => _pageSize;
    set => _pageSize = value > _maxPageSize || value == 0 ? _maxPageSize : value;
  }
}