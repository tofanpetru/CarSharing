using Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repository.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        void AddBook(Book book, bool isBookPending);
        public Book GetBookById(int id);
        bool RemoveBookById(int? id);
        public void Update(int id, string title, string author, string language, DateTime publishDate, string image);
        public IEnumerable<Book> GetValidBooksWithAssignments();
        public IQueryable<Book> GetAllNonPendingBooks();
        public IEnumerable<Book> GetAllBooksWithDetails(User user);
        public IEnumerable<Book> GetPendingBooks();
        public IEnumerable<Book> GetPagedPendingBooks(int firstItem, int pageSize);
        public IEnumerable<Book> GetAllByAuthor(Author author);
        IEnumerable<Book> GetByTitleAndAuthor(string title, string author);
    }
}
