using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IMovieRepository
    {
        Task UpsertAsync(MovieEntity genre);

        Task<MovieEntity> GetAsync(long id);
    }
}
