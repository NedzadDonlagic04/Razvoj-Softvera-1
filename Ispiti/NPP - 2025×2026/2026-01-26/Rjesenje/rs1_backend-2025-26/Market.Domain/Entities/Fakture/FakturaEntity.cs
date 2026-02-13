using Market.Domain.Common;

namespace Market.Domain.Entities.Fakture;

public class FakturaEntity : BaseEntity
{
    public required string BrojRacuna { get; set; }
    public required FakturaTip Tip { get; set; }
    public string? Napomena { get; set; }
    public int BrojStavki => Stavke.Count;
    public ICollection<FakturaStavkaEntity> Stavke { get; set; } = [];

    public static class Constraints
    {
        public const int BrojRacunaMaxLength = 20;
        public const int NapomenaMaxLength = 500;
    }
}
