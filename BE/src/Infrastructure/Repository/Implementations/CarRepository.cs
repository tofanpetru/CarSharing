using Infrastructure.Persistence;
using Infrastructure.Repository.Abstract;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
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
            return CarSharingContext.Cars.Where(c => c.IsAvalable)
                                         .Take(3)
                                         .OrderBy(c => c.PublishDate);
        }

        public Car GetCarById(int id)
        {
            return CarSharingContext.Cars.Where(c => c.Id == id)
                                         .Include(c => c.CarBrand)
                                         .Include(ca => ca.Categories)
                                         .FirstOrDefault();
        }

        public IEnumerable<Car> GetAllCars()
        {
            return CarSharingContext.Cars.Include(c => c.Categories)
                                         .Include(cb => cb.CarBrand);
        }

        public IEnumerable<Car> GetAllCarSpecifications()
        {
            return CarSharingContext.Cars.Distinct();
        }
    }
}
