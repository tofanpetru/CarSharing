using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repository.Interfaces;
using System.Collections.Generic;

namespace Application.Manager
{
    public class CarManager : ICarManager
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;

        public CarManager(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }


        public CarDetailsDTO Get(int id)
        {
            return _mapper.Map<CarDetailsDTO>(_carRepository.GetCarById(id));
        }

        public IEnumerable<AllCarsDTO> GetAllCars()
        {
            return _mapper.Map<ICollection<AllCarsDTO>>(_carRepository.GetAllCars());
        }

        public IEnumerable<HomePageCarsDTO> GetLastThreeAvalableCars()
        {
            return _mapper.Map<ICollection<HomePageCarsDTO>>(_carRepository.GetLastThreeAvalableCars());
        }
        public IEnumerable<CarsSpecificationsDTO> GetCarsSpecifications()
        {
            return _mapper.Map<ICollection<CarsSpecificationsDTO>>(_carRepository.GetAllCarSpecifications());
        }
    }
}
