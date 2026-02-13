using Market.Domain.Entities.Fakture;

namespace Market.Application.Modules.Fakture.Commands.Create;

public class CreateFakturaCommandHandler(
    IAppDbContext ctx) : IRequestHandler<CreateFakturaCommand, int>
{
    public async Task<int> Handle(CreateFakturaCommand request, CancellationToken ct)
    {
        if (request.Items.Count == 0)
        {
            throw new MarketConflictException("Broj stavki ne moze biti 0");
        }

        request.Items = NormalizeItems(request.Items);

        if (request.Tip == FakturaTip.Ulazna)
        {
            return await HandleUlaznaFaktura(request, ct);
        }

        return await HandleIzlaznaFaktura(request, ct);
    }

    private List<FakturaStavka> NormalizeItems(List<FakturaStavka> items)
    {
        var normalizedItems = items.GroupBy(item => (item.Proizvod, item.KategorijaId))
                                   .Select(group => new FakturaStavka
                                   {
                                        Proizvod = group.Key.Proizvod,
                                        KategorijaId = group.Key.KategorijaId,
                                        Kolicina = group.Sum(item => item.Kolicina)
                                   }).ToList();

        return normalizedItems;
    }

    private async Task<int> HandleUlaznaFaktura(CreateFakturaCommand request, CancellationToken ct)
    {
        var products = await GetProizvodiForStavke(request.Items, ct);
        var faktura = CreateFaktura(request);

        ctx.Fakture.Add(faktura);
        await ctx.SaveChangesAsync(ct);

        foreach (var stavka in request.Items)
        {
            var stavkaFakture = CreateStavkaFakture(faktura.Id, stavka);
            ctx.FakturaStavke.Add(stavkaFakture);

            var productKey = (stavka.Proizvod, stavka.KategorijaId);
            if (products.TryGetValue(productKey, out var product))
            {
                product.StockQuantity += stavka.Kolicina;
            }
            else
            {
                var newProduct = CreateProduct(stavka);
                ctx.Products.Add(newProduct);
            }
        }

        await ctx.SaveChangesAsync(ct);

        return faktura.Id;
    }

    private async Task<int> HandleIzlaznaFaktura(CreateFakturaCommand request, CancellationToken ct)
    {
        var products = await GetProizvodiForStavke(request.Items, ct);

        ThrowIfInvalidProductsForStavka(products, request.Items);
        
        var faktura = CreateFaktura(request);
        
        ctx.Fakture.Add(faktura);
        await ctx.SaveChangesAsync(ct);

        foreach (var stavka in request.Items)
        {
            var stavkaFakture = CreateStavkaFakture(faktura.Id, stavka);
            ctx.FakturaStavke.Add(stavkaFakture);

            var productKey = (stavka.Proizvod, stavka.KategorijaId);
            var product = products[productKey];

            product.StockQuantity -= stavka.Kolicina;
            product.IsEnabled = product.StockQuantity > 0;
        }

        await ctx.SaveChangesAsync(ct);

        return faktura.Id;
    }

    private async Task<Dictionary<(string, int), ProductEntity>> GetProizvodiForStavke(List<FakturaStavka> stavke, CancellationToken ct)
    {
        var stavkeProductNames = stavke.Select(stavka => stavka.Proizvod.ToLower()).Distinct().ToList();
        var stavkeCategories = stavke.Select(stavka => stavka.KategorijaId).Distinct().ToList();

        var products = await ctx.Products
            .Where(product => stavkeCategories.Contains(product.CategoryId)
                           && stavkeProductNames.Contains(product.Name.ToLower())
            )
            .ToListAsync(ct);

        return products
            .Where(product => stavke.Any(stavka =>
                stavka.KategorijaId == product.CategoryId &&
                stavka.Proizvod.Equals(product.Name, StringComparison.OrdinalIgnoreCase)))
            .ToDictionary(product => (product.Name, product.CategoryId));
    }

    private FakturaEntity CreateFaktura(CreateFakturaCommand request)
    {
        var faktura = new FakturaEntity
        {
            BrojRacuna = request.BrojRacuna,
            Napomena = request.Napomena,
            Tip = request.Tip,
        };

        return faktura;
    }

    private FakturaStavkaEntity CreateStavkaFakture(int fakturaId, FakturaStavka stavka)
    {
        var stavkaFakture = new FakturaStavkaEntity
        {
            FakturaId = fakturaId,
            ProductName = stavka.Proizvod,
            StockQuantity = stavka.Kolicina,
            CategoryId = stavka.KategorijaId
        };

        return stavkaFakture;
    }

    private ProductEntity CreateProduct(FakturaStavka stavka)
    {
        var product = new ProductEntity
        {
            Name = stavka.Proizvod,
            StockQuantity = stavka.Kolicina,
            CategoryId = stavka.KategorijaId,
            Description = "kreirano putem ulazne facture",
            IsEnabled = false
        };

        return product;
    }

    private void ThrowIfInvalidProductsForStavka(Dictionary<(string, int), ProductEntity> products, List<FakturaStavka> stavke)
    {
        if (products.Count != stavke.Count)
        {
            throw new MarketConflictException("Broj proizvoda pronadenih i broj stavki fakture nije jednak");
        }
        else if (stavke.Any(stavka => products[(stavka.Proizvod, stavka.KategorijaId)].StockQuantity - stavka.Kolicina < 0))
        {
            throw new MarketConflictException("Pronaden proizvod sa manjim brojem kolicine nego sto se trazi");
        }
    }
}
