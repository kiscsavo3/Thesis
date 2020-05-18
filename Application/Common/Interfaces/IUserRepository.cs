using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<User> InsertAndGetUser(string nameIndentifier);

        Task<bool> WriteReview(Review review, string nameIdentifier);

        Task<bool> CreateRating(Rating rating, string nameIndetifier);

        Task<float?> GetRating(string nameidentifier, int tmdbId);
    }
}
