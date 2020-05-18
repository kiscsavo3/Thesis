using System.Collections.Generic;

namespace Domain.Internal.ViewModel
{
    public class MovieDetailsViewModel
    {
        public string TmdbId { get; set; }
        public string Title { get; set; }
        public int? Runtime { get; set; }
        public long? Revenue { get; set; }
        public string ReleaseDate { get; set; }
        public int RatingCount { get; set; }
        public float RatingAverage { get; set; }
        public object PosterPath { get; set; }
        public string Overview { get; set; }
        public int? Budget { get; set; }
        public float? Rating { get; set; }
        public GenreViewModel[] Genres { get; set; }
        public ProductionCompanyViewModel[] ProductionCompanies { get; set; }
        public ProductionCountryViewModel[] ProductionCountries { get; set; }
        public ReviewViewModel[] Reviews { get; set; }
        public List<MovieViewModel> Recommendations { get; set; }
    }
}
