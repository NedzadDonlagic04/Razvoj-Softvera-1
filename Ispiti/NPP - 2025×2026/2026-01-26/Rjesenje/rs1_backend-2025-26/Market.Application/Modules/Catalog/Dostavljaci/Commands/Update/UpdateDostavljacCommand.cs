namespace Market.Application.Modules.Catalog.Dostavljaci.Commands.Update;

public sealed class UpdateDostavljacCommand : IRequest<Unit>
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public DostavljacType Type { get; set; }
    public string Code { get; set; } = "";
    public bool IsActive { get; set; }
}
