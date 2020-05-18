using Domain.Entities;
using Domain.External.DTO;
using Domain.Internal.ViewModel;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IMovieAppService
    {
        Task<PersonDetailsViewModel> GetPersonViewModel(string personId);
        Task<MovieCreditsViewModel> GetMovieCredits(int tmdbId);

        Task<MovieDetailsViewModel> GetMovieDetailsAsync(int tmdbId);

        Task<List<MovieViewModel>> GetMovieViewModelList(string title);

        Task<List<Movie>> GetMovieTmdbIdsAsync(string title);

        Task<List<string>> GetTitlesAsync(string term);

        Task<List<MovieViewModel>> GetDiscoveredMovieViewModelList();

        Task<List<MovieViewModel>> GetRecMovieViewModelList();
    }
}
