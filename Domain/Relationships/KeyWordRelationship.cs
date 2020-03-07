using Domain.Common;
using Domain.Entities;

namespace Domain.Relationships
{
    public class KeyWordRelationship : BaseRelationship
    {
        public KeyWordRelationship(BaseEntity Src, BaseEntity Trg) : base(Src, Trg)
        {

        }
        private MovieEntity movieEntity;
        private KeyWordEntity keyWordEntity;
        public override BaseEntity StartNode { get { return movieEntity; } set { movieEntity = (MovieEntity)value; } }

        public override BaseEntity EndNode { get { return keyWordEntity; } set { keyWordEntity = (KeyWordEntity)value; } }
    }
}
