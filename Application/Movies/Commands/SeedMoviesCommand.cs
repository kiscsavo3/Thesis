using Application.Services.ExternalApi;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Movies.Commands
{
    public class SeedMoviesCommand : IRequest<int>
    {
        public class SeedMoviesCommandHandler : IRequestHandler<SeedMoviesCommand, int>
        {
            private readonly IPullMoviesAppService pullMoviesAppService;
            public SeedMoviesCommandHandler(IPullMoviesAppService pullMoviesAppService)
            {
                this.pullMoviesAppService = pullMoviesAppService;
            }
            public async Task<int> Handle(SeedMoviesCommand request, CancellationToken cancellationToken)
            {
                return await pullMoviesAppService.GetMoviesAsync();

            }
        }
    }
}
