using Application.Genres.Commands;
using Application.Movies.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    public class DataSeedController : BaseController
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> SeedGenres()
        {
            return Ok(await Mediator.Send(new SeedMoviesCommand() ));
        }
    }
}