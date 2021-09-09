﻿using Application.Interfaces;
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

        public ICollection<AllCarsDTO> GetAll()
        {
            return _mapper.Map<ICollection<AllCarsDTO>>(_carRepository.GetAll());
        }
    }
}
