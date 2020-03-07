using Domain.Common;
using Domain.Entities;

namespace Domain.Relationships
{
    public class CrewRelationship : BaseRelationship
    {
        public CrewRelationship(BaseEntity Src, BaseEntity Trg) : base(Src, Trg)
        {

        }
        private MovieEntity movieEntity;
        private PersonEntity personEntity;
        public override BaseEntity StartNode { get { return movieEntity; } set { movieEntity = (MovieEntity)value; } }
        public override BaseEntity EndNode { get { return personEntity; } set { personEntity = (PersonEntity)value; } }

        public string Department { get; set; }
        public int Gender { get; set; }
        public string Job { get; set; }

        public string EntityCredit { get; set; }
    }
}
