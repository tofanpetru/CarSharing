using Infrastructure.Persistance;
using Infrastructure.Repository.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Application.Managers
{
    [TestFixture]
    class LanguageManagerTests
    {
        Mock<ILanguageRepository> _languageRepository;

        [SetUp]
        public void Setup()
        {
            _languageRepository = new Mock<ILanguageRepository>();
        }

        [Test]
        public void ValidateLanguage_ReturnsTrueIfExists()
        {
            //Arrange
            var languages = new List<Language>() { new Language { Id = 1, Name = "English" },
                                                   new Language { Id = 2, Name = "Romanian" },
                                                   new Language { Id = 3, Name = "Russian" } };
            _languageRepository.Setup(i => i.GetAll()).Returns(languages);
            var existingSearchingLanguage = "English";
            var nonExistingSearchingLanguage = "French";
            var responseExistingLanguage = true;
            var responseNonExistingLanguage = false;

            //Act
            var allLanguages = _languageRepository.Object.GetAll().Select(i => i.Name);
            responseExistingLanguage = false;
            foreach (var language in allLanguages)
                if (language == existingSearchingLanguage)
                    responseExistingLanguage = true;
            foreach (var language in allLanguages)
                if (language == nonExistingSearchingLanguage)
                    responseNonExistingLanguage = true;

            //Assert
            Assert.IsTrue(responseExistingLanguage);
            Assert.IsFalse(responseNonExistingLanguage);
        }
    }
}
