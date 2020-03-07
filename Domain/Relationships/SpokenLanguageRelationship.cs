using Domain.Common;
using Domain.Entities;

namespace Domain.Relationships
{
    public class SpokenLanguageRelationship : BaseRelationship
    {
        public SpokenLanguageRelationship(BaseEntity Src, BaseEntity Trg) : base(Src, Trg)
        {

        }
        private MovieEntity movieEntity;
        private LanguageEntity languageEntity;
        public override BaseEntity StartNode { get { return movieEntity; } set { movieEntity = (MovieEntity)value; } }
        public override BaseEntity EndNode { get { return languageEntity; } set { languageEntity = (LanguageEntity)value; } }
    }
}
