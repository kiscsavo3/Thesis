using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IGenreRepository
    {
        Task UpsertAsync(GenreEntity genre);

        Task<GenreEntity> GetAsync(long id);
    }
}
