namespace Market.Application.Modules.Catalog.Dostavljaci.Commands.Update;

public sealed class UpdateDostavljacCommandValidator : AbstractValidator<UpdateDostavljacCommand>
{
    public UpdateDostavljacCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Dostavljac name is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Dostavljac code is required.")
            .Length(3).WithMessage("Dostavljac code must be 3 chars long.");

        RuleFor(x => x.Type)
            .IsInEnum();
    }
}
