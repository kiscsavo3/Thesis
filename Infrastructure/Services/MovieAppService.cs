using Application.Common.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.External.DTO;
using Domain.Internal.ViewModel;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class MovieAppService : IMovieAppService
    {
        private readonly IMapper mapper;
        private readonly IMovieRepository movieRepository;
        private readonly IUserRepository userRepository;
        private readonly IOptions<ApiCredentials> apiCredentials;
        private readonly IHttpContextAccessor httpContextAccessor;
        public MovieAppService(IMapper mapper, IMovieRepository movieRepository, IOptions<ApiCredentials> apiCredentials, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.mapper = mapper;
            this.movieRepository = movieRepository;
            this.userRepository = userRepository;
            this.apiCredentials = apiCredentials;
            this.httpContextAccessor = httpContextAccessor;
        }
        public Task<PersonDetailsViewModel> GetPersonViewModel(string personId)
        {
            return Task.Run(() => 
            {
                var externalApiHelper = new ExternalApiHelper();
                var uri = $"{apiCredentials.Value.BaseUri}/person/{personId}{apiCredentials.Value.PartUri}{apiCredentials.Value.ApiKey}";
                var apiPersonDetailsDTO = externalApiHelper.GetData<ApiPersonDetailsDTO>(uri);
                if (apiPersonDetailsDTO == null) return null;
                if (apiPersonDetailsDTO.profile_path != null)
                {
                    apiPersonDetailsDTO.profile_path = "https://image.tmdb.org/t/p/w500" + apiPersonDetailsDTO.profile_path;
                }
                else
                {
                    if (apiPersonDetailsDTO.gender == 1) apiPersonDetailsDTO.profile_path = "/img/actorWoman.jpg";
                    else apiPersonDetailsDTO.profile_path = "/img/actorMan.jpg";
                }
                var personDetailsViewModel = mapper.Map<PersonDetailsViewModel>(apiPersonDetailsDTO);

                return personDetailsViewModel;
            });
        }
        private Task<MovieDetailsViewModel> GetApiMovieDetails(int tmdbId)
        {
            return Task.Run(() =>
            {
                var externalApiHelper = new ExternalApiHelper();
                var uri = $"{apiCredentials.Value.BaseUri}/movie/{tmdbId}{apiCredentials.Value.PartUri}{apiCredentials.Value.ApiKey}";
                var apiMovieDetailsDTO = externalApiHelper.GetData<ApiMovieDetailsDTO>(uri);
                if (apiMovieDetailsDTO == null) return null;
                if (apiMovieDetailsDTO.poster_path != null)
                {
                    apiMovieDetailsDTO.poster_path = "https://image.tmdb.org/t/p/w500" + apiMovieDetailsDTO.poster_path;
                }
                else
                {
                    apiMovieDetailsDTO.poster_path = "/img/posterTemp.jpg";
                }
                var movieDetailsViewModel = mapper.Map<MovieDetailsViewModel>(apiMovieDetailsDTO);
                return movieDetailsViewModel;
            });
        }
        public async Task<MovieDetailsViewModel> GetMovieDetailsAsync(int tmdbId)
        {
            var recommendedMovies = await movieRepository.GetRecommendationsByTag(tmdbId);
            var movieViewModels = Helper(recommendedMovies);
            var movieDetails = await GetApiMovieDetails(tmdbId);
            var movie = await movieRepository.GetMovieById(tmdbId);
            var reviews = await movieRepository.GetReviews(tmdbId);
            float? rating = null;
            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var nameidentifier = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                rating = await userRepository.GetRating(nameidentifier, tmdbId);
            }
            mapper.Map(movie, movieDetails);
            movieDetails.Rating = rating;
            movieDetails.Reviews = reviews.Select(r => mapper.Map<ReviewViewModel>(r)).ToArray();
            movieDetails.Recommendations = await movieViewModels;
            return movieDetails;

        } 

        public Task<MovieCreditsViewModel> GetMovieCredits(int tmdbId)
        {
            return Task.Run(() => 
            {
                var externalApiHelper = new ExternalApiHelper();
                var uri = $"{apiCredentials.Value.BaseUri}/movie/{tmdbId}/credits{apiCredentials.Value.PartUri}{apiCredentials.Value.ApiKey}";
                var apiMovieCreditsDTO = externalApiHelper.GetData<ApiMovieCreditsDTO>(uri);
                if (apiMovieCreditsDTO == null) return null;
                foreach (var cast in apiMovieCreditsDTO.cast)
                {
                    if (cast.profile_path != null)
                    {
                        cast.profile_path = "https://image.tmdb.org/t/p/w500" + cast.profile_path;
                    }
                    else
                    {
                        if (cast.gender == 1) cast.profile_path = "/img/actorWoman.jpg";
                        else cast.profile_path = "/img/actorMan.jpg";
                    }
                }
                foreach (var crew in apiMovieCreditsDTO.crew)
                {
                    if (crew.profile_path != null)
                    {
                        crew.profile_path = "https://image.tmdb.org/t/p/w500" + crew.profile_path;
                    }
                    else
                    {
                        if(crew.gender == 1) crew.profile_path = "/img/actorWoman.jpg";
                        else crew.profile_path = "/img/actorMan.jpg";
                    }
                }
                var movieCreditsViewModel = mapper.Map<MovieCreditsViewModel>(apiMovieCreditsDTO);
                return movieCreditsViewModel;
            });
        }

        private ApiImagesDTO GetApiImages(string tmdbId)
        {
            var externalApiHelper = new ExternalApiHelper();
            var uri = $"{apiCredentials.Value.BaseUri}/movie/{tmdbId}/images{apiCredentials.Value.PartUri}{apiCredentials.Value.ApiKey}";
            var apiMovieImagesDTO = externalApiHelper.GetData<ApiImagesDTO>(uri);
            return apiMovieImagesDTO;
        }
        
        private async Task<List<MovieViewModel>> Helper(List<Movie> movieList)
        {
            List<Task<MovieViewModel>> movies = new List<Task<MovieViewModel>>();
            foreach (var movie in movieList)
            {
                var result = Task.Run(() => 
                {
                    var apiImageDTO = GetApiImages(movie.TmdbId);
                    if (apiImageDTO != null)
                    {
                        if (apiImageDTO.posters != null && apiImageDTO.posters.Length > 0)
                        {
                            movie.ImageUri = "https://image.tmdb.org/t/p/w500" + apiImageDTO.posters[0].file_path;
                        }
                        else
                        {
                            movie.ImageUri = "/img/posterTemp.jpg";
                        }
                    }
                    var movieViewModel = mapper.Map<MovieViewModel>(movie);
                    return movieViewModel;
                });
                movies.Add(result);
            }
            var movieViewModels = await Task.WhenAll(movies);
            return movieViewModels.Where(x => x.ImageUri != null).ToList();
        }

        public async Task<List<MovieViewModel>> GetMovieViewModelList(string title)
        {
            var movieList = await GetMovieTmdbIdsAsync(title);
            var result = await Helper(movieList);
            return result;
        }
        public async Task<List<Movie>> GetMovieTmdbIdsAsync(string title)
        {
            while(title.EndsWith(' '))
            {
                title = title.Remove(title.Length - 1);
            }
            var nodes = await movieRepository.GetMoviesByTitle(title);
            return nodes;
        }

        public async Task<List<string>> GetTitlesAsync(string term)
        {
            while (term.EndsWith(' '))
            {
                term = term.Remove(term.Length - 1);
            }
            var titles = await movieRepository.GetTitlesByTerm(term);
            return titles;
        }

        public async Task<List<MovieViewModel>> GetDiscoveredMovieViewModelList()
        {
            var nameidentifier = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var movies = await movieRepository.GetDiscoveredMovies(nameidentifier);
            if (movies.Count == 0) movies = await movieRepository.GetDefaultMovies(nameidentifier);
            var result = await Helper(movies);
            return result;
        }

        public async Task<List<MovieViewModel>> GetRecMovieViewModelList()
        {
            var nameidentifier = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var movies = await movieRepository.GetRecMovies(nameidentifier);
            if (movies.Count == 0) movies = await movieRepository.GetDefaultMovies(nameidentifier);
            var result = await Helper(movies);
            return result;
        }
    }
}
