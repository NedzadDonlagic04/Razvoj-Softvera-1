using Market.Domain.Entities.Fakture;

namespace Market.Application.Modules.Fakture.Commands.Create;

public class FakturaStavka 
{ 
    public int KategorijaId { get; set; }
    public string Proizvod { get; set; } = "";
    public int Kolicina { get; set; }
}

public class CreateFakturaCommand : IRequest<int>
{
    public string BrojRacuna { get; set; } = "";
    public FakturaTip Tip { get; set; }
    public string Napomena { get; set; } = "";
    public List<FakturaStavka> Items { get; set; } = []; 
}