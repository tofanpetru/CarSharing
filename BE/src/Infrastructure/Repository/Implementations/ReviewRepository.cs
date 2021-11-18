using Infrastructure.Persistance;
using Infrastructure.Repository.Abstract;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repository.Implementations
{
    public class ReviewRepository : AbstractRepository<Review>, IReviewRepository
    {
        public ReviewRepository(BookSharingContext context) : base(context) { }

        public IEnumerable<Review> GetBookReviews(int bookId, int firstItem, int pageSize, string currentUserName)
        {
            return DataBaseContext.Reviews.Where(i => i.Book.Id == bookId && i.User.UserName != currentUserName).OrderByDescending(i => i.PublishDate).Skip(firstItem).Take(pageSize).Include(i => i.Book).Include(i => i.User);
        }

        public IEnumerable<int> GetBookRatings(int bookId)
        {
            return DataBaseContext.Reviews.Where(i => i.Book.Id == bookId).Select(i => i.Rating);
        }

        public bool HasUserBookReview(string userName, int bookId)
        {
            return DataBaseContext.Reviews.Where(i => i.Book.Id == bookId && i.User.UserName == userName).Any();
        }

        public Review GetUserReviewByBookId(string userName, int bookId)
        {
            return DataBaseContext.Reviews.Where(i => i.User.UserName == userName && i.Book.Id == bookId).FirstOrDefault();
        }

        public IEnumerable<Review> GetPagedAllReviews(int firstItem, int pageSize)
        {
            return DataBaseContext.Reviews.OrderByDescending(i => i.Id).Skip(firstItem).Take(pageSize).Include(i => i.Book).Include(i => i.User);
        }

        public void DeleteReview(int id)
        {
            DataBaseContext.Reviews.Remove(DataBaseContext.Reviews.Find(id));
        }
    }
}
