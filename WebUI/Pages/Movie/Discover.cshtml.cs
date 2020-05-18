using Application.Services;
using Domain.Internal.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace WebUI
{
    public class DiscoverModel : PageModel
    {
        private readonly IMovieAppService movieAppService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public DiscoverModel(IMovieAppService movieAppService, IHttpContextAccessor httpContextAccessor)
        {
            this.movieAppService = movieAppService;
            this.httpContextAccessor = httpContextAccessor;
        }
        public List<MovieViewModel> Movies { get; set; } = new List<MovieViewModel>();
        public void OnGet()
        {
            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                Movies = movieAppService.GetDiscoveredMovieViewModelList().Result;
            }
        }
    }
}