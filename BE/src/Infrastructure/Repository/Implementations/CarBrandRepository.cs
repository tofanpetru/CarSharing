using Infrastructure.Persistence;
using Infrastructure.Repository.Abstract;
using Infrastructure.Repository.Interfaces;

namespace Infrastructure.Repository.Implementations
{
    public class CarBrandRepository : AbstractRepository<CarBrand>, ICarBrandRepository
    {
        public CarBrandRepository(CarSharingContext context) : base(context) { }
    }
}
