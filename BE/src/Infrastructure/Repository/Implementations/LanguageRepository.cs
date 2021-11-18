using Infrastructure.Persistance;
using Infrastructure.Repository.Abstract;
using Infrastructure.Repository.Interfaces;
using System.Linq;

namespace Infrastructure.Repository.Implementations
{
    public class LanguageRepository : AbstractRepository<Language>, ILanguageRepository
    {
        public LanguageRepository(BookSharingContext context) : base(context) { }

        public Language GetByName(string language)
        {
            return DataBaseContext.Languages.Where(x => x.Name == language).FirstOrDefault();
        }
    }
}
