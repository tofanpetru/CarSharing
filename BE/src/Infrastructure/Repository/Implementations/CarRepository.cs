using Infrastructure.Persistence;
using Infrastructure.Repository.Abstract;
using Infrastructure.Repository.Interfaces;
using System.Collections.Generic;
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

        public IEnumerable<Car> GetLastThreeAvalableCars()
        {
            return CarSharingContext.Cars.Where(c => c.IsAvalable).Take(3).OrderBy(c => c.PublishDate);
        }
    }
}
