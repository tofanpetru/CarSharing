using Domain.Entities;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IReviewManager
    {
        public ICollection<GetReviewDto> GetByBookId(int bookId, int firstItem, int pageSize);
        public float GetRatingsByBookId(int bookId);
        public bool HasUserBookReview(string userName, int bookid);
        public GetReviewDto GetByUsernameAndBookId(string userName, int bookId);
        public bool AddReview(AddReviewDTO reviewDTO);
        public ICollection<ReviewListDTO> GetAllReviewsList(int firstItem, int pageSize);
        public void DeleteReview(int id);
    }
}
