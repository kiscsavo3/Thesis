using Application.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web
{
    public class IndexModel : PageModel
    {
        private readonly IUserAppService userAppService;
        public IndexModel(IUserAppService userAppService)
        {
            this.userAppService = userAppService;
        }
        public void OnGet()
        {
            var user = userAppService.InsertAndGetUser().Result;
        }
    }
}