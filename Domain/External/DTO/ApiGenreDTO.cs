namespace Domain.External.DTO
{

    public class ApiGenreDTOList
    {
        public ApiGenreDTO[] genres { get; set; }
    }

    public class ApiGenreDTO
    {
        public int id { get; set; }
        public string name { get; set; }
    }

}
