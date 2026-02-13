namespace Market.Application.Modules.Catalog.Dostavljaci.Queries.List;

public sealed class ListDostavljaciQuery : BasePagedQuery<ListDostavljaciQueryDto>
{
    public string? Search { get; init; }
}
