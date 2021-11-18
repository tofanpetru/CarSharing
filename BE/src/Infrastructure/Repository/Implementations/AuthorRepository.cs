using Infrastructure.Persistance;
using Infrastructure.Repository.Abstract;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace Infrastructure.Repository.Implementations
{
    public class AuthorRepository : AbstractRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(BookSharingContext context) : base(context) { }

        public int FindByName(string name)
        {
            return DataBaseContext.Authors.Where(i => i.FullName == name).Select(i => i.Id).FirstOrDefault();
        }

        public new IEnumerable<Author> GetAll()
        {
            return DataBaseContext.Authors;
        }

        public Author GetByName(string name)
        {
            return DataBaseContext.Authors.Where(a => a.FullName == name).Include(x => x.Books).FirstOrDefault();
        }

        public IEnumerable<Author> GetAllNonPendingAuthors()
        {
            return DataBaseContext.Authors.Where(i => i.IsPending == false);
        }

        public int GetAuthorBooksNumber(int id)
        {
            return DataBaseContext.Books.Where(i => i.Author.Id == id).Count();
        }

        public Author GetFromNotPending(string fullName)
        {
            return DataBaseContext.Authors.Where(a => a.FullName == fullName && a.IsPending == false).FirstOrDefault();
        }

    }
}
