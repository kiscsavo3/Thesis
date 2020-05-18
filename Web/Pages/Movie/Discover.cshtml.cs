using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Domain.Internal.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web
{
    public class DiscoverModel : PageModel
    {
        private readonly IMovieAppService movieAppService;
        public DiscoverModel(IMovieAppService movieAppService)
        {
            this.movieAppService = movieAppService;
        }
        public List<MovieViewModel> Movies { get; set; } = new List<MovieViewModel>();
        public void OnGet()
        {
            Movies = movieAppService.GetDiscoveredMovieViewModelList().Result.ToList();
        }
    }
}