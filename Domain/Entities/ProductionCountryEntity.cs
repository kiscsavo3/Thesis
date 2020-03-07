using Domain.Common;

namespace Domain.Entities
{
    public class ProductionCountryEntity : BaseEntity
    {
        public string Iso_3166_1 { get; set; }
        public string Name { get; set; }
    }
}
