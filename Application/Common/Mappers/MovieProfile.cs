using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Extensions;
using Domain.External.DTO;
using Domain.Internal.DTO;
using Domain.Relationships;
using Neo4j.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Application.Common.Mappers
{
    public class RelationshipValueResolver : IValueResolver<BaseRelationship, Relationship, Dictionary<string, object>>
    {
        public Dictionary<string, object> Resolve(BaseRelationship source, Relationship destination, Dictionary<string, object> destMember, ResolutionContext context)
        {
            var propertyInfos = source.GetType().GetProperties();
            Dictionary<string, object> properties = new Dictionary<string, object>();
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType.GenericTypeArguments.Count() != 0)
                {
                    continue;
                }
                else if (propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType != typeof(string)) { continue; }
                else if (propertyInfo.Name.Contains("Id")) { continue; }
                else { properties.Add(propertyInfo.Name, propertyInfo.GetValue(source)); }
            }
            return properties;
        }
    }

    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<ApiMovieDetailsDTO, MovieEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.Budget, opt => opt.MapFrom(src => src.budget))
                .ForMember(dest => dest.Homepage, opt => opt.MapFrom(src => src.homepage))
                .ForMember(dest => dest.Imdb_id, opt => opt.MapFrom(src => src.imdb_id))
                .ForMember(dest => dest.Overview, opt => opt.MapFrom(src => src.overview))
                .ForMember(dest => dest.Popularity, opt => opt.MapFrom(src => src.popularity))
                .ForMember(dest => dest.Poster_path, opt => opt.MapFrom(src => src.poster_path))
                .ForMember(dest => dest.Release_date, opt => opt.MapFrom(src => src.release_date))
                .ForMember(dest => dest.Revenue, opt => opt.MapFrom(src => src.revenue))
                .ForMember(dest => dest.Runtime, opt => opt.MapFrom(src => src.runtime))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.status))
                .ForMember(dest => dest.Tagline, opt => opt.MapFrom(src => src.tagline))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.title))
                .ForMember(dest => dest.Vote_average, opt => opt.MapFrom(src => src.vote_average))
                .ForMember(dest => dest.Vote_count, opt => opt.MapFrom(src => src.vote_count))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<MovieEntity, Node>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.GetType().Name.ShortName()))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom<NodeValueResolver>());

            CreateMap<INode, MovieEntity>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForAllOtherMembers(dest => dest.MapFrom(src => src.Properties));


            CreateMap<Genre, GenreEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.id));

            CreateMap<BaseRelationship, Relationship>().IncludeAllDerived()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.GetType().Name.ShortName()))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom<RelationshipValueResolver>());

            CreateMap<Spoken_Languages, LanguageEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.iso_639_1));

            CreateMap<LanguageEntity, Node>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.GetType().Name.ShortName()))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom<NodeValueResolver>());

            CreateMap<Production_Companies, ProductionCompanyEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.id));

            CreateMap<ProductionCompanyEntity, Node>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.GetType().Name.ShortName()))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom<NodeValueResolver>());


            CreateMap<Production_Countries, ProductionCountryEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.iso_3166_1));

            CreateMap<ProductionCountryEntity, Node>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.GetType().Name.ShortName()))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom<NodeValueResolver>());


            CreateMap<Keyword, KeyWordEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.id));

            CreateMap<KeyWordEntity, Node>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.GetType().Name.ShortName()))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom<NodeValueResolver>());

            CreateMap<Cast, CastRelationship>()
                .ForMember(dest => dest.Character, opt => opt.MapFrom(src => src.character))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.gender))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.order))
                .ForMember(dest => dest.EntityCredit, opt => opt.MapFrom(src => src.credit_id))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<CastRelationship, Relationship>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.GetType().Name.ShortName()))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom<RelationshipValueResolver>());

            CreateMap<Crew, CrewRelationship>()
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.department))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.gender))
                .ForMember(dest => dest.Job, opt => opt.MapFrom(src => src.job))
                .ForMember(dest => dest.EntityCredit, opt => opt.MapFrom(src => src.credit_id))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<CrewRelationship, Relationship>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.GetType().Name.ShortName()))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom<RelationshipValueResolver>());

            CreateMap<ApiPeopleDetailsDTO, PersonEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.id));

            CreateMap<PersonEntity, Node>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.GetType().Name.ShortName()))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom<NodeValueResolver>()); 
        }
    }
}
