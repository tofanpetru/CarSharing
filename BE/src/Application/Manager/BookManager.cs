using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistance;
using Infrastructure.Repository.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using X.PagedList;

namespace Application.Manager
{
    public class BookManager : IBookManager
    {
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly INotificationRepository _notificationRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorManager _authorManager;
        private readonly IGenreManager _genreManager;
        private readonly ILanguageManager _languageManager;
        private readonly IUserManager _userManager;
        private readonly IWishBookManager _wishBookManager;
        private readonly INotificationManager _notificationManager;

        public BookManager(IBookRepository bookRepository, IMapper mapper, IAuthorManager authorManager, IGenreManager genreManager,
                           ILanguageManager languageManager, IHostEnvironment hostEnvironment, INotificationRepository notificationRepository,
                           IUserManager userManager, IWishBookManager wishBookManager, INotificationManager notificationManager)
        {
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
            _notificationRepository = notificationRepository;
            _bookRepository = bookRepository;
            _authorManager = authorManager;
            _genreManager = genreManager;
            _languageManager = languageManager;
            _userManager = userManager;
            _wishBookManager = wishBookManager;
            _notificationManager = notificationManager;
        }

        public async Task<bool> UploadBook(AddBookDTO addBookDTO)
        {
            try
            {
                addBookDTO.Title = Regex.Replace(addBookDTO.Title, @"\s+", " ").Trim();
                if (_genreManager.ValidateGenres(addBookDTO.Genres) && _languageManager.ValidateLanguage(addBookDTO.Language))
                {
                    addBookDTO.Title = addBookDTO.Title.Trim();
                    var book = _mapper.Map<Book>(addBookDTO);
                    book.Owner = _userManager.GetCurrentUser();
                    var partialPath = Settings.GetBookCoversPath(_hostEnvironment);
                    book.ImagePath = await ServerUtils.UploadImage(addBookDTO.Image, partialPath);
                    if (_authorManager.IsExistingAuthor(addBookDTO.Author))
                    {
                        if (_authorManager.IsPending(book.Author.FullName))
                        {
                            _bookRepository.AddBook(book, true);
                            _bookRepository.SaveChanges();
                        }
                        else
                        {
                            _bookRepository.AddBook(book, false);
                            _bookRepository.SaveChanges();
                            _wishBookManager.RemoveExistingBookFromWishList(book.Title, book.Author.FullName, book.Id);
                        }
                        return true;
                    }
                    else
                    {
                        var author = new Author { FullName = book.Author.FullName, IsPending = true };
                        _authorManager.Add(author);
                        _bookRepository.AddBook(book, true);
                        _bookRepository.SaveChanges();
                        _notificationManager.AddAdminNotification(String.Format(AppSettings.Instance.Notifications.Admin.PendingAuthor, book.Owner.UserName, author.FullName),
                                                                  AppSettings.Instance.Notifications.Admin.PendingAuthorActionPath, pendingAuthorId: book.Author.Id);
                        return true;
                    }
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public BookDetailsDTO GetBookDetails(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if(book != null)
            {
                book.ImagePath = ServerUtils.GetBookCoverImageSrc(book.ImagePath);
                return _mapper.Map<BookDetailsDTO>(book);
            }
            else
                return null;
        }

        public BookUpdateDTO GetBookToUpdate(int id)
        {
            return _mapper.Map<BookUpdateDTO>(_bookRepository.GetBookById(id));
        }

        public async Task<bool> Update(ResponseEditBookDTO responseBook)
        {
            try
            {
                responseBook.Title = Regex.Replace(responseBook.Title, @"\s+", " ").Trim();
                if (_genreManager.ValidateGenres(responseBook.Genres) && _languageManager.ValidateLanguage(responseBook.Language))
                {
                    responseBook.Title = responseBook.Title.Trim();
                    var partialPath = Settings.GetBookCoversPath(_hostEnvironment);
                    var book = _bookRepository.GetBookById(responseBook.Id);
                    if (responseBook.Image != null)
                        book.ImagePath = await ServerUtils.UploadImage(responseBook.Image, partialPath);
                    book.Title = responseBook.Title;
                    book.Author = _authorManager.GetByName(responseBook.Author);
                    book.Language = _languageManager.GetByName(responseBook.Language);
                    var genres = new List<Genre>();
                    foreach (var genre in responseBook.Genres)
                        genres.Add(_genreManager.GetByName(genre));
                    book.Genres = genres;
                    book.PublishDate = responseBook.PublishDate;
                    _bookRepository.SaveChanges();
                    _wishBookManager.RemoveExistingBookFromWishList(book.Title, book.Author.FullName, book.Id);
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteBook(int id)
        {
            try
            {
                var book = _bookRepository.GetBookById(id);
                if (book != null && book.IsPending)
                {
                    if (_authorManager.GetAuthorBooksNumber(book.Author.Id) <= 1)
                        _authorManager.RemoveAuthorById(book.Author.Id);
                    return _bookRepository.RemoveBookById(id);
                }
                else
                    return _bookRepository.RemoveBookById(id);
            }
            catch
            {
                return false;
            }
        }

        public IPagedList<AllBooksDto> GetAllPagedUserBooks(User user, int page)
        {
            var userBooks = _bookRepository.GetAllBooksWithDetails(user);
            var paginatedBooks = userBooks.ToPagedList((page < 1) ? 1 : page, AppSettings.Instance.BooksPageSize);
            return _mapper.Map<IPagedList<Book>, IPagedList<AllBooksDto>>(paginatedBooks);
        }

        public ICollection<PendingRequestsDTO> GetPendingBooks()
        {
            return _mapper.Map<ICollection<PendingRequestsDTO>>(_bookRepository.GetPendingBooks());
        }

        public void AddBookFromPending(int id, string author)
        {
            var pendingBook = _bookRepository.GetBookById(id);
            Author oldAuthor = pendingBook.Author;
            var relatedBooks = GetAllByAuthor(oldAuthor);
            if (pendingBook.Author.FullName.ToLower() != author.ToLower())
            {
                if (!_authorManager.IsExistingAuthor(author))
                {
                    var newAuthor = _authorManager.GetByName(_authorManager.AddAuthorNotPending(author));
                    _notificationRepository.RemoveBy(oldAuthor);
                    ChangeBooksAuthor(relatedBooks, newAuthor);
                    RemoveBooksFromPending(relatedBooks);
                    _bookRepository.SaveChanges();
                    _authorManager.DeleteAuthor(oldAuthor);
                }

            }
            else if (_authorManager.ExistsNotPending(pendingBook.Author.FullName))
            {
                var newAuthor = _authorManager.GetFromNotPending(pendingBook.Author.FullName);
                _notificationRepository.RemoveBy(oldAuthor);
                ChangeBooksAuthor(relatedBooks, newAuthor);
                RemoveBooksFromPending(relatedBooks);
            }
            else
            {
                _authorManager.AddAuthorFromPending(pendingBook.Author);
                RemoveBooksFromPending(relatedBooks);
                _notificationRepository.RemoveBy(oldAuthor);
            }
            _wishBookManager.RemoveExistingBookFromWishList(pendingBook.Title, pendingBook.Author.FullName, id);
            _bookRepository.SaveChanges();
        }

        public bool UpdateAuthorInPendingBook(int id, string newAuthorName)
        {
            var pendingBook = _bookRepository.GetBookById(id);
            if (_authorManager.UpdatePendingAuthor(pendingBook.Author, newAuthorName))
                return true;
            else
                return false;
        }

        public IPagedList<AllBooksDto> GetPagedFilteredBooks(BookSearch bookSearch, CheckBoxFilter checkBoxFilter, BookRating bookRating, int page)
        {
            var books = _bookRepository.GetAllNonPendingBooks();
            try
            {
                books = SearchAndFilterByAuthor(bookSearch, checkBoxFilter, books);
                books = SearchAndFilterByGenre(bookSearch, checkBoxFilter, books);
                books = SearchAndFilterByLanguage(bookSearch, checkBoxFilter, books);
                books = FilterByBookStatus(books, checkBoxFilter);
                books = FilterByRating(books, bookRating);
            }
            catch (NullReferenceException) { }
            var paginatedBooks = books.ToPagedList((page < 1) ? 1 : page, AppSettings.Instance.BooksPageSize);
            return _mapper.Map<IPagedList<Book>, IPagedList<AllBooksDto>>(paginatedBooks);
        }

        private static IQueryable<Book> FilterByBookStatus(IQueryable<Book> books, CheckBoxFilter checkBoxFilter)
        {
            if (checkBoxFilter != null && checkBoxFilter.BookStatuses != null)
            {
                if (checkBoxFilter.BookStatuses.Contains(BookStatus.Available.ToString()) && checkBoxFilter.BookStatuses.Contains(BookStatus.Busy.ToString()))
                {
                    return books.OrderBy(x => x.Title);
                }
                else
                {
                    if (checkBoxFilter.BookStatuses.Contains(BookStatus.Available.ToString()))
                    {
                        return books.Where(a => a.IsAvailable == true).OrderBy(x => x.Title);
                    }
                    else if (checkBoxFilter.BookStatuses.Contains(BookStatus.Busy.ToString()))
                    {
                        return books.Where(a => a.IsAvailable == false).OrderBy(x => x.Title);
                    }
                }
            }
            return books;
        }

        private static IQueryable<Book> SearchAndFilterByAuthor(BookSearch bookSearch, CheckBoxFilter checkBoxFilter, IQueryable<Book> books)
        {
            var authorFilters = new List<string>();

            if (checkBoxFilter != null && checkBoxFilter.CheckBoxAuthors != null)
                foreach (var author in checkBoxFilter.CheckBoxAuthors)
                    authorFilters.Add(author.ToUpper());

            if (!string.IsNullOrEmpty(bookSearch.ByAuthor))
            {
                if (authorFilters.Any())
                    books = books.Where(a => authorFilters.Contains(a.Author.FullName.ToUpper()) || a.Author.FullName.ToUpper().Contains(bookSearch.ByAuthor.ToUpper()))
                                 .OrderBy(x => x.Author.FullName);
                else
                    books = books.Where(a => a.Author.FullName.ToUpper().Contains(bookSearch.ByAuthor.ToUpper())).OrderBy(x => x.Author.FullName);
            }
            else
            {
                if (authorFilters.Any())
                    books = books.Where(a => authorFilters.Contains(a.Author.FullName.ToUpper())).OrderBy(x => x.Author.FullName);
            }
            return books;
        }

        private static IQueryable<Book> SearchAndFilterByGenre(BookSearch bookSearch, CheckBoxFilter checkBoxFilter, IQueryable<Book> books)
        {
            var genreFilters = new List<string>();
            if (checkBoxFilter.CheckBoxGenres != null)
                foreach (var author in checkBoxFilter.CheckBoxGenres)
                    genreFilters.Add(author.ToUpper());
            if (!string.IsNullOrEmpty(bookSearch.ByGenre))
            {
                if (genreFilters.Any())
                    books = books.Where(a => a.Genres.Any(g => genreFilters.Any(gf => gf == g.Name.ToUpper())) || a.Genres.Any(x => x.Name.ToUpper().Contains(bookSearch.ByGenre.ToUpper())));
                else
                    books = books.Where(a => a.Genres.Any(g => g.Name.Contains(bookSearch.ByGenre.ToUpper()))).OrderBy(x => x.Title);
            }
            else
                if (genreFilters.Any())
                books = books.Where(a => a.Genres.Any(g => genreFilters.Any(gf => gf == g.Name.ToUpper()))).OrderBy(x => x.Title);
            return books;
        }

        private static IQueryable<Book> SearchAndFilterByLanguage(BookSearch bookSearch, CheckBoxFilter checkBoxFilter, IQueryable<Book> books)
        {
            var languageFilters = new List<string>();
            if (checkBoxFilter.CheckBoxLanguages != null)
                foreach (var language in checkBoxFilter.CheckBoxLanguages)
                    languageFilters.Add(language.ToUpper());
            if (!string.IsNullOrEmpty(bookSearch.ByLanguage))
            {
                if (languageFilters.Any())
                    books = books.Where(a => languageFilters.Contains(a.Language.Name.ToUpper()) || a.Language.Name.ToUpper().Contains(bookSearch.ByLanguage.ToUpper()))
                                 .OrderBy(x => x.Language.Name);
                else
                    books = books.Where(a => a.Language.Name.ToUpper().Contains(bookSearch.ByLanguage.ToUpper())).OrderBy(x => x.Language.Name);
            }
            else
            {
                if (languageFilters.Any())
                    books = books.Where(a => languageFilters.Contains(a.Language.Name.ToUpper())).OrderBy(x => x.Language.Name);
            }
            return books;
        }

        private static IQueryable<Book> FilterByRating(IQueryable<Book> books, BookRating bookRating)
        {
            if (bookRating.Rating != 0)
            {
                books = books.Where(b => Math.Round(b.Reviews.Average(rev => rev.Rating)) == bookRating.Rating);
            }
            return books;
        }

        public ICollection<PendingRequestsDTO> GetPagedPendingBooksAuthors(int firstItem, int pageSize)
        {
            return _mapper.Map<ICollection<PendingRequestsDTO>>(_bookRepository.GetPagedPendingBooks(firstItem, pageSize));
        }

        public IEnumerable<Book> GetAllByAuthor(Author author)
        {
            return _bookRepository.GetAllByAuthor(author);
        }

        public static void ChangeBooksAuthor(IEnumerable<Book> books, Author newAuthor)
        {
            foreach (Book book in books)
                book.Author = newAuthor;
        }

        public static void RemoveBooksFromPending(IEnumerable<Book> books)
        {
            foreach (Book book in books)
            {
                book.IsPending = false;
                book.IsAvailable = true;
            }
        }
    }
}