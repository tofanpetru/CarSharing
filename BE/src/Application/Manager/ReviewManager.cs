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
    public class ReviewManager : IReviewManager
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IUserManager _userManager;
        private readonly IBookRepository _bookRepository;
        private readonly INotificationManager _notificationManager;

        public ReviewManager(IReviewRepository reviewRepository, IMapper mapper, IUserManager userManager, IBookRepository bookRepository,
                             INotificationManager notificationManager)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _userManager = userManager;
            _bookRepository = bookRepository;
            _notificationManager = notificationManager;
        }

        public ICollection<GetReviewDto> GetByBookId(int bookId, int firstItem, int pageSize)
        {
            var bookReviews = _reviewRepository.GetBookReviews(bookId, firstItem, pageSize, _userManager.GetCurrentUser().UserName);
            var model = _mapper.Map<ICollection<GetReviewDto>>(bookReviews);
            return model;
        }

        public bool AddReview(AddReviewDTO reviewDTO)
        {
            if (HasUserBookReview(_userManager.GetCurrentUser().UserName, reviewDTO.BookId))
                return false;
            var review = _mapper.Map<Review>(reviewDTO);
            review.Book = _bookRepository.GetBookById(review.Book.Id);
            if (review.Book == null)
                return false;
            review.PublishDate = DateTime.Today;
            review.User = _userManager.GetCurrentUser();
            _reviewRepository.Add(review);
            _reviewRepository.SaveChanges();
            _notificationManager.AddAdminNotification(String.Format(AppSettings.Instance.Notifications.Admin.NewReview, review.User.UserName, review.Book.Title),
                                                      AppSettings.Instance.Notifications.Admin.NewReviewActionPath, reviewId: review.Id);
            return true;
        }

        public float GetRatingsByBookId(int bookId)
        {
            var reviews = _reviewRepository.GetBookRatings(bookId);
            return (float)reviews.Sum() / reviews.Count() * 20;
        }

        public bool HasUserBookReview(string userName, int bookid)
        {
            return _reviewRepository.HasUserBookReview(userName, bookid);
        }

        public GetReviewDto GetByUsernameAndBookId(string userName, int bookId)
        {
            return _mapper.Map<GetReviewDto>(_reviewRepository.GetUserReviewByBookId(userName, bookId));
        }

        public ICollection<ReviewListDTO> GetAllReviewsList(int firstItem, int pageSize)
        {
            return _mapper.Map<ICollection<ReviewListDTO>>(_reviewRepository.GetPagedAllReviews(firstItem, pageSize));
        }

        public void DeleteReview(int id)
        {
            _reviewRepository.DeleteReview(id);
            _reviewRepository.SaveChanges();
        }
    }
}
