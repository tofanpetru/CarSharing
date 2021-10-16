using Infrastructure.Persistence;
using System.Collections.Generic;

namespace Infrastructure.Repository.Interfaces
{
    public interface ICarRepository : IRepository<Car>
    {
        string FindCarPostByTitle(string title);
        IEnumerable<Car> GetLastThreeAvalableCars();
        IEnumerable<Car> GetAllCars();
    }
}
