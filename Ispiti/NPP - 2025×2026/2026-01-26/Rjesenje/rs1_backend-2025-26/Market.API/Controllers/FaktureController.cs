using Market.Application.Modules.Fakture.Commands.Create;
using Market.Application.Modules.Fakture.Queries.List;

namespace Market.API.Controllers;

[ApiController]
[Route("[controller]")]
public class FaktureController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateFakturaCommand command, CancellationToken ct)
    {
        var fakturaId = await sender.Send(command, ct);

        return new ObjectResult(fakturaId)
        { 
            StatusCode = StatusCodes.Status201Created
        };
    }

    [HttpGet]
    public async Task<PageResult<ListFaktureQueryDto>> List([FromQuery] ListFaktureQuery query, CancellationToken ct)
    {
        var fakture = await sender.Send(query, ct);
        return fakture;
    }
}
