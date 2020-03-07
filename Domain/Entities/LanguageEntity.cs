using Domain.Common;

namespace Domain.Entities
{
    public class LanguageEntity : BaseEntity
    {
        public string Iso_639_1 { get; set; }
        public string Name { get; set; }
    }
}
