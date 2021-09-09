using Infrastructure.Persistence;
using Infrastructure.Repository.Abstract;
using Infrastructure.Repository.Interfaces;
using System.Linq;

namespace Infrastructure.Repository.Implementations
{
    public class CarRepository : AbstractRepository<Car>, ICarRepository
    {
        public CarRepository(CarSharingContext context) : base(context) { }

        public string FindCarPostByTitle(string title)
        {
            return CarSharingContext.Cars.Where(c => c.Title == title)
                                                .Select(x => x.Title)
                                                .FirstOrDefault();
        }
    }
}
