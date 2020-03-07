using Domain.Common;
using Domain.Entities;

namespace Domain.Relationships
{
    public class ProductionCompanyRelationship : BaseRelationship
    {
        public ProductionCompanyRelationship(BaseEntity Src, BaseEntity Trg) : base(Src, Trg)
        {

        }
        private MovieEntity movieEntity;
        private ProductionCompanyEntity productionCompanyEntity;
        public override BaseEntity StartNode { get { return movieEntity; } set { movieEntity = (MovieEntity)value; } }
        public override BaseEntity EndNode { get { return productionCompanyEntity; } set { productionCompanyEntity = (ProductionCompanyEntity)value; } }
    }
}
