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

            CreateMap<HomePageCarsDTO, Car>().ReverseMap();

            CreateMap<CarCategoryDTO, Category>().ReverseMap();

            CreateMap<AllCarsDTO, Car>(MemberList.Source)
                .ForPath(dest => dest.CarBrand.Name, src => src.MapFrom(b => b.CarBrand))
                .ReverseMap();

            CreateMap<CarBrandDTO, CarBrand>(MemberList.Source)
                .ReverseMap();
            CreateMap<CarsSpecificationsDTO, Car>()
                .ReverseMap();
        }
    }
}
