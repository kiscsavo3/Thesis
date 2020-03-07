namespace Domain.Common
{
    public abstract class BaseRelationship
    {
        public BaseRelationship(BaseEntity Start, BaseEntity End)
        {
            StartNode = Start;
            EndNode = End;
            StartNodeId = Start.Id;
            EndNodeId = End.Id;
        }
        public long Id { get; set; }
        public long StartNodeId { get; set; }
        public long EndNodeId { get; set; }

        public abstract BaseEntity StartNode { get; set; }
        public abstract BaseEntity EndNode { get; set; }
    }
}
