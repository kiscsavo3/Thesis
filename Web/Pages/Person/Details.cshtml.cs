using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Domain.Internal.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Person
{
    public class DetailsModel : PageModel
    {
        private readonly IMovieAppService movieAppService;
        public PersonDetailsViewModel Person { get; set; }
        public string NoData { get; set; }
        public DetailsModel(IMovieAppService movieAppService)
        {
            this.movieAppService = movieAppService;
            NoData = String.Empty;
        }
        public void OnGet(int id)
        {
            var personDetailsViewModel = movieAppService.GetPersonViewModel(id.ToString()).Result;
            if (personDetailsViewModel == null) NoData = "We have not found any related data!";
            else Person = personDetailsViewModel;
        }
    }
}