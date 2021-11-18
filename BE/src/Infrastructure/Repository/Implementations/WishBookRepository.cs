using Infrastructure.Persistance;
using Infrastructure.Repository.Abstract;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repository.Implementations
{
    public class WishBookRepository : AbstractRepository<WishBook>, IWishBookRepository
    {
        public WishBookRepository(BookSharingContext context) : base(context) { }

        public IEnumerable<WishBook> GetAllWishedBooks()
        {
            return DataBaseContext.WishBooks.Include(i => i.Users);
        }

        public IEnumerable<WishBook> GetWishedBooks(int firstitem, int pageSize, User user)
        {
            return DataBaseContext.WishBooks.Where(x => (user == null || x.Users.Contains(user))).Skip(firstitem).Take(pageSize).Include(i => i.Users);
        }

        public WishBook GetWishBookByTitleAndAuthor(string title, string author)
        {
            return DataBaseContext.WishBooks.Include(x => x.Users).Where(x => x.BookTitle == title && x.BookAuthor == author).FirstOrDefault();
        }

        public WishBook GetById(int id)
        {
            return DataBaseContext.WishBooks.Include(u => u.Users).FirstOrDefault(x => x.Id == id);
        }
    }
}
