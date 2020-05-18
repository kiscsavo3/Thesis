using Domain.Entities;
using Domain.Internal.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IUserAppService
    {
        Task<User> InsertAndGetUser();

        Task<ReviewViewModel> CreateReviewAsync(ReviewViewModel reviewViewModel);

        Task<bool> CreateRateAsync(RatingViewModel ratingViewModel);
    }
}
