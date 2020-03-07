using Application.Common.Interfaces;
using Application.Services.ExternalApi;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.External.DTO;
using Domain.Relationships;
using Infrastructure.Helpers;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Infrastructure.Services.ExternalApi
{
    public class PullMoviesAppService : IPullMoviesAppService
    {
        private readonly IMapper mapper;
        private readonly IMovieRepository movieRepository;
        private readonly IOptions<ApiCredentials> apiCredentials;
        public PullMoviesAppService(IMapper mapper, IMovieRepository movieRepository, IOptions<ApiCredentials> apiCredentials)
        {
            this.mapper = mapper;
            this.movieRepository = movieRepository;
            this.apiCredentials = apiCredentials;
        }
        public async Task<int> GetMoviesAsync()
        {
            for (int i = 7; i <= 500; i++) // eddig 125 film lett seedelve
            {
                string uri = $"{apiCredentials.Value.BaseUri}/discover/movie{apiCredentials.Value.PartUri}{apiCredentials.Value.ApiKey}" +
                    $"&sort_by=popularity.desc&include_adult=true&include_video=false&page={i}&with_original_language=en";
                var apiMovieDTOList = (APIMovieDTOList)ExternalApiHelper.GetData<APIMovieDTOList>(uri);
                foreach (var apiMovieDTO in apiMovieDTOList.results)
                {
                    uri = $"{apiCredentials.Value.BaseUri}/movie/{apiMovieDTO.id}{apiCredentials.Value.PartUri}{apiCredentials.Value.ApiKey}";
                    var result = ExternalApiHelper.GetData<ApiMovieDetailsDTO>(uri);
                    if (result.GetType() == typeof(object)) continue;
                    var apiMoviesDetailsDTO = (ApiMovieDetailsDTO)result;
                    var movieEntity = mapper.Map<MovieEntity>(apiMoviesDetailsDTO);
                    foreach (var apiMovieDetailsDTO in apiMoviesDetailsDTO.genres)
                    {
                        var genreEntity = mapper.Map<GenreEntity>(apiMovieDetailsDTO);
                        var genreRelationship = new GenreRelationship(movieEntity, genreEntity);
                        movieEntity.Genres.Add(genreRelationship);
                    }
                    foreach (var apiMovieDetailsDTO in apiMoviesDetailsDTO.production_companies)
                    {
                        var productionCompanyEntity = mapper.Map<ProductionCompanyEntity>(apiMovieDetailsDTO);
                        var productionCompanyRelationship = new ProductionCompanyRelationship(movieEntity, productionCompanyEntity);
                        movieEntity.ProductionCompanies.Add(productionCompanyRelationship);
                    }
                    foreach (var apiMovieDetailsDTO in apiMoviesDetailsDTO.production_countries)
                    {
                        var productionCountryEntity = mapper.Map<ProductionCountryEntity>(apiMovieDetailsDTO);
                        var productionCountyRelationship = new ProductionCountyRelationship(movieEntity, productionCountryEntity);
                        movieEntity.ProductionCountries.Add(productionCountyRelationship);
                    }
                    foreach (var apiMovieDetailsDTO in apiMoviesDetailsDTO.spoken_languages)
                    {
                        var languageEntity = mapper.Map<LanguageEntity>(apiMovieDetailsDTO);
                        var spokenLanguageRelationship = new SpokenLanguageRelationship(movieEntity, languageEntity);
                        movieEntity.SpokenLanguages.Add(spokenLanguageRelationship);
                    }
                    uri = $"{apiCredentials.Value.BaseUri}/movie/{apiMovieDTO.id}/keywords{apiCredentials.Value.PartUri}{apiCredentials.Value.ApiKey}";
                    result = ExternalApiHelper.GetData<ApiMovieKeywordsDTO>(uri);
                    if (result.GetType() == typeof(object)) continue;
                    var apiMovieKeywordsDTO = (ApiMovieKeywordsDTO)result;
                    foreach (var apiMovieKeywordDTO in apiMovieKeywordsDTO.keywords)
                    {
                        var keyWordEntity = mapper.Map<KeyWordEntity>(apiMovieKeywordDTO);
                        var keywordRelationship = new KeyWordRelationship(movieEntity, keyWordEntity);
                        movieEntity.KeyWords.Add(keywordRelationship);
                    }
                    uri = $"{apiCredentials.Value.BaseUri}/movie/{apiMovieDTO.id}/credits{apiCredentials.Value.PartUri}{apiCredentials.Value.ApiKey}";
                    var apiMovieCreditsDTO = (ApiMovieCreditsDTO)ExternalApiHelper.GetData<ApiMovieCreditsDTO>(uri);
                    foreach (var apiMovieCrew in apiMovieCreditsDTO.crew)
                    {
                        uri = $"{apiCredentials.Value.BaseUri}/person/{apiMovieCrew.id}{apiCredentials.Value.PartUri}{apiCredentials.Value.ApiKey}";
                        result = ExternalApiHelper.GetData<ApiPeopleDetailsDTO>(uri);
                        if (result.GetType() == typeof(object)) continue;
                        var apiPeopleDetailsDTO = (ApiPeopleDetailsDTO)result;
                        var personEntity = mapper.Map<PersonEntity>(apiPeopleDetailsDTO);
                        var personRelationship = new CrewRelationship(movieEntity, personEntity);
                        mapper.Map(apiMovieCrew, personRelationship);
                        movieEntity.Crew.Add(personRelationship);
                    }
                    foreach (var apiMovieCast in apiMovieCreditsDTO.cast)
                    {
                        uri = $"{apiCredentials.Value.BaseUri}/person/{apiMovieCast.id}{apiCredentials.Value.PartUri}{apiCredentials.Value.ApiKey}";
                        result = ExternalApiHelper.GetData<ApiPeopleDetailsDTO>(uri);
                        if (result.GetType() == typeof(object)) continue;
                        var apiPeopleDetailsDTO = (ApiPeopleDetailsDTO)result;
                        var personEntity = mapper.Map<PersonEntity>(apiPeopleDetailsDTO);
                        var personRelationship = new CastRelationship(movieEntity, personEntity);
                        mapper.Map(apiMovieCast, personRelationship);
                        movieEntity.Cast.Add(personRelationship);
                    }
                    await movieRepository.UpsertAsync(movieEntity);
                }
            }

            return 10000;
        }
    }
}
