using Domain.Common;
using Domain.Relationships;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class MovieEntity : BaseEntity
    {
        public int Budget { get; set; }
        public List<GenreRelationship> Genres { get; set; } = new List<GenreRelationship>();
        public string Homepage { get; set; }
        public string Imdb_id { get; set; }
        public string Overview { get; set; }
        public float Popularity { get; set; }
        public string Poster_path { get; set; }
        public List<ProductionCompanyRelationship> ProductionCompanies { get; set; } = new List<ProductionCompanyRelationship>();
        public List<ProductionCountyRelationship> ProductionCountries { get; set; } = new List<ProductionCountyRelationship>();
        public string Release_date { get; set; }
        public long Revenue { get; set; }
        public int Runtime { get; set; }
        public List<SpokenLanguageRelationship> SpokenLanguages { get; set; } = new List<SpokenLanguageRelationship>();
        public string Status { get; set; }
        public string Tagline { get; set; }
        public string Title { get; set; }
        public float Vote_average { get; set; }
        public int Vote_count { get; set; }
        public List<CrewRelationship> Crew { get; set; } = new List<CrewRelationship>();
        public List<CastRelationship> Cast { get; set; } = new List<CastRelationship>();
        public List<KeyWordRelationship> KeyWords { get; set; } = new List<KeyWordRelationship>();
    }
}
