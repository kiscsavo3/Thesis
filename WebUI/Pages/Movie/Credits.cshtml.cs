using Application.Services;
using Domain.Internal.ViewModel;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Movie
{
    public class CreditsModel : PageModel
    {
        private readonly IMovieAppService movieAppService;
        public MovieCreditsViewModel Credits { get; set; }
        public CreditsModel(IMovieAppService movieAppService)
        {
            this.movieAppService = movieAppService;
        }

        public void OnGet(int id)
        {
            var credits = movieAppService.GetMovieCredits(id).Result;
            Credits = credits;
        }
    }
}