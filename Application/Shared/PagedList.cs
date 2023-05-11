
namespace TheProjector.Application.Shared;

public class PagedList<T> : List<T>
{
  public int TotalCount { get; init; }
  public int PageSize { get; init; }
  public int CurrentPage { get; init; }
  public int TotalPages { get; init; }

  private PagedList(List<T> items, int count, int pageNumber, int pageSize)
  {
    TotalCount = count;
    PageSize = pageSize;
    CurrentPage = pageNumber;

    var total = (int)Math.Ceiling(count / (double)PageSize);
    TotalPages = total <= 0 ? 1 : total;

    AddRange(items);
  }

  public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
  {
    var count = source.Count();

    var items = source
    .Skip(pageSize * (pageNumber - 1))
    .Take(pageSize)
    .ToList();

    return new PagedList<T>(items, count, pageNumber, pageSize);
  }
}
