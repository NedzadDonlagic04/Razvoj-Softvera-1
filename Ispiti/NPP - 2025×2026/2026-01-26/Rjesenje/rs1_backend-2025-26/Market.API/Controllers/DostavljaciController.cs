using Market.Application.Modules.Catalog.Dostavljaci.Commands.Create;
using Market.Application.Modules.Catalog.Dostavljaci.Commands.Delete;
using Market.Application.Modules.Catalog.Dostavljaci.Commands.Update;
using Market.Application.Modules.Catalog.Dostavljaci.Queries.GetById;
using Market.Application.Modules.Catalog.Dostavljaci.Queries.List;

namespace Market.API.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class DostavljaciController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = "Staff")]
    public async Task<ActionResult<int>> Create(CreateDostavljacCommand command, CancellationToken ct)
    {
        int dostavljacId = await sender.Send(command, ct);

        return CreatedAtAction(nameof(GetById), new { id = dostavljacId }, new { id = dostavljacId });
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "Staff")]
    public async Task Update(int id, UpdateDostavljacCommand command, CancellationToken ct)
    {
        command.Id = id;
        await sender.Send(command, ct);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "Staff")]
    public async Task Delete(int id, CancellationToken ct)
    {
        await sender.Send(new DeleteDostavljacCommand { Id = id }, ct);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<GetDostavljacByIdQueryDto> GetById(int id, CancellationToken ct)
    {
        var dostavljac = await sender.Send(new GetDostavljacByIdQuery { Id = id }, ct);
        return dostavljac; 
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<PageResult<ListDostavljaciQueryDto>> List([FromQuery] ListDostavljaciQuery query, CancellationToken ct)
    {
        var dostavljaci = await sender.Send(query, ct);
        return dostavljaci;
    }
}
