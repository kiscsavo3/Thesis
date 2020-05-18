using Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IMovieRepository
    {
        Task<Movie> GetMovieById(int tmdbId);

        Task<List<Movie>> GetMoviesByTitle(string title);

        Task<List<string>> GetTitlesByTerm(string term);

        Task<List<Review>> GetReviews(int tmdbId);

        Task<List<Movie>> GetRecommendationsByTag(int tmdbId);

        Task<List<Movie>> GetDiscoveredMovies(string entityid);

        Task<List<Movie>> GetRecMovies(string entityid);

        Task<List<Movie>> GetDefaultMovies(string entityid);
    }
}
