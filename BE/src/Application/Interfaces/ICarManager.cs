using Domain.Entities;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ICarManager
    {
        CarDetailsDTO Get(int id);
        ICollection<AllCarsDTO> GetAll();
        IEnumerable<HomePageCarsDTO> GetLastThreeAvalableCars();
    }
}
