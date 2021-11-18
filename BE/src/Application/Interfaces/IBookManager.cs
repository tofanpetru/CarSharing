using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistance;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace Application.Interfaces
{
    public interface IBookManager
    {
        bool DeleteBook(int id);
        public Task<bool> UploadBook(AddBookDTO addBookDTO);
        public BookDetailsDTO GetBookDetails(int id);
        public BookUpdateDTO GetBookToUpdate(int id);
        public Task<bool> Update(ResponseEditBookDTO responseBook);
        public IPagedList<AllBooksDto> GetAllPagedUserBooks(User user, int page);
        public void AddBookFromPending(int id, string author);
        public bool UpdateAuthorInPendingBook(int id, string newAuthorName);
        public IPagedList<AllBooksDto> GetPagedFilteredBooks(BookSearch bookSearch, CheckBoxFilter checkBoxFilter, BookRating bookRating, int page);
        public ICollection<PendingRequestsDTO> GetPendingBooks();
        public ICollection<PendingRequestsDTO> GetPagedPendingBooksAuthors(int firstItem, int pageSize);
        public IEnumerable<Book> GetAllByAuthor(Author author);
    }
}
