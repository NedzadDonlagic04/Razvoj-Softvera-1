namespace Market.Application.Modules.Catalog.Dostavljaci.Commands.Update;

public sealed class UpdateDostavljacCommandHandler(
    IAppDbContext ctx) : IRequestHandler<UpdateDostavljacCommand, Unit>
{
    public async Task<Unit> Handle(UpdateDostavljacCommand request, CancellationToken ct)
    {
        var normalizedName = request.Name?.Trim();
        var normalizedCode = request.Code?.Trim();

        if (string.IsNullOrWhiteSpace(normalizedName))
            throw new ValidationException("Name is required.");
        else if (string.IsNullOrWhiteSpace(normalizedCode))
            throw new ValidationException("Code is required.");

        var dostavljac = await ctx.Dostavljaci
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(ct);

        if (dostavljac is null)
            throw new MarketNotFoundException($"Dostavljac (ID={request.Id}) nije pronađena.");

        bool exists = await ctx.Dostavljaci
            .AnyAsync(x => x.Id != request.Id && x.Code == normalizedCode, ct);

        if (exists)
        {
            throw new MarketConflictException("Code already exists.");
        }

        dostavljac.Name = normalizedName;
        dostavljac.Code = normalizedCode;
        dostavljac.Type = request.Type;
        dostavljac.IsActive = request.IsActive;

        await ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
