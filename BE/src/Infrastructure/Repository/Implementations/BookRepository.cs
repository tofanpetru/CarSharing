using Infrastructure.Repository.Interfaces;
using Infrastructure.Persistance;
using Infrastructure.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repository.Implementations
{
    public class BookRepository : AbstractRepository<Book>, IBookRepository
    {
        public BookRepository(BookSharingContext context) : base(context) { }

        public void AddBook(Book book, bool isBookPending)
        {
            book.Language = DataBaseContext.Languages.Where(i => i.Name == book.Language.Name).FirstOrDefault();
            book.Owner = DataBaseContext.Users.Find(book.Owner.Id);
            book.Author = DataBaseContext.Authors.Where(i => i.FullName == book.Author.FullName).FirstOrDefault();
            book.IsPending = isBookPending;
            var genres = new List<Genre>();
            foreach (var genre in book.Genres)
            {
                genres.Add(DataBaseContext.Genres.Where(i => i.Name == genre.Name).FirstOrDefault());
            }
            book.Genres = genres;
            book.IsAvailable = !isBookPending;
            Add(book);
        }

        public Book GetBookById(int id)
        {
            return DataBaseContext.Books.Where(x => x.Id == id).Include(x => x.Language).Include(x => x.Author).Include(x => x.Owner).Include(x=>x.Genres).FirstOrDefault();
        }

        public void Update(int id, string title, string authorName, string languageName, DateTime publishDate, string image)
        {
            var book = GetBookById(id);
            var users = DataBaseContext.Users.Where(u => u.Books.Contains(book)).FirstOrDefault();
            var author = DataBaseContext.Authors.Where(x => x.FullName == authorName).FirstOrDefault();
            var language = DataBaseContext.Languages.Where(x => x.Name == languageName).FirstOrDefault();
            book.Title = title;
            book.Author = author;
            book.Language = language;
            book.PublishDate = publishDate;
            if (image != null)
                book.ImagePath = image;
            DataBaseContext.SaveChanges();
        }

        public bool RemoveBookById(int? id)
        {          
            var book = DataBaseContext.Books.Find(id);
            Remove(book);
            DataBaseContext.SaveChanges();
            return true;
        }

        public IEnumerable<Book> GetAllBooksWithDetails(User user)
        {
            return DataBaseContext.Books.Where(i=>i.Owner == user).Include(x => x.Language).Include(x => x.Author).Include(x => x.Owner).Include(x => x.Genres);
        }

        public IEnumerable<Book> GetValidBooksWithAssignments()
        {
            return DataBaseContext.Books.Where(b => b.IsPending == false).Include(t => t.Assignments).ThenInclude(t => t.Extend).Include(t => t.Assignments).ThenInclude(t => t.Assignee);
        }

        public IQueryable<Book> GetAllNonPendingBooks()
        {
            return DataBaseContext.Books.Where(b => !b.IsPending).Include(x => x.Language).Include(x => x.Author).Include(x => x.Owner).Include(x => x.Genres);
        }

        public IEnumerable<Book> GetPendingBooks()
        {
            return DataBaseContext.Books.Include(x => x.Author).Include(x => x.Owner).Where(x => x.IsPending == true);
        }

        public IEnumerable<Book> GetPagedPendingBooks(int firstItem, int pageSize)
        {
            return DataBaseContext.Books.Where(x => x.IsPending == true).Skip(firstItem).Take(pageSize).Include(x => x.Author).Include(x => x.Owner);
        }

        public IEnumerable<Book> GetAllByAuthor(Author author)
        {
            return DataBaseContext.Books.Where(x => x.Author == author).Include(x => x.Author);
        }

        public IEnumerable<Book> GetByTitleAndAuthor(string title,string author)
        {
            return DataBaseContext.Books.Where(x => x.Title == title && x.Author.FullName == author);
        }
    }
}
