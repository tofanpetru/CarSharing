using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repository.Interfaces;
using System.Collections.Generic;

namespace Application.Manager
{
    public class CarBrandManager : ICarBrandManager
    {
        private readonly ICarBrandRepository _carBrandRepository;
        private readonly IMapper _mapper;

        public CarBrandManager(ICarBrandRepository carBrandRepository, IMapper mapper)
        {
            _carBrandRepository = carBrandRepository;
            _mapper = mapper;
        }

        public IEnumerable<CarBrandDTO> GetAllCarBrands()
        {
            return _mapper.Map<ICollection<CarBrandDTO>>(_carBrandRepository.GetAll());
        }
    }
}
