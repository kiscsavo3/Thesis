using Domain.Common;

namespace Domain.Entities
{
    public class ProductionCompanyEntity : BaseEntity
    {
        public string Logo_path { get; set; }
        public string Name { get; set; }
        public string Origin_country { get; set; }
    }
}
