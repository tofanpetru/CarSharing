using Application;
using AutoMapper;
using NUnit.Framework;

namespace UnitTests.Application
{
    [TestFixture]
    class MappingProfilesTests
    {
        [Test]
        public void AutoMapper_Configuration_isValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            config.AssertConfigurationIsValid();
        }
    }
}
