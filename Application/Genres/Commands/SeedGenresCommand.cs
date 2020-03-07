using MediatR;

namespace Application.Genres.Commands
{
    public class SeedGenresCommand : IRequest<int>
    {
        public string SpecUri { get; set; }

    }
}
