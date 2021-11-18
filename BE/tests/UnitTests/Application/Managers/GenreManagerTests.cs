using Infrastructure.Persistance;
using Infrastructure.Repository.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Application.Managers
{
    [TestFixture]
    class GenreManagerTests
    {
        Mock<IGenreRepository> _genreRepository;

        [SetUp]
        public void SetUp()
        {
            _genreRepository = new Mock<IGenreRepository>();
        }

        [Test]
        public void ValidateGenre_TestIfAtLeastOneGenreDoesNotExist()
        {
            //Arrange
            var genres = new List<Genre>() { new Genre { Id = 1, Name = "Fantasy" },
                                             new Genre { Id = 2, Name = "Sci-Fi" },
                                             new Genre { Id = 3, Name = "Mystery" } };
            _genreRepository.Setup(i => i.GetAll()).Returns(genres);
            var existingSearchingGenre = new List<string>() { "Fantasy", "Sci-Fi" };
            var nonexistingSearchingGenre = new List<string>() { "Fantasy", "Mystery", "Thriller" };
            var responseExistingGenre = true;
            var responseNonExistingGenre = false;

            //Act
            responseExistingGenre = false;
            var allGenres = _genreRepository.Object.GetAll().Select(i => i.Name);
            foreach (var genre in existingSearchingGenre)
            {
                if (!allGenres.Contains(genre))
                    responseExistingGenre = true;
            }
            foreach (var genre in nonexistingSearchingGenre)
            {
                if (!allGenres.Contains(genre))
                    responseExistingGenre = true;
            }

            //Assert
            Assert.IsTrue(responseExistingGenre);
            Assert.IsFalse(responseNonExistingGenre);
        }
    }
}
