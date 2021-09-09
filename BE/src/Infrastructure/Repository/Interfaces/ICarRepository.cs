using Infrastructure.Persistence;

namespace Infrastructure.Repository.Interfaces
{
    public interface ICarRepository : IRepository<Car>
    {
        public string FindCarPostByTitle(string title);
    }
}
