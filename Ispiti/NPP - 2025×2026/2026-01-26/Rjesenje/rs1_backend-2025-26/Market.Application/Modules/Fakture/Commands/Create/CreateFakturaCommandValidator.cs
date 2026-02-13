namespace Market.Application.Modules.Fakture.Commands.Create;

public sealed class CreateFakturaCommandValidator : AbstractValidator<CreateFakturaCommand>
{
    public CreateFakturaCommandValidator()
    {
        RuleFor(x => x.BrojRacuna)
            .NotEmpty().WithMessage("Broj racuna is required.");

        RuleFor(x => x.Tip)
            .IsInEnum();

        RuleForEach(x => x.Items).SetValidator(new CreateFakturaStavkaCommandValidator());
    }
}

public sealed class CreateFakturaStavkaCommandValidator : AbstractValidator<FakturaStavka>
{
    public CreateFakturaStavkaCommandValidator()
    {
        RuleFor(x => x.Proizvod)
            .NotEmpty().WithMessage("Proizvod is required.");

        RuleFor(x => x.KategorijaId)
            .GreaterThan(0).WithMessage("Kategorija Id must be greater than 0.");

        RuleFor(x => x.Kolicina)
            .GreaterThan(0).WithMessage("Kolicina must be greater than 0.");
    }
}
