using Infrastructure.Persistance;
using Infrastructure.Repository;
using Infrastructure.Repository.Implementations;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Infrastructure.Repository
{
    [TestFixture]
    class AuthorRepositoryTests
    {
        DbContextOptions<BookSharingContext> _bookSharingContext;
        BookSharingContext _dbContext;
        AuthorRepository _authorRepository;

        [SetUp]
        public void SetUp()
        {
            _bookSharingContext = new DbContextOptionsBuilder<BookSharingContext>()
                                        .UseInMemoryDatabase(databaseName: "BookSharingDev").Options;
            _dbContext = new BookSharingContext(_bookSharingContext);
            _authorRepository = new AuthorRepository(_dbContext);
        }

        public void CreateContext()
        {
            _dbContext.Authors.Add(new Author { FullName = "Agatha Christie", IsPending = false });
            _dbContext.Authors.Add(new Author { FullName = "Barbara Cartland", IsPending = false });
            _dbContext.Authors.Add(new Author { FullName = "Danielle Steel", IsPending = true });
            _dbContext.Authors.Add(new Author { FullName = "Gilbert Patten", IsPending = true });
            _dbContext.SaveChanges();
        }

        public void ClearContext()
        {
            var authors = _dbContext.Authors.ToList();
            foreach(var author in authors)
            {
                _dbContext.Remove(author);
            }
            _dbContext.SaveChanges();
        }

        [Test]
        public void GetAllNonPendingAuthors_Test()
        {
            //Arrange
            CreateContext();
            var expectingResult = new List<Author> { new Author { Id = 1, FullName = "Agatha Christie", IsPending = false },
                                                     new Author { Id = 2, FullName = "Barbara Cartland", IsPending = false }};

            //Act
            var nonPendingAuthors = _authorRepository.GetAllNonPendingAuthors().ToList();

            //Assert
            Assert.AreEqual(expectingResult.Count, nonPendingAuthors.Count);
            ClearContext();
        }

        [Test]
        public void FindByName_Test()
        {
            //Arrange
            CreateContext();
            var existingAuthor = "Agatha Christie";
            var nonExistingAuthor = "Enid Blyton";
            var existingAuthorResult = 1;
            var nonExistingAuthorResult = 0;

            //Act
            var result1 = _authorRepository.FindByName(existingAuthor);
            var result2 = _authorRepository.FindByName(nonExistingAuthor);

            //Assert
            Assert.AreEqual(existingAuthorResult, result1);
            Assert.AreEqual(nonExistingAuthorResult, result2);
            ClearContext();
        }
    }
}
