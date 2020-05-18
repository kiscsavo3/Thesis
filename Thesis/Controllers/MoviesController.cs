using Application.Movies.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    public class MoviesController : BaseController
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMovies()
        {
            return Ok(await Mediator.Send(new GetMoviesByTermQuery("The Avengers") ));
        }
    }
}