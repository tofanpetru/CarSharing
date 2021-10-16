using Domain.Entities;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ICarManager
    {
        IEnumerable<AllCarsDTO> GetAllCars();
        IEnumerable<HomePageCarsDTO> GetLastThreeAvalableCars();
    }
}
