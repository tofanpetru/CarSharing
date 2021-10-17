using Domain.Entities;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ICategoriesManager
    {
        IEnumerable<CarCategoryDTO> GetAllCategories();
    }
}
