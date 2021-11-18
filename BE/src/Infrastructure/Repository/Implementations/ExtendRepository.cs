using Infrastructure.Persistance;
using Infrastructure.Repository.Abstract;
using Infrastructure.Repository.Interfaces;

namespace Infrastructure.Repository.Implementations 
{ 
    public class ExtendRepository : AbstractRepository<Extend>, IExtendRepository
    {
        public ExtendRepository(BookSharingContext context) : base(context) { }
    }
}
