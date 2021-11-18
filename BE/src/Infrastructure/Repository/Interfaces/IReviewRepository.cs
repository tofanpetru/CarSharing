using Infrastructure.Persistance;
using System.Collections.Generic;

namespace Infrastructure.Repository.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        public IEnumerable<Review> GetBookReviews(int bookId, int firstItem, int pageSize, string currentUserName);
        public IEnumerable<int> GetBookRatings(int bookId);
        public bool HasUserBookReview(string userName, int bookId);
        public Review GetUserReviewByBookId(string userName, int bookId);
        public IEnumerable<Review> GetPagedAllReviews(int firstItem, int pageSize);
        public void DeleteReview(int id);
    }
}
