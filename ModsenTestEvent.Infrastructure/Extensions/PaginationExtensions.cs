namespace ModsenTestEvent.Infrastructure.Extensions;

public static class PaginationExtensions
{
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, PageParamsDto pageParams)
    {
        var page = pageParams.Page ?? 1;
        var pageSize = pageParams.PageSize ?? 10;

        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }
}