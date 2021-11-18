using Infrastructure.Persistance;
using System.Collections.Generic;

namespace Infrastructure.Repository.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        public int FindByName(string name);
        public Author GetByName(string name);
        public new IEnumerable<Author> GetAll();
        public int GetAuthorBooksNumber(int id);
        public IEnumerable<Author> GetAllNonPendingAuthors();
        public Author GetFromNotPending(string fullName);
    }
}
