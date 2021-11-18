using Infrastructure.Persistance;

namespace Infrastructure.Repository.Interfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
        public Genre GetByName(string genre);
    }
}
