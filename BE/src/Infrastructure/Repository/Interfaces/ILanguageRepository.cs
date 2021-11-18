using Infrastructure.Persistance;

namespace Infrastructure.Repository.Interfaces
{
    public interface ILanguageRepository : IRepository<Language>
    {
        public Language GetByName(string language);
    }
}
