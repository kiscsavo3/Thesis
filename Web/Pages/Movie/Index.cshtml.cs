using Application.Services;
using Domain.Internal.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Web.Movie
{
    public class IndexModel : PageModel
    {
        private readonly IMovieAppService movieAppService;
        public List<MovieViewModel> Movies { get; set; } = new List<MovieViewModel>();
        public IndexModel(IMovieAppService movieAppService)
        {
            this.movieAppService = movieAppService;
        }
        public void OnGet()
        {

        }
        public IActionResult OnGetSearch(string term)
        {
            var movieTitles = movieAppService.GetTitlesAsync(term).Result;
            return new JsonResult(movieTitles);
        }

        public IActionResult OnGetSearchMovies(string title)
        {
            var movieViewModelList = movieAppService.GetMovieViewModelList(title).Result;
            return new JsonResult(movieViewModelList);
        }
    }
}