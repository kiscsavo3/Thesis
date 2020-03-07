using Domain.External.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.ExternalApi
{
    public interface IPullGenresAppService
    {
        Task<List<ApiGenreDTO>> GetGenres(string uriSpeci);
    }
    
}
