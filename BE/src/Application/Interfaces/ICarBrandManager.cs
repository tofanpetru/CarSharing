using Domain.Entities;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ICarBrandManager
    {
        IEnumerable<CarBrandDTO> GetAllCarBrands();
    }
}
