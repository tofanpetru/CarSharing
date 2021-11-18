using Infrastructure.Persistance;
using Infrastructure.Repository.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Application.Managers
{
    [TestFixture]
    class ReviewManagerTests
    {
        Mock<IReviewRepository> _reviewRepository;

        [SetUp]
        public void SetUp()
        {
            _reviewRepository = new Mock<IReviewRepository>();
        }

        [Test]
        public void GetRatingsByBookId_Test()
        {
            //Arrange
            var authors = new List<Author> { new Author { Id = 1, FullName = "Agatha Christie", IsPending = false },
                                             new Author { Id = 2, FullName = "Barbara Cartland", IsPending = false } };
            var language = new Language { Id = 1, Name = "English" };
            var books = new List<Book> { new Book { Id = 1, Author = authors[0], Language = language, Title = "Book1" },
                                         new Book { Id = 2, Author = authors[1], Language = language, Title = "Book2" } };
            var reviews = new List<Review> { new Review { Id = 1, Title = "Review 1", Content = "Review 1 Content", Rating = 5, Book = books[0] },
                                             new Review { Id = 2, Title = "Review 2", Content = "Review 2 Content", Rating = 1, Book = books[1] },
                                             new Review { Id = 3, Title = "Review 3", Content = "Review 3 Content", Rating = 2, Book = books[0] },
                                             new Review { Id = 4, Title = "Review 4", Content = "Review 4 Content", Rating = 3, Book = books[0] },
                                             new Review { Id = 5, Title = "Review 5", Content = "Review 5 Content", Rating = 3, Book = books[0] },
                                             new Review { Id = 6, Title = "Review 6", Content = "Review 6 Content", Rating = 4, Book = books[1] },
                                             new Review { Id = 7, Title = "Review 7", Content = "Review 7 Content", Rating = 1, Book = books[0] },};
            var bookId = 1;
            float totalRating = 0;
            _reviewRepository.Setup(i => i.GetBookRatings(bookId)).Returns(new List<int> { 5, 2, 3, 3, 1 });
            var expectedResult = 56;

            //Act
            var bookReviews = _reviewRepository.Object.GetBookRatings(bookId);
            totalRating = bookReviews.Sum();
            totalRating = totalRating / bookReviews.Count() * 20;

            //Assert
            Assert.AreEqual(expectedResult, totalRating);
        }
    }
}
