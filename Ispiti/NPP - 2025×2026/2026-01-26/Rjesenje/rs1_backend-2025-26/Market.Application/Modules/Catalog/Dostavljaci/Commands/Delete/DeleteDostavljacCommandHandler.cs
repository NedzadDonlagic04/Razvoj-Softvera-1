namespace Market.Application.Modules.Catalog.Dostavljaci.Commands.Delete;

public class DeleteDostavljacCommandHandler(
    IAppDbContext context,
    IAppCurrentUser appCurrentUser) : IRequestHandler<DeleteDostavljacCommand, Unit>
{
    public async Task<Unit> Handle(DeleteDostavljacCommand request, CancellationToken cancellationToken)
    {
        if (!appCurrentUser.IsAdmin)
            throw new MarketBusinessRuleException("123", "Samo admin moze brisati.");

        var dostavljac = await context.Dostavljaci
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (dostavljac is null)
            throw new MarketNotFoundException("Dostavljac nije pronađena.");

        context.Dostavljaci.Remove(dostavljac);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
