using Infrastructure.Persistance;
using System.Collections.Generic;

namespace Infrastructure.Repository.Interfaces
{
    public interface IWishBookRepository : IRepository<WishBook>
    {
        public IEnumerable<WishBook> GetAllWishedBooks();
        WishBook GetById(int id);
        WishBook GetWishBookByTitleAndAuthor(string title, string author);
        IEnumerable<WishBook> GetWishedBooks(int firstitem, int pageSize, User user);
    }
}
