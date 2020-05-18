using AutoMapper;
using Domain.Entities;
using Domain.External.DTO;
using Domain.Internal.ViewModel;
using Neo4j.Driver;
using System;

namespace Application.Common.Mappers
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<INode, Movie>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.Properties["entityid"]))
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Properties["title"]))
               .ForMember(dest => dest.ImdbId, opt => opt.MapFrom(src => src.Properties["imdbid"]))
               .ForMember(dest => dest.TmdbId, opt => opt.MapFrom(src => src.Properties["tmdbid"]))
               .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Properties["year"]))
               .ForMember(dest => dest.RatingCount, opt => opt.MapFrom(src => src.Properties["ratecount"]))
               .ForMember(dest => dest.RatingAverage, opt => opt.MapFrom(src => (float)Math.Round((double)src.Properties["rateavg"], 1)));

            CreateMap<IRelationship, Review>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Properties["text"]))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Properties["date"]))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Properties["username"]))
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.Properties["entityid"]));

            CreateMap<Review, ReviewViewModel>();
            CreateMap<ReviewViewModel, Review>();
            CreateMap<RatingViewModel, Rating>();
            CreateMap<Movie, MovieViewModel>();

            CreateMap<ApiMovieDetailsDTO, MovieDetailsViewModel>()
                .ForMember(dest => dest.TmdbId, opt => opt.MapFrom(src => src.id.ToString()))
                .ForMember(dest => dest.PosterPath, opt => opt.MapFrom(src => src.poster_path))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.genres))
                .ForMember(dest => dest.ProductionCompanies, opt => opt.MapFrom(src => src.production_companies))
                .ForMember(dest => dest.ProductionCountries, opt => opt.MapFrom(src => src.production_countries))
                .ForMember(dest => dest.RatingCount, opt => opt.Ignore())
                .ForMember(dest => dest.RatingAverage, opt => opt.Ignore())
                .ForMember(dest => dest.Reviews, opt => opt.Ignore())
                .ForMember(dest => dest.Rating, opt => opt.Ignore())
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.release_date));

            CreateMap<Movie, MovieDetailsViewModel>()
                .ForMember(dest => dest.RatingAverage, opt => opt.MapFrom(src => (float)Math.Round((double)src.RatingAverage, 1)))
                .ForMember(dest => dest.RatingCount, opt => opt.MapFrom(src => src.RatingCount))
                .ForAllOtherMembers(dest => dest.Ignore());

            CreateMap<Genre, GenreViewModel>();
            CreateMap<Production_Companies, ProductionCompanyViewModel>();
            CreateMap<Production_Countries, ProductionCountryViewModel>();

            CreateMap<ApiMovieCreditsDTO, MovieCreditsViewModel>()
                .ForMember(dest => dest.Cast, opt => opt.MapFrom(src => src.cast))
                .ForMember(dest => dest.Crew, opt => opt.MapFrom(src => src.crew));

            CreateMap<Cast, CastViewModel>()
                .ForMember(dest => dest.ProfilePath, opt => opt.MapFrom(src => src.profile_path));

            CreateMap<Crew, CrewViewModel>()
                .ForMember(dest => dest.ProfilePath, opt => opt.MapFrom(src => src.profile_path));

            CreateMap<ApiPersonDetailsDTO, PersonDetailsViewModel>()
                .ForMember(dest => dest.KnownForDepartment, opt => opt.MapFrom(src => src.known_for_department))
                .ForMember(dest => dest.PlaceOfBirth, opt => opt.MapFrom(src => src.place_of_birth))
                .ForMember(dest => dest.ProfilePath, opt => opt.MapFrom(src => src.profile_path));
        }
    }
}
