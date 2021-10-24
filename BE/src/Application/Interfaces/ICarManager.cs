using Domain.Entities;
using Domain.Entities.Pagination;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ICarManager
    {
        ICollection<AllCarsDTO> GetAllCars();
        CarDetailsDTO Get(int id);
        IEnumerable<HomePageCarsDTO> GetLastThreeAvalableCars();
        IEnumerable<CarsSpecificationsDTO> GetCarsSpecifications();
        PagedList<AllCarsDTO> GetPagedCars(CarParameters carParameters);
    }
}
