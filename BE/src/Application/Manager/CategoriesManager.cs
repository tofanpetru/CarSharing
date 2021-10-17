using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repository.Interfaces;
using System.Collections.Generic;

namespace Application.Manager
{
    public class CategoriesManager : ICategoriesManager
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IMapper _mapper;

        public CategoriesManager(ICategoriesRepository categoriesRepository, IMapper mapper)
        {
            _categoriesRepository = categoriesRepository;
            _mapper = mapper;
        }

        public IEnumerable<CarCategoryDTO> GetAllCategories()
        {
            return _mapper.Map<ICollection<CarCategoryDTO>>(_categoriesRepository.GetAll());
        }
    }
}
