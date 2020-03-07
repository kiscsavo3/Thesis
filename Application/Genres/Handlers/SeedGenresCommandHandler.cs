using Application.Common.Interfaces;
using Application.Genres.Commands;
using Application.Services.ExternalApi;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Genres.Handlers
{
    public class SeedGenresCommandHandler : IRequestHandler<SeedGenresCommand, int>
    {
        private readonly IGenreRepository genreRepository;
        private readonly IPullGenresAppService pullGenresAppService;
        private readonly IMapper mapper;
        public SeedGenresCommandHandler(IGenreRepository genreRepository, IPullGenresAppService pullGenresAppService, IMapper mapper)
        {
            this.genreRepository = genreRepository;
            this.pullGenresAppService = pullGenresAppService;
            this.mapper = mapper;
        }
        public async Task<int> Handle(SeedGenresCommand request, CancellationToken cancellationToken)
        {
            var genreDTOs = await pullGenresAppService.GetGenres(request.SpecUri);
            foreach (var genreDTO in genreDTOs)
            {
                var genre = mapper.Map<GenreEntity>(genreDTO);
                await genreRepository.UpsertAsync(genre);
            }
            return genreDTOs.Count();

        }
    }
}
