using Market.Domain.Common;

namespace Market.Domain.Entities.Catalog;

public enum DostavljacType { Ekstern = 1, Intern, Freelancer }

public sealed class DostavljacEntity : BaseEntity
{
    public string Name { get; set; } = "";
    public DostavljacType Type { get; set; }
    public string Code { get; set; } = "";
    public bool IsActive { get; set; }

    public static class Constraints {
        public const int CodeMaxLength = 3;
    }
}
