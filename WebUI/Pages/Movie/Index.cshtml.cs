using Application.Services;
using Domain.Internal.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace WebUI.Movie
{
    public class IndexModel : PageModel
    {
        private readonly IMovieAppService movieAppService;
        public List<MovieViewModel> Movies { get; set; } = new List<MovieViewModel>();
        public IndexModel(IMovieAppService movieAppService)
        {
            this.movieAppService = movieAppService;
        }
        public void OnGet(string title = null)
        {
            if (!string.IsNullOrEmpty(title))
            {
                Movies = movieAppService.GetMovieViewModelList(title).Result;
            }
        }
        public IActionResult OnGetSearch(string term)
        {
            var movieTitles = movieAppService.GetTitlesAsync(term).Result;
            return new JsonResult(movieTitles);
        }
        /*
        public IActionResult OnGetSearchMovies(string title)
        {
            var movieViewModelList = movieAppService.GetMovieViewModel(title).Result;
            return new JsonResult(movieViewModelList);
        }
        */
    }
}