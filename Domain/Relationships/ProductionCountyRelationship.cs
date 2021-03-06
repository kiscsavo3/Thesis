﻿using Domain.Common;
using Domain.Entities;

namespace Domain.Relationships
{
    public class ProductionCountyRelationship : BaseRelationship
    {
        public ProductionCountyRelationship(BaseEntity Src, BaseEntity Trg) : base(Src, Trg)
        {

        }
        private MovieEntity movieEntity;
        private ProductionCountryEntity productionCountryEntity;
        public override BaseEntity StartNode { get { return movieEntity; } set { movieEntity = (MovieEntity)value; } }
        public override BaseEntity EndNode { get { return productionCountryEntity; } set { productionCountryEntity = (ProductionCountryEntity)value; } }
    }
}
