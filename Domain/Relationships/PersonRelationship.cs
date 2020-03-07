using Domain.Common;
using Domain.Entities;

namespace Domain.Relationships
{
    public class PersonRelationship : BaseRelationship
    {
        public PersonRelationship(BaseEntity Src, BaseEntity Trg) : base(Src, Trg)
        {

        }
        private PersonEntity personEntity;
        private MovieEntity movieEntity;
        public override BaseEntity StartNode { get { return movieEntity; } set { movieEntity = (MovieEntity)value; } }
        public override BaseEntity EndNode { get { return personEntity; } set { personEntity = (PersonEntity)value; } }
    }
}
