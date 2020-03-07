using System.Threading.Tasks;

namespace Application.Services.ExternalApi
{
    public interface IPullMoviesAppService
    {
        Task<int> GetMoviesAsync();
    }
}
