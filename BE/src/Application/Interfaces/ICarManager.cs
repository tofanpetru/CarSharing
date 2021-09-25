using Domain.Entities;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ICarManager
    {
        ICollection<AllCarsDTO> GetAll();
        IEnumerable<HomePageCarsDTO> GetLastThreeAvalableCars();
    }
}
