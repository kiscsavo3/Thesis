using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Services;
using Domain.Internal.ViewModel;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Movie
{
    public class DetailsModel : PageModel
    {
        private readonly IMovieAppService movieAppService;
        private readonly IUserAppService userAppService;
        [BindProperty]
        public MovieDetailsViewModel Movie { get; set; }

        public ReviewViewModel ReviewViewModel { get; set; }
        public string NoData { get; set; }
        public int Id { get; set; }

        public DetailsModel(IMovieAppService movieAppService, IUserAppService userAppService)
        {
            this.movieAppService = movieAppService;
            this.userAppService = userAppService;
            //Movie = new MovieDetailsViewModel();
            NoData = String.Empty;
            
        }
        public void OnGet(int id)
        {
            var movieDetailsViewModel = movieAppService.GetMovieDetailsAsync(id).Result;
            if (movieDetailsViewModel != null)
            {
                Movie = movieDetailsViewModel;
                Id = id;
            }
            else
            {
                NoData = "We have not found any related data!";
            }
        }

        public IActionResult OnPostReview([FromBody]ReviewViewModel reviewViewModel)
        {
            reviewViewModel = userAppService.CreateReviewAsync(reviewViewModel).Result;
            return new JsonResult(reviewViewModel);
        }


        public IActionResult OnPostRating([FromBody]RatingViewModel ratingViewModel)
        {
            var result = userAppService.CreateRateAsync(ratingViewModel).Result;
            return new JsonResult(result);
        }
    }
}