namespace Domain.External.DTO
{

    public class ApiMovieKeywordsDTO
    {
        public int id { get; set; }
        public Keyword[] keywords { get; set; }
    }

    public class Keyword
    {
        public int id { get; set; }
        public string name { get; set; }
    }

}
