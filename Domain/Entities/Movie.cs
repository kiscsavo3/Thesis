using Domain.Common;

namespace Domain.Entities
{
    public class Movie : BaseEntity
    {
        public string ImdbId { get; set; }
        public string TmdbId { get; set; }
        public string Year { get; set; }
        public string Title { get; set; }
        public string ImageUri { get; set; }
        public int RatingCount { get; set; }
        public float RatingAverage { get; set; }
    }
}
