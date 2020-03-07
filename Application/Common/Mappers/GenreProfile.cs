using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Extensions;
using Domain.External.DTO;
using Domain.Internal.DTO;
using Neo4j.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Application.Common.Mappers
{
    public class NodeValueResolver : IValueResolver<BaseEntity, Node, Dictionary<string, object>>
    {
        public Dictionary<string, object> Resolve(BaseEntity source, Node destination, Dictionary<string, object> destMember, ResolutionContext context)
        {
            var propertyInfos = source.GetType().GetProperties();
            Dictionary<string, object> properties = new Dictionary<string, object>();
            foreach (var propertyInfo in propertyInfos)
            {
                if(propertyInfo.PropertyType.GenericTypeArguments.Count() != 0)
                {
                    continue;
                }
                else if (propertyInfo.Name == "Id") { continue; }
                else { properties.Add(propertyInfo.Name, propertyInfo.GetValue(source)); }
            }
            return properties;
        }
    }


   
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<ApiGenreDTO, GenreEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name));

            CreateMap<GenreEntity, Node>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.GetType().Name.ShortName()))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom<NodeValueResolver>());

            CreateMap<INode, GenreEntity>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForAllOtherMembers(dest => dest.MapFrom(src => src.Properties));
        }
    }
}
