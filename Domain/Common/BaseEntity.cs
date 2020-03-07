namespace Domain.Common
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }

        public string EntityId { get; set; }
    }
}
