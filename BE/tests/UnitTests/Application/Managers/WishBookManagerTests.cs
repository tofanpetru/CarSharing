using Infrastructure.Persistance;
using Infrastructure.Repository.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Application.Managers
{
    [TestFixture]
    class WishBookManagerTests
    {
        Mock<IWishBookRepository> _wishListRepository;

        [SetUp]
        public void SetUp()
        {
            _wishListRepository = new Mock<IWishBookRepository>();
        }

        [Test]
        public void GetPartialWishedBooks_Test()
        {
            //Arrange
            var wishLists = new List<WishBook> { new WishBook { Id = 1, BookTitle = "Book 1" },
                                                new WishBook { Id = 2, BookTitle = "Book 2" },
                                                new WishBook { Id = 3, BookTitle = "Book 3" },
                                                new WishBook { Id = 4, BookTitle = "Book 4" },
                                                new WishBook { Id = 5, BookTitle = "Book 5" },
                                                new WishBook { Id = 6, BookTitle = "Book 6" },
                                                new WishBook { Id = 7, BookTitle = "Book 7" },
                                                new WishBook { Id = 8, BookTitle = "Book 8" },
                                                new WishBook { Id = 9, BookTitle = "Book 9" },
                                                new WishBook { Id = 10, BookTitle = "Book 10" }};
            var pageSize = 5;
            var firstItem = 0;
            _wishListRepository.Setup(i => i.GetAllWishedBooks()).Returns(wishLists);

            //Act
            var wishList = _wishListRepository.Object.GetAllWishedBooks().ToList();
            var model = wishList.Skip(firstItem).Take(pageSize).ToList();

            //Assert
            for (int i = 0; i < model.Count; i++)
            {
                Assert.AreEqual(model[i].Id - 1, i);
            }
            Assert.AreEqual(model.Count, pageSize);
        }
    }
}
