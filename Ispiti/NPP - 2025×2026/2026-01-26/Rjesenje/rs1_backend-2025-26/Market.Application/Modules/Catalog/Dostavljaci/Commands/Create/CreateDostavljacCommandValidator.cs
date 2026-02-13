namespace Market.Application.Modules.Catalog.Dostavljaci.Commands.Create;

public sealed class CreateDostavljacCommandValidator : AbstractValidator<CreateDostavljacCommand>
{
    public CreateDostavljacCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Dostavljac name is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Dostavljac code is required.")
            .Length(3).WithMessage("Dostavljac code must be 3 characters long.");

        RuleFor(x => x.Type)
            .IsInEnum();
    }
}
