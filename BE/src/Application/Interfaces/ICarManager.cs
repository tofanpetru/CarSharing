using Domain.Entities;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ICarManager
    {
        IEnumerable<AllCarsDTO> GetAllCars();
        CarDetailsDTO Get(int id);
        IEnumerable<HomePageCarsDTO> GetLastThreeAvalableCars();
        IEnumerable<CarsSpecificationsDTO> GetCarsSpecifications();
    }
}
