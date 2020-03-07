using Domain.Common;
using Domain.Entities;

namespace Domain.Relationships
{
    public class GenreRelationship : BaseRelationship
    {
        public GenreRelationship(BaseEntity Src, BaseEntity Trg) : base(Src, Trg)
        {

        }
        private GenreEntity genreEntity;
        private MovieEntity movieEntity;
        public override BaseEntity StartNode { get { return movieEntity; } set { movieEntity = (MovieEntity)value; } }
        public override BaseEntity EndNode { get { return genreEntity; } set { genreEntity = (GenreEntity)value; } }
    }
}
