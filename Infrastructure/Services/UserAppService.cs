using Application.Common.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Internal.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserAppService(IUserRepository userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }
        public Task<User> InsertAndGetUser()
        {
            var nameIdentifier = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var nodeUser = userRepository.InsertAndGetUser(nameIdentifier);
            return nodeUser;
        }

        public async Task<ReviewViewModel> CreateReviewAsync(ReviewViewModel reviewViewModel)
        {
            reviewViewModel.Date = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";
            var name = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "name").Value;
            var entityid = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            reviewViewModel.UserName = name;
            reviewViewModel.EntityId = entityid;
            var review = mapper.Map<Review>(reviewViewModel);
            var nameidentifier = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            await userRepository.WriteReview(review, nameidentifier);
            return reviewViewModel;
        }

        public async Task<bool> CreateRateAsync(RatingViewModel ratingViewModel)
        {
            var rating = mapper.Map<Rating>(ratingViewModel);
            var nameidentifier = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return await userRepository.CreateRating(rating, nameidentifier);
        }
    }
}
