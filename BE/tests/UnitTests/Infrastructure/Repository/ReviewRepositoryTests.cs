using Infrastructure.Persistance;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Infrastructure.Repository
{
    [TestFixture]
    class ReviewRepositoryTests
    {
        DbContextOptions<BookSharingContext> _bookSharingContext;
        BookSharingContext _dbContext;

        [SetUp]
        public void SetUp()
        {
            _bookSharingContext = new DbContextOptionsBuilder<BookSharingContext>()
                                        .UseInMemoryDatabase(databaseName: "BookSharingDev").Options;
            _dbContext = new BookSharingContext(_bookSharingContext);
        }

        public void CreateContext()
        {
            _dbContext.Authors.Add(new Author { Id = 1, FullName = "Agatha Christie", IsPending = false });
            _dbContext.Authors.Add(new Author { Id = 2, FullName = "Barbara Cartland", IsPending = false });
            _dbContext.Authors.Add(new Author { Id = 3, FullName = "Danielle Steel", IsPending = true });
            _dbContext.Authors.Add(new Author { Id = 4, FullName = "Gilbert Patten", IsPending = true });
            _dbContext.Users.Add(new User { Id = "UserId1", UserName = "User1" });
            _dbContext.Users.Add(new User { Id = "UserId2", UserName = "User2" });
            _dbContext.Users.Add(new User { Id = "UserId3", UserName = "User3" });
            _dbContext.Users.Add(new User { Id = "UserId4", UserName = "User4" });
            _dbContext.Users.Add(new User { Id = "UserId5", UserName = "User5" });
            _dbContext.SaveChanges();
            _dbContext.Languages.Add(new Language { Id = 1, Name = "English" });
            _dbContext.SaveChanges();
            _dbContext.Books.Add(new Book { Id = 1, Author = _dbContext.Authors.Find(1), Language = _dbContext.Languages.Find(1), Title = "Book1", Owner = _dbContext.Users.Find("UserId1") });
            _dbContext.Books.Add(new Book { Id = 2, Author = _dbContext.Authors.Find(2), Language = _dbContext.Languages.Find(1), Title = "Book2", Owner = _dbContext.Users.Find("UserId2") });
            _dbContext.SaveChanges();
            _dbContext.Reviews.Add(new Review { Id = 1, Title = "Review 1", Content = "Review 1 Content", Rating = 5, Book = _dbContext.Books.Find(1), User = _dbContext.Users.Find("UserId1") });
            _dbContext.Reviews.Add(new Review { Id = 2, Title = "Review 2", Content = "Review 2 Content", Rating = 1, Book = _dbContext.Books.Find(2), User = _dbContext.Users.Find("UserId1") });
            _dbContext.Reviews.Add(new Review { Id = 3, Title = "Review 3", Content = "Review 3 Content", Rating = 2, Book = _dbContext.Books.Find(1), User = _dbContext.Users.Find("UserId2") });
            _dbContext.Reviews.Add(new Review { Id = 4, Title = "Review 4", Content = "Review 4 Content", Rating = 3, Book = _dbContext.Books.Find(1), User = _dbContext.Users.Find("UserId3") });
            _dbContext.Reviews.Add(new Review { Id = 5, Title = "Review 5", Content = "Review 5 Content", Rating = 3, Book = _dbContext.Books.Find(1), User = _dbContext.Users.Find("UserId4") });
            _dbContext.Reviews.Add(new Review { Id = 6, Title = "Review 6", Content = "Review 6 Content", Rating = 4, Book = _dbContext.Books.Find(2), User = _dbContext.Users.Find("UserId2") });
            _dbContext.Reviews.Add(new Review { Id = 7, Title = "Review 7", Content = "Review 7 Content", Rating = 1, Book = _dbContext.Books.Find(1), User = _dbContext.Users.Find("UserId5") });
            _dbContext.SaveChanges();
        }

        public void ClearContext()
        {
            var reviews = _dbContext.Reviews.ToList();
            foreach (var review in reviews)
            {
                _dbContext.Remove(review);
            }
            _dbContext.SaveChanges();
        }

        [Test]
        public void GetBookRatings_Test()
        {
            //Arrange
            CreateContext();
            var bookId = 1;
            var expectingResult = new List<int> { 5, 2, 3, 3, 1 };

            //Act
            var bookReviewsRating = _dbContext.Reviews.Where(i => i.Book.Id == bookId).OrderBy(i => i.Id).Select(i => i.Rating).ToList();

            //Assert
            Assert.AreEqual(expectingResult.Count, bookReviewsRating.Count);
            for (int i = 0; i < expectingResult.Count; i++)
                Assert.AreEqual(expectingResult[i], bookReviewsRating[i]);
        }

        [Test]
        public void GetBookReviews_Test()
        {
            //Arrange
            var bookId = 1;
            var firstItem = 0;
            var pageSize = 3;
            var expectingResult = new List<Review> { _dbContext.Reviews.Find(7), _dbContext.Reviews.Find(5), _dbContext.Reviews.Find(4) };

            //Act
            var pagedBookReviews = _dbContext.Reviews.Where(i => i.Book.Id == bookId).OrderByDescending(i => i.Id).Skip(firstItem).Take(pageSize).Include(i => i.Book).Include(i => i.User).ToList();

            //Assert
            Assert.AreEqual(pagedBookReviews.Count, pageSize);
            for (int i = 0; i < pagedBookReviews.Count; i++)
                Assert.AreEqual(pagedBookReviews[i], expectingResult[i]);
        }

        [Test]
        public void GetUserReviewByBookId_Test()
        {
            //Arrange
            var userName = "User1";
            var bookId = 2;
            var expectedReview = _dbContext.Reviews.Find(2);

            //Act
            var actualResult = _dbContext.Reviews.Where(i => i.User.UserName == userName && i.Book.Id == bookId).FirstOrDefault();

            //Assert
            Assert.AreEqual(expectedReview, actualResult);
        }

        [Test]
        public void HasUserBookReview_Test()
        {
            //Arrange
            var userName = "User5";
            var firstBookId = 1;
            var secondBookId = 2;

            //Act
            var result1 = _dbContext.Reviews.Where(i => i.Book.Id == firstBookId).Where(i => i.User.UserName == userName).Any();
            var result2 = _dbContext.Reviews.Where(i => i.Book.Id == secondBookId).Where(i => i.User.UserName == userName).Any();

            //Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }
    }
}