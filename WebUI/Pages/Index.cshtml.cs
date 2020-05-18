using Application.Services;
using Domain.Internal.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace WebUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMovieAppService movieAppService;
        private readonly IUserAppService userAppService;

        public IndexModel(IHttpContextAccessor httpContextAccessor, IMovieAppService movieAppService, IUserAppService userAppService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.movieAppService = movieAppService;
            this.userAppService = userAppService;
        }
        public List<MovieViewModel> Movies { get; set; } = new List<MovieViewModel>();
        public void OnGet()
        {
            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var user = userAppService.InsertAndGetUser().Result;
                Movies = movieAppService.GetRecMovieViewModelList().Result;
            }
        }
    }
}
