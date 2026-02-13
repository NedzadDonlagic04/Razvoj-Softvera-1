namespace Market.Application.Modules.Catalog.Dostavljaci.Queries.List;

public sealed class ListDostavljaciQueryHandler(IAppDbContext ctx)
        : IRequestHandler<ListDostavljaciQuery, PageResult<ListDostavljaciQueryDto>>
{
    public async Task<PageResult<ListDostavljaciQueryDto>> Handle(
        ListDostavljaciQuery request, CancellationToken ct)
    {
        var q = ctx.Dostavljaci.AsNoTracking();

        var searchTerm = request.Search?.Trim().ToLower() ?? "";

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
             q = q.Where(x => x.Name.ToLower().Contains(searchTerm));
        }

        var projectedQuery = q.OrderBy(x => x.Name)
            .Select(x => new ListDostavljaciQueryDto
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                IsActive = x.IsActive,
                Type = x.Type,
            });

        return await PageResult<ListDostavljaciQueryDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);
    }


}
