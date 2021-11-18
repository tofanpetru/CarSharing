using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistance;
using Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Manager
{
    public class WishBookManager : IWishBookManager
    {
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;
        private readonly INotificationManager _notificationManager;
        private readonly IWishBookRepository _wishListRepository;
        public WishBookManager(IWishBookRepository wishListRepository, IMapper mapper, IBookRepository bookRepository, INotificationManager notificationManager)
        {
            _wishListRepository = wishListRepository;
            _mapper = mapper;
            _bookRepository = bookRepository;
            _notificationManager = notificationManager;
        }

        public ICollection<WishBookDTO> GetWishedBooks(int firstItem, int pageSize, string sortOrder, string searchString,User user)
        {
            return _mapper.Map<ICollection<WishBookDTO>>(_wishListRepository.GetWishedBooks(firstItem, pageSize,user));
        }

        public WishBookResultDTO AddWishBook(WishBookDTO wishBookDTO,User user)
        {
            var wishBook = _mapper.Map<WishBook>(wishBookDTO);
            var dbWishBook = _wishListRepository.GetWishBookByTitleAndAuthor(wishBook.BookTitle,wishBook.BookAuthor);
            var dbBook = _bookRepository.GetByTitleAndAuthor(wishBook.BookTitle, wishBook.BookAuthor);
            var result = new WishBookResultDTO();
            if (dbBook.Any())
            {
                result.Error = true;
                result.Message = "Book with such title and author already exists";
                return result;
            }
            if (dbWishBook != null)
            {
                if (dbWishBook.Users.Contains(user))
                {
                    result.Error = true;
                    result.Message = "You already have wish book with such title and author";
                }
                dbWishBook.Users.Add(user);

                _wishListRepository.SaveChanges();
                return result;
            }
            wishBook.Users.Add(user);
            _wishListRepository.Add(wishBook);
            _wishListRepository.SaveChanges();
            return result;
        }

        public WishBookResultDTO UpdateWishBook(WishBookDTO wishBook, User user)
        {
            var dbWishBook = _wishListRepository.GetById(wishBook.Id);
            var dbBook = _bookRepository.GetByTitleAndAuthor(wishBook.BookTitle, wishBook.BookAuthor);
            var wishBookUser = _wishListRepository.GetWishBookByTitleAndAuthor(wishBook.BookTitle, wishBook.BookAuthor);
            var result = new WishBookResultDTO();

            if (dbBook.Any())
            {
                result.Error = true;
                result.Message = "Book with such title and author already exists";
                return result;
            }
            if (dbWishBook == null)
            {
                result.Error = true;
                return result;
            }
            if (wishBook.BookAuthor == dbWishBook.BookAuthor && dbWishBook.BookTitle == wishBook.BookTitle )
            {
                return result;
            }
            if (wishBookUser != null && wishBookUser.Users.Contains(user))
            {
                result.Error = true;
                result.Message = "You already have wish book with such title and author";
                return result;
            }
            if (dbWishBook.Users.Count > 1)
            {
                dbWishBook.Users.Remove(user);
                
                _wishListRepository.Add(new WishBook {BookAuthor = wishBook.BookAuthor.Trim(), BookTitle = wishBook.BookTitle.Trim(), Users = new List<User>() {user} });
            }
            else
            {
                dbWishBook.BookAuthor = wishBook.BookAuthor;
                dbWishBook.BookTitle = wishBook.BookTitle;
            }

            _wishListRepository.SaveChanges();

            return result;
        }

        public bool DeleteWishBook(User user,int id)
        {
            var dbWishBook = _wishListRepository.GetById(id);

            if (dbWishBook == null)
            {
                return false;
            }
            if (dbWishBook.Users.Count > 1)
            {
                dbWishBook.Users.Remove(user);
            }
            else
            {
                _wishListRepository.Remove(dbWishBook);
            }

            _wishListRepository.SaveChanges();

            return true;
        }

        public bool RemoveExistingBookFromWishList(string title, string author, int bookId)
        {
            var wishBook = _wishListRepository.GetWishBookByTitleAndAuthor(title, author);
            if (wishBook != null)
            {
                _notificationManager.AddWishedBookNotification(String.Format(AppSettings.Instance.Notifications.User.WishedBookAvailable, title),
                                       String.Format(AppSettings.Instance.Notifications.User.WishedBookAvailableActionPath, bookId), wishBook);
                _wishListRepository.Remove(wishBook);
                _wishListRepository.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
