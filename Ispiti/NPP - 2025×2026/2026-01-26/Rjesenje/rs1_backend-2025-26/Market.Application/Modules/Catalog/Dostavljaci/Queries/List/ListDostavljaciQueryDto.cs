namespace Market.Application.Modules.Catalog.Dostavljaci.Queries.List;

public sealed class ListDostavljaciQueryDto
{
    public required int Id { get; init; }
    public required string Name { get; set; } = "";
    public required DostavljacType Type { get; set; }
    public required string Code { get; set; } = "";
    public required bool IsActive { get; set; }
}
