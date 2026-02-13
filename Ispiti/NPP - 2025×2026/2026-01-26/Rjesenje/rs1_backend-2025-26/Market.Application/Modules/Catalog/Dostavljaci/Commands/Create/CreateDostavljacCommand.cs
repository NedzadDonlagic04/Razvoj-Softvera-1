namespace Market.Application.Modules.Catalog.Dostavljaci.Commands.Create;

public class CreateDostavljacCommand : IRequest<int>
{
    public string Name { get; set; } = "";
    public DostavljacType Type { get; set; }
    public string Code { get; set; } = "";
    public bool IsActive { get; set; }
}