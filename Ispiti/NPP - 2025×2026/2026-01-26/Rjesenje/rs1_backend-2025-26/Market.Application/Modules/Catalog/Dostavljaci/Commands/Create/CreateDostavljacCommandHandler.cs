namespace Market.Application.Modules.Catalog.Dostavljaci.Commands.Create;

public class CreateDostavljacCommandHandler(IAppDbContext ctx) : IRequestHandler<CreateDostavljacCommand, int>
{
    public async Task<int> Handle(CreateDostavljacCommand request, CancellationToken ct)
    {
        var normalizedName = request.Name?.Trim();
        var normalizedCode = request.Code?.Trim();

        if (string.IsNullOrWhiteSpace(normalizedName))
            throw new ValidationException("Name is required.");
        else if (string.IsNullOrWhiteSpace(normalizedCode))
            throw new ValidationException("Code is required.");

        bool exists = await ctx.Dostavljaci
            .AnyAsync(x => x.Code == normalizedCode, ct);

        if (exists)
        {
            throw new MarketConflictException("Code already exists.");
        }

        var dostavljac = new DostavljacEntity
        {
            Name = normalizedName,
            Code = normalizedCode,
            IsActive = request.IsActive,
            Type = request.Type,
        };

        ctx.Dostavljaci.Add(dostavljac);
        await ctx.SaveChangesAsync(ct);

        return dostavljac.Id;
    }
}
