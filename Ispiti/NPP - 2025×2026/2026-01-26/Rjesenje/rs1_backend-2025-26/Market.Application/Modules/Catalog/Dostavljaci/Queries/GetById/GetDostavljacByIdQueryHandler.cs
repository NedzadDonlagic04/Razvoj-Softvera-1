namespace Market.Application.Modules.Catalog.Dostavljaci.Queries.GetById;

public class GetDostavljacByIdQueryHandler(IAppDbContext context) : IRequestHandler<GetDostavljacByIdQuery, GetDostavljacByIdQueryDto>
{
    public async Task<GetDostavljacByIdQueryDto> Handle(GetDostavljacByIdQuery request, CancellationToken cancellationToken)
    {
        var q = context.Dostavljaci
            .Where(c => c.Id == request.Id);

        var dto = await q
            .Select(x => new GetDostavljacByIdQueryDto
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                IsActive = x.IsActive,
                Type = x.Type,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (dto == null)
        {
            throw new MarketNotFoundException($"Dostavljac with Id {request.Id} not found.");
        }

        return dto;
    }
}