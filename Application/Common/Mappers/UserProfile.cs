using AutoMapper;
using Domain.Entities;
using Neo4j.Driver;

namespace Application.Common.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<INode, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EntityId, opt =>opt.MapFrom(src => src.Properties["entityid"]));
        }
    }
}
