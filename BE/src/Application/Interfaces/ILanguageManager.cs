using Domain.Entities;
using Infrastructure.Persistance;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ILanguageManager
    {
        ICollection<LanguageDTO> GetAll();
        bool ValidateLanguage(string language);
        public List<string> GetLanguagesByName(string term);
        public Language GetByName(string language);
    }
}
