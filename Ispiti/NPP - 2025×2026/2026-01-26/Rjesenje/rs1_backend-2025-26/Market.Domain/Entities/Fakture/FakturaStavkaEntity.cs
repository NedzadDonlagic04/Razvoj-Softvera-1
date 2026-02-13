using Market.Domain.Common;
using Market.Domain.Entities.Catalog;

namespace Market.Domain.Entities.Fakture;

public sealed class FakturaStavkaEntity : BaseEntity
{
    public int CategoryId { get; set; }
    public ProductCategoryEntity Category { get; set; } = null!;
    public string ProductName { get; set; } = "";
    public int StockQuantity { get; set; }
    public int FakturaId { get; set; }
    public FakturaEntity Faktura { get; set; } = null!;
}
