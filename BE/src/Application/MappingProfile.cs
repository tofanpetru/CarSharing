using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AllCarsDTO, Car>(MemberList.Source).ForPath(dest => dest.CarBrand.Name, src => src.MapFrom(i => i.CarBrand))
                .ReverseMap();
        }
    }
}
