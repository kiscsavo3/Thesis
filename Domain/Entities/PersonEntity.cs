using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class PersonEntity : BaseEntity
    {
        public string Birthday { get; set; }
        public string Known_for_department { get; set; }
        public object Deathday { get; set; }
        public string Name { get; set; }
        //public List<string> Also_known_as { get; set; }
        public int? Gender { get; set; }
        public string Biography { get; set; }
        public float? Popularity { get; set; }
        public string Place_of_birth { get; set; }
        public string Profile_path { get; set; }
        public string Imdb_id { get; set; }
        public string Homepage { get; set; }
    }
}
