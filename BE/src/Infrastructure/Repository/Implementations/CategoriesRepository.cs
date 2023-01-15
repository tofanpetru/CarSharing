using Infrastructure.Persistence;
using Infrastructure.Repository.Abstract;
using Infrastructure.Repository.Interfaces;

namespace Infrastructure.Repository.Implementations
{
    public class CategoriesRepository : AbstractRepository<Category>, ICategoriesRepository
    {
        public CategoriesRepository(CarSharingContext context) : base(context) { }
    }
}
