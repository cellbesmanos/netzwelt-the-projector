
using Microsoft.EntityFrameworkCore;

namespace TheProjector.Application.Shared;

public class PaginatedList<T> : List<T>
{
  private static int _maxPageSize = 15;

  public int TotalPages { get; init; }
  public int TotalCount { get; init; }
  public int PageNumber { get; init; }
  public int PageSize { get; init; }

  private PaginatedList(List<T> items, int totalPages, int totalCount, int pageNumber, int pageSize = 15)
  {
    TotalPages = totalPages;
    TotalCount = totalCount;
    PageNumber = pageNumber;
    PageSize = pageSize;

    this.AddRange(items);
  }

  public static async Task<PaginatedList<T>> Build(IQueryable<T> source, int pageNumber, int pageSize = 15)
  {
    var totalCount = source.Count();
    var ps = pageSize > _maxPageSize ? _maxPageSize : pageSize;
    var tp = (int)Math.Ceiling(totalCount / (double)pageSize);
    var totalPages = tp <= 0 ? 1 : tp;
    var pn = pageNumber > totalPages ? totalPages : Math.Max(1, pageNumber);

    return new PaginatedList<T>(
      await source.Skip(ps * (pn - 1)).Take(ps).ToListAsync(),
      totalPages,
      totalCount,
      pn,
      ps
      );
  }
}