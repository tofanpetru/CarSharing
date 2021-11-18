using Infrastructure.Persistance;
using Infrastructure.Repository.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests.Application.Managers
{
    [TestFixture]
    class AuthorManagerTests
    {
        Mock<IAuthorRepository> _authorRepository;

        [SetUp]
        public void SetUp()
        {
            _authorRepository = new Mock<IAuthorRepository>();
        }

        [Test]
        public void IsPending_Test()
        {
            //Arrange
            var authors = new List<Author>() { new Author { FullName = "Agatha Christie", IsPending = false },
                                             { new Author { FullName = "Barbara Cartland", IsPending = false } },
                                             { new Author { FullName = "Danielle Steel", IsPending = true } },
                                             {  new Author { FullName = "Gilbert Patten", IsPending = true } } };
            _authorRepository.Setup(i => i.GetByName("Agatha Christie")).Returns(authors[0]);
            _authorRepository.Setup(i => i.GetByName("Barbara Cartland")).Returns(authors[2]);
            var author = _authorRepository.Object.GetByName("Agatha Christie");
            var isPendingAuthor = false;
            if (author.IsPending == isPendingAuthor)
                Assert.Pass();
        }
    }
}
