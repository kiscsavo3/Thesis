using Domain.Common;
using Domain.Entities;

namespace Domain.Relationships
{
    public class CastRelationship : BaseRelationship
    {
        public CastRelationship(BaseEntity Src, BaseEntity Trg) : base(Src, Trg)
        {

        }
        private MovieEntity movieEntity;
        private PersonEntity personEntity;
        public override BaseEntity StartNode { get { return movieEntity; } set { movieEntity = (MovieEntity)value; } }

        public override BaseEntity EndNode { get { return personEntity; } set { personEntity = (PersonEntity)value; } }

        public string Character { get; set; }

        public int? Gender { get; set; }

        public int? Order { get; set; }

        public string EntityCredit { get; set; }
    }
}
