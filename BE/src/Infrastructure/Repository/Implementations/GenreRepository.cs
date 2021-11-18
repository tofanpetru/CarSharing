using Infrastructure.Persistance;
using Infrastructure.Repository.Abstract;
using Infrastructure.Repository.Interfaces;
using System.Linq;

namespace Infrastructure.Repository.Implementations
{
    public class GenreRepository : AbstractRepository<Genre>, IGenreRepository
    {
        public GenreRepository(BookSharingContext context) : base(context) { }

        public Genre GetByName(string genre)
        {
            return DataBaseContext.Genres.Where(x => x.Name == genre).FirstOrDefault();
        }
    }
}
